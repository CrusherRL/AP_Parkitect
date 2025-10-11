using Archipelago.MultiClient.Net;
using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Config;
using ArchipelagoMod.Src.Connector;
using ArchipelagoMod.Src.SlotData;
using ArchipelagoMod.Src.Window;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArchipelagoMod.Src.Controller
{
    class ArchipelagoController : MonoBehaviour
    {
        ParkitectController ParkitectController = null;
        ArchipelagoConnector ArchipelagoConnector = null;
        ArchipelagoWindow ArchipelagoWindow = null;
        ParkitectAPConfig ParkitectAPConfig = null;
        SaveData SaveData = null;

        private Dictionary<string, object> SlotData = null;
        private readonly Queue<AP_Item> PendingAPItems = new Queue<AP_Item>();
        private readonly Queue<long> PendingLocations= new Queue<long>();

        protected bool IsReady = false;
        protected string Game = "Parkitect";

        public List<Challenge> Challenges = new List<Challenge>();

        void Start ()
        {
            Helper.Debug("[ArchipelagoController::Start]");
            this.ParkitectController = GetComponent<ParkitectController>();
            this.InitAPConfig();

            if (this.ParkitectAPConfig == null)
            {
                this.ParkitectController.SendMessage("Archipelago Config missing. Can be found here", ParkitectAPConfig.GetConfigFilePath());
                Helper.Debug("[ArchipelagoController::Start] -> Missing AP Config");
                return;
            }

            this.ArchipelagoWindow = GetComponent<ArchipelagoWindow>();

            if (!this.ParkitectController.PlayerIsInCorrectMode())
            {
                return;
            }

            Helper.Debug("[ArchipelagoController::Start] -> Booted");
        }

        void Update ()
        {
            if (this.ArchipelagoConnector == null && GameController.Instance.loadingHasBeenCompleted)
            {
                this.ConnectWithArchipelago();
            }

            //this.ParkitectController.PlayerCheckHasWon();
        }

        private void ConnectWithArchipelago ()
        {
            this.ArchipelagoConnector = new ArchipelagoConnector(this.ParkitectAPConfig, this.Game);
            this.Listen();
            this.ArchipelagoConnector.ConnectAsync();
        }

        private void InitAPConfig ()
        {
            if (!ParkitectAPConfig.HasConfigFile())
            {
                ParkitectAPConfig.CreateConfig();
                this.ParkitectController.SendMessage(
                    "Parkitect config file for Archipelago not found",
                    "Created a new file here: " + ParkitectAPConfig.GetConfigFilePath()
                );
                return;
            }

            this.ParkitectAPConfig = ParkitectAPConfig.Load();
        }

        protected void Listen ()
        {
            Helper.Debug("[ArchipelagoController::Listen]");
            this.ArchipelagoConnector.OnConnected += (LoginSuccessful success) => {
                Helper.Debug("[ArchipelagoController::Listen] -> OnConnected");
                this.OnConnected();
                this.SlotData = success.SlotData;
                this.Handle();
            };

            this.ArchipelagoConnector.OnConnectionFailed += () => {
                if (this.ArchipelagoConnector.Session.Socket.Connected)
                {
                    Helper.Debug("[ArchipelagoController::Listen] -> OnConnectionFailed");
                    this.OnDisconnect();
                }
            };

            this.ArchipelagoConnector.OnReconnected += () => {
                this.OnConnecting();
                Helper.Debug($"[ArchipelagoController::Listen] -> OnReconnected - Retry in : { this.ArchipelagoConnector.Retry / 1000} seconds");
            };

            // Won the Scenario :)
            EventManager.Instance.OnScenarioWon += this.GoalAchieved;

            this.ArchipelagoConnector.OnItemReceived += this.OnReceivedItem;
        }

        private void OnReceivedItem (string prefabItem, int finishedLocationId)
        {
            string serializedName = this.ParkitectController.GetSerializedFromPrefabs(prefabItem);
            AP_Item AP_Item = AP_Item.Init(prefabItem, finishedLocationId, serializedName);

            if (!this.IsReady)
            {
                Helper.Debug($"[ArchipelagoController::OnReceivedItem] -> Queue Item");
                this.PendingAPItems.Enqueue(AP_Item);
                return;
            }

            this.HandleItem(AP_Item);
        }

        public void ProcessPendingItems()
        {
            Helper.Debug($"[ArchipelagoController::ProcessPendingItems]");
            while (this.IsReady && this.PendingAPItems.Count > 0)
            {
                this.HandleItem(this.PendingAPItems.Dequeue());
            }
        }
     
        public void ProcessPendingLocations()
        {
            Helper.Debug($"[ArchipelagoController::ProcessPendingLocations]");
            if (this.IsOffline() || !this.IsReady) {
                return;
            }
            
            while (this.PendingLocations.Count > 0)
            {
                this.CompleteLocation(this.PendingLocations.Dequeue());
            }
        }

        private void HandleItem(AP_Item AP_Item)
        {
            Helper.Debug($"[ArchipelagoController::HandleItem] -> Handling Item - {AP_Item.Name}");

            if (AP_Item.IsTrap)
            {
                this.ParkitectController.PlayerRedeemTrap(AP_Item);
            }
            else if (!this.ParkitectController.PlayerHasUnlockedItem(AP_Item))
            {
                this.ParkitectController.PlayerUnlockItem(AP_Item);
                this.ParkitectController.SendMessage($"You Received: \"{AP_Item.SerializedName}\"");
            }

            if (AP_Item.LocationId < 0)
            {
                return;
            }

            this.ArchipelagoWindow.NextChallenge(AP_Item.LocationId);
        }

        public void OnDisconnect ()
        {
            this.ArchipelagoWindow.SetStatus(_Status.States.DISCONNECTED);
        }

        public void OnConnecting ()
        {
            //this.ArchipelagoWindow.SetStatus(_Status.States.CONNECTING);
        }

        public void OnConnected ()
        {
            this.ArchipelagoWindow.SetStatus(_Status.States.CONNECTED);
        }

        public bool IsOffline ()
        {
            return this.ArchipelagoConnector.Session == null || this.ArchipelagoConnector.Session.Locations == null || this.ArchipelagoConnector.Session.Locations.AllLocations == null;
        }

        private void Handle ()
        {
            if (this.SlotData == null)
            {
                return;
            }

            Helper.Debug(JsonConvert.SerializeObject(this.SlotData, Formatting.Indented), "slot_data.json");

            this.HandleChallenges();
            this.HandleScenario();
            this.HandleRules();
            
            if (this.SaveData == null)
            {
                this.SaveData = GetComponent<SaveData>();
            }

            if (this.SaveData != null)
            {
                List<long> locations = this.SaveData.GetLocations();
                if (locations.Count > 0)
                {
                    foreach (long id in locations)
                    {
                        this.PendingLocations.Enqueue(id);
                    }
                    this.SaveData.DeleteAllLocations();
                }
            }

            this.IsReady = true;
            this.ProcessPendingItems();
            this.ProcessPendingLocations();

            Helper.UpdateChallengeFile(this.SaveData);
        }
    
        private void HandleScenario ()
        {
            this.SlotData.TryGetValue("scenario", out object scenarioData);
            AP_Scenario Scenario = AP_Scenario.Init(scenarioData);

            if (!this.ParkitectController.PlayerIsInPark(Scenario.name))
            {
                this.ParkitectController.SendMessage($"Park not recognized. Please load '{ Scenario.name }'");
                _ = this.ArchipelagoConnector.DisconnectAsync();
                this.OnDisconnect();
                return;
            }

            Constants.ScenarioName = Scenario.name;

            // Player had saved the game?
            if (!this.ParkitectController.PlayerHasSavegame())
            {
                this.HandleGoals();
                this.ParkitectController.SendMessage("Park is freshly started! All Shops and Attractions are removed for the Challenge", "Please save the game immediately to avoid data loss :)");
            }
        }
    
        private void HandleGoals ()
        {
            if (!this.SlotData.TryGetValue("goals", out object goalData))
            {
                return;
            }

            AP_Goals Goals = AP_Goals.Init(goalData);

            if (Goals.park_tickets.enabled)
            {
                ParkTicketsGoal parkTicketsGoal = new ParkTicketsGoal();
                parkTicketsGoal.value = Goals.park_tickets.value;

                if (!this.ParkitectController.PlayerHasScenarioGoal(parkTicketsGoal))
                {
                    this.ParkitectController.PlayerAddScenarioGoal(parkTicketsGoal);
                }
            }

            if (Goals.guests.enabled)
            {
                GuestsInParkGoal guestsInParkGoal = new GuestsInParkGoal();
                guestsInParkGoal.value = Goals.guests.value;

                if (!this.ParkitectController.PlayerHasScenarioGoal(guestsInParkGoal))
                {
                    this.ParkitectController.PlayerAddScenarioGoal(guestsInParkGoal);
                }
            }

            if (Goals.money.enabled)
            {
                MoneyGoal moneyGoal = new MoneyGoal();
                moneyGoal.value = Goals.money.value;

                if (!this.ParkitectController.PlayerHasScenarioGoal(moneyGoal))
                {
                    this.ParkitectController.PlayerAddScenarioGoal(moneyGoal);
                }
            }

            if (Goals.coaster_rides.enabled)
            {
                // we can only do 1 rating for a CoastersGoal, so we do 2 :D
                if (Goals.coaster_rides.values.intensity > 0)
                {
                    CoastersGoal coastersGoal = new CoastersGoal();
                    coastersGoal.coasterCount = Goals.coaster_rides.value;
                    coastersGoal.optionIndex = 1;

                    if (!this.ParkitectController.PlayerHasScenarioGoal(coastersGoal))
                    {
                        this.ParkitectController.PlayerAddScenarioGoal(coastersGoal);
                    }
                }
                if (Goals.coaster_rides.values.excitement > 0)
                {
                    CoastersGoal coastersGoal = new CoastersGoal();
                    coastersGoal.coasterCount = Goals.coaster_rides.value;
                    coastersGoal.optionIndex = 0;

                    if (!this.ParkitectController.PlayerHasScenarioGoal(coastersGoal))
                    {
                        this.ParkitectController.PlayerAddScenarioGoal(coastersGoal);
                    }
                }
            }

            if (Goals.ride_profit.enabled)
            {
                RideProfitGoal rideProfitGoal = new RideProfitGoal();
                rideProfitGoal.value = Goals.ride_profit.value;

                if (!this.ParkitectController.PlayerHasScenarioGoal(rideProfitGoal))
                {
                    this.ParkitectController.PlayerAddScenarioGoal(rideProfitGoal);
                }
            }

            if (Goals.shop_profit.enabled)
            {
                ShopProfitGoal shopProfitGoal = new ShopProfitGoal();
                shopProfitGoal.value = Goals.shop_profit.value;

                if (!this.ParkitectController.PlayerHasScenarioGoal(shopProfitGoal))
                {
                    this.ParkitectController.PlayerAddScenarioGoal(shopProfitGoal);
                }
            }

            if (Goals.shops.enabled)
            {
                ShopsCountGoal shopsCountGoal = new ShopsCountGoal();
                shopsCountGoal.value = Goals.shops.value;

                if (!this.ParkitectController.PlayerHasScenarioGoal(shopsCountGoal))
                {
                    this.ParkitectController.PlayerAddScenarioGoal(shopsCountGoal);
                }
            }
        }
   
        private void HandleRules ()
        {
            if (!this.SlotData.TryGetValue("rules", out object goalData))
            {
                return;
            }

            this.ParkitectController.AP_Rules = AP_Rules.Init(goalData);
        }
    
        private void HandleChallenges ()
        {
            if (!this.SlotData.TryGetValue("challenges", out object goalData))
            {
                return;
            }

            AP_Challenges AP_Challenges = AP_Challenges.Init(goalData);

            foreach (AP_Challenge ap_challenge in AP_Challenges.challenges)
            {
                Challenge challenge = new Challenge(this.ParkitectController, ap_challenge.LocationId);

                // Challenge is to have just shops
                if (Constants.Stall.Types.Contains(ap_challenge.item.type) && ap_challenge.item.name == "")
                {
                    challenge.SetShopType(ap_challenge.item.type, ap_challenge.item.amount);
                }

                // Challenge is to have a specific shop
                else if (ap_challenge.item.type == "shop")
                {
                    challenge.SetShop(ap_challenge.item.name, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                }

                // Challenge is to have a just an attraction
                else if (Constants.Attraction.Types.Contains(ap_challenge.item.type) && ap_challenge.item.name == "")
                {
                    challenge.SetAttractionType(ap_challenge.item.type, ap_challenge.item.amount);
                }

                // Challenge is to have a specific ride
                else if (ap_challenge.item.type == "ride")
                {
                    challenge.SetAttraction(ap_challenge.item.name, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                }

                // Challenge is to have a specific coaster
                else if (ap_challenge.item.type == "coaster")
                {
                    challenge.SetAttraction(ap_challenge.item.name, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                    challenge.AddExcitement(Helper.SafeFloat(ap_challenge.item.excitement));
                    challenge.AddIntensity(Helper.SafeFloat(ap_challenge.item.intensity));
                    challenge.AddNausea(Helper.SafeFloat(ap_challenge.item.nausea));
                    challenge.AddSatisfaction(Helper.SafeFloat(ap_challenge.item.satisfaction));
                }

                this.Challenges.Add(challenge);
            }

            this.ArchipelagoWindow.HandOver(this.Challenges);
        }

        public bool CompleteLocation(int location_id, SaveData SaveData = null)
        {
            long id = (long)(Constants.ArchipelagoBaseId + location_id);

            return this.CompleteLocation(id, SaveData);
        }

        public bool CompleteLocation(long id, SaveData SaveData = null)
        {
            if (SaveData != null && this.SaveData == null)
            {
                this.SaveData = SaveData;
            }

            if (this.IsOffline())
            {
                if (this.SaveData != null)
                {
                    this.SaveData.AddLocation(id);
                }
                this.PendingLocations.Enqueue(id);
                Helper.Debug($"[ArchipelagoController::CompleteLocation] -> no Session found - offline");
                return true;
            }

            _ = this.ArchipelagoConnector.Session.Locations.CompleteLocationChecksAsync(id);

            Helper.Debug($"[ArchipelagoController::CompleteLocation] -> CompleteLocation {id}");
            return true;
        }

        public void GoalAchieved ()
        {
            Helper.Debug($"[ArchipelagoController::GoalAchieved]");
            this.ParkitectController.SendMessage($"Congratulations! You've won this Scenario. I hope you enjoyed the Game :)");
            this.ArchipelagoWindow.Finish();
            this.ArchipelagoConnector.Session.SetGoalAchieved();
        }
    }
}