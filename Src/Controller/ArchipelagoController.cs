using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using Archipelago.Src.EnergyLink;
using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Config;
using ArchipelagoMod.Src.Connector;
using ArchipelagoMod.Src.Dispatcher;
using ArchipelagoMod.Src.SlotData;
using ArchipelagoMod.Src.UI;
using ArchipelagoMod.Src.Window;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace ArchipelagoMod.Src.Controller
{
    class ArchipelagoController : MainThreadDispatcher
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
        public bool IsProcessingPendingItems = false;

        private bool DoReconnect = false;
        private readonly List<string> PostGoalReceivedItemNames = new List<string>();

        private float receivedLastItem = Time.time;

        void Start()
        {
            Helper.Debug("[ArchipelagoController::Start]");
            this.ListenAPConfigChangeFromUI();
            this.ParkitectController = GetComponent<ParkitectController>();
            this.InitAPConfig();

            if (this.ParkitectAPConfig == null)
            {
                Helper.Debug("[ArchipelagoController::Start] -> Missing AP Config");
                return;
            }

            Constants.Playername = this.ParkitectAPConfig.Playername;
            this.ArchipelagoWindow = GetComponent<ArchipelagoWindow>();

            if (!this.ParkitectController.PlayerIsInCorrectMode())
            {
                return;
            }

            Helper.Debug("[ArchipelagoController::Start] -> Booted");
            this.DoReconnect = true;
        }

        void Update()
        {
            if (
                this.DoReconnect
                && this.ArchipelagoConnector == null
                && GameController.Instance.loadingHasBeenCompleted
                && this.ParkitectAPConfig != null
                && this.ParkitectAPConfig.IsValid()
                && !Constants.DontConnect
                )
            {
                this.DoReconnect = false;
                this.ConnectWithArchipelago();
            }

            this.SendItemSummaryIfReady();
        }

        void OnDestroy()
        {
            this.Destroy();
            this.DoReconnect = true;
        }

        void Destroy(bool connectorOnly = false)
        {
            ScriptableSingleton<ArchipelagoSettings>.Instance.UpdatedParkitectAPConfig -= this.UpdatedParkitectAPConfig;

            if (!connectorOnly)
            {
                EventManager.Instance.OnScenarioWon -= this.GoalAchieved;
            }

            if (this.ArchipelagoConnector == null)
            {
                return;
            }

            this.ArchipelagoConnector.OnConnected -= this.OnConnected;
            this.ArchipelagoConnector.OnConnectionFailed -= this.OnConnectionFailed;
            this.ArchipelagoConnector.OnReconnect -= this.OnReconnect;
            this.ArchipelagoConnector.OnReceivedPacket -= this.OnReceivedPacket;
            this.ArchipelagoConnector.OnDisconnected -= this.OnDisconnected;
            this.ArchipelagoConnector.OnItemReceived -= this.OnReceivedItem;
            this.ArchipelagoConnector.OnStopped -= this.OnStopped;
            this.ArchipelagoConnector.OnTrapReceived -= this.OnTrapReceived;
            _ = this.ArchipelagoConnector.DisconnectAsync();

            this.ArchipelagoConnector = null;
        }

        private void ConnectWithArchipelago()
        {
            this.ArchipelagoConnector = ArchipelagoConnector.Instance;
            this.ArchipelagoConnector.Init(this.ParkitectAPConfig, this.Game);
            this.Listen();
            this.ArchipelagoConnector.ConnectAsync();
        }

        private void InitAPConfig()
        {
            Helper.Debug("[ArchipelagoController::InitAPConfig]");
            if (!ParkitectAPConfig.HasConfigFile())
            {
                ParkitectAPConfig.CreateConfig();
            }

            this.ParkitectAPConfig = ParkitectAPConfig.Load();

            if (!this.ParkitectAPConfig.IsValid())
            {
                this.ParkitectController.SendMessage(this.ParkitectAPConfig.GetInvalidMessage());
            }
        }

        protected void ListenAPConfigChangeFromUI()
        {
            ScriptableSingleton<ArchipelagoSettings>.Instance.UpdatedParkitectAPConfig += this.UpdatedParkitectAPConfig;
        }

        protected void Listen()
        {
            // Won the Scenario :)
            if (!this.IsReady)
            {
                EventManager.Instance.OnScenarioWon += this.GoalAchieved;
            }

            Helper.Debug("[ArchipelagoController::Listen]");
            this.ArchipelagoConnector.OnConnected += this.OnConnected;
            this.ArchipelagoConnector.OnConnectionFailed += this.OnConnectionFailed;
            this.ArchipelagoConnector.OnReconnect += this.OnReconnect;
            this.ArchipelagoConnector.OnReceivedPacket += this.OnReceivedPacket;
            this.ArchipelagoConnector.OnDisconnected += this.OnDisconnected;
            this.ArchipelagoConnector.OnItemReceived += this.OnReceivedItem;
            this.ArchipelagoConnector.OnStopped += this.OnStopped;
            this.ArchipelagoConnector.OnTrapReceived += this.OnTrapReceived;
        }

        private void OnConnected(LoginSuccessful success)
        {
            Helper.Debug("[ArchipelagoController::Listen] -> OnConnected");
            this.OnConnected();
            this.SlotData = success.SlotData;

            MainThreadDispatcher.Enqueue(() =>
            {
                string key = this.ArchipelagoConnector.GetEnergyLinkKey();
                this.ArchipelagoConnector.Session.DataStorage[Scope.Global, key].Initialize(0);
                this.RefreshBankAccount();
            });

            this.Handle();
        }

        private void OnConnectionFailed()
        {
            if (this.ArchipelagoConnector.Session.Socket.Connected)
            {
                Helper.Debug("[ArchipelagoController::Listen] -> OnConnectionFailed");
                this.OnDisconnect();
            }
        }

        private void OnReconnect()
        {
            this.ParkitectController.SendMessage("Lost Connection to Archipelago - retrying...");
            Helper.Debug($"[ArchipelagoController::Listen] OnReconnect - Retry in : {this.ArchipelagoConnector.Retry / 1000} seconds");
        }

        private void OnReceivedPacket(string message)
        {
            this.ParkitectController.SendMessage("Archipelago Server", message);
            Helper.Debug($"[ArchipelagoController::Listen] PacketReceived - {message}");
        }

        private void OnDisconnected()
        {
            this.OnDisconnect();
            this.ParkitectController.SendMessage("Lost Connection to Archipelago");
            this.Destroy(true);
            this.DoReconnect = true;
        }

        private void OnStopped()
        {
            this.ParkitectController.SendMessage($"Maximum retries reached ({this.ArchipelagoConnector.maxRetries}).", "Make sure your Credentials are correct and the Server is running.");
        }

        private void OnTrapReceived(string trapName, string fromPlayer)
        {
            Helper.Debug($"[ArchipelagoController::OnTrapReceived] -> {trapName}");
            AP_Item AP_Item = AP_Item.Init(trapName, fromPlayer, -1);
            this.ParkitectController.PlayerRedeemTrap(AP_Item);
        }

        private void OnReceivedItem(string itemName, string player, long locationId)
        {
            string serializedName = this.ParkitectController.GetSerializedFromPrefabs(itemName);
            AP_Item AP_Item = AP_Item.Init(itemName, player, locationId, serializedName);

            if (this.SaveData != null && this.SaveData.HasUnlockedAPLocation(AP_Item.LocationId))
            {
                Helper.Debug($"[SaveData::HasUnlockedAPItem] true");
                return;
            }

            if (this.receivedLastItem + Constants.NextCheckTimeDelay >= Time.time)
            {
                this.ParkitectController.UpdateSuppressMessages();
                this.PostGoalReceivedItemNames.Add(AP_Item.Name);
            }

            this.receivedLastItem = Time.time;

            if (!this.IsReady || this.IsProcessingPendingItems || this.PendingAPItems.Count > 0)
            {
                Helper.Debug($"[ArchipelagoController::OnReceivedItem] Queue Item");
                this.PendingAPItems.Enqueue(AP_Item);
                return;
            }

            this.HandleItem(AP_Item);
        }

        private void SendItemSummaryIfReady()
        {
            if (this.PostGoalReceivedItemNames.Count == 0 || this.receivedLastItem + (Constants.NextCheckTimeDelay * 4f) > Time.time)
            {
                return;
            }

            this.ParkitectController.SendMessage(
                string.Join("\n", this.GetPostGoalReceivedItemSummary()),
                silent: true
            );

            string extraMessage = this.SaveData.HasFinished() ? "after achieving your Goal" : "";

            this.ParkitectController.SendMessage(
                $"Received {this.PostGoalReceivedItemNames.Count} items {extraMessage}",
                "Check List below"
            );

            this.PostGoalReceivedItemNames.Clear();
        }

        private List<string> GetPostGoalReceivedItemSummary()
        {
            return this.PostGoalReceivedItemNames
                .GroupBy(itemName => itemName)
                .Select(group => group.Count() > 1 ? $"{group.Count()}x {group.Key}" : group.Key)
                .ToList();
        }

        private void UpdatedParkitectAPConfig()
        {
            Helper.Debug($"[ArchipelagoController::UpdatedParkitectAPConfig]");
            if (
                this.ArchipelagoConnector != null
                && this.ArchipelagoConnector.IsConnected
                )
            {
                this.ParkitectController.SendMessage("Archipelago settings were updated, but you're already connected. The current connection will remain active.");
                return;
            }

            this.ParkitectController.SendMessage("Archipelago settings were updated.");
            this.InitAPConfig();
            this.OnDestroy();
        }

        public void ProcessPendingItems()
        {
            if (this.IsProcessingPendingItems)
            {
                return; // prevent re-entry
            }

            this.IsProcessingPendingItems = true;

            Helper.Debug($"[ArchipelagoController::ProcessPendingItems]");

            if (this.IsOffline() || !this.IsReady)
            {
                this.IsProcessingPendingItems = false;
                return;
            }

            while (this.PendingAPItems.Count > 0)
            {
                AP_Item item = this.PendingAPItems.Dequeue();
                this.HandleItem(item);
            }

            this.IsProcessingPendingItems = false;
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
            this.SaveData.DeleteAllPendingLocations();
        }

        private void HandleItem(AP_Item AP_Item)
        {
            if (this.SaveData.HasUnlockedAPLocation(AP_Item.LocationId))
            {
                return;
            }

            Helper.Debug($"[ArchipelagoController::HandleItem] Handling Item - {AP_Item.Name}");
            this.SaveData.SetUnlockedAPLocation(AP_Item.LocationId);

            if (AP_Item.IsSkip)
            {
                this.ParkitectController.SendMessage(AP_Item.Message(), canBeSuppressed: true);
                this.SaveData.IncreaseSkip();
                this.ArchipelagoWindow.UpdateSkipText();
                return;
            }

            if (AP_Item.IsSpeedup)
            {
                this.ParkitectController.SendMessage(AP_Item.Message(), canBeSuppressed: true);
                this.SaveData.IncreaseMaxSpeedup();
                this.ArchipelagoWindow.UpdateSpeedups();
                return;
            }

            if (AP_Item.IsTrap)
            {
                if (this.SaveData.HasFinished())
                {
                    Helper.Debug($"[ArchipelagoController::HandleItem] Ignoring Trap after finish - {AP_Item.Name}");
                    return;
                }

                this.ParkitectController.PlayerRedeemTrap(AP_Item);

                if (this.ArchipelagoConnector.JoinedTrapLink && !this.ArchipelagoConnector.ForwardTrapLink(AP_Item.Name))
                {
                    this.ParkitectController.SendMessage("TrapLink enabled, but wasn't joined", canBeSuppressed: true);
                }
                
                return;
            }

            if (!this.ParkitectController.PlayerHasUnlockedItem(AP_Item))
            {
                this.ParkitectController.PlayerUnlockItem(AP_Item);
                this.ParkitectController.SendMessage(AP_Item.Message(), canBeSuppressed: true);
                return;
            }
        }

        public void OnDisconnect()
        {
            this.ArchipelagoWindow.SetStatus(_Status.States.DISCONNECTED);
        }

        public void OnConnected()
        {
            this.ArchipelagoWindow.SetStatus(_Status.States.CONNECTED);
        }

        public bool IsOffline()
        {
            return this.ArchipelagoConnector.Session == null || this.ArchipelagoConnector.Session.Locations == null || this.ArchipelagoConnector.Session.Locations.AllLocations == null;
        }

        private void Handle()
        {
            Helper.Debug($"[ArchipelagoController::Handle] ready={this.IsReady}");
            this.SlotData.TryGetValue("version", out object ap_world_version);
            Version Version = new Version(ap_world_version.ToString());

            if (!Version.IsCompatible())
            {
                this.ParkitectController.SendMessage(Version.Messages());
                this.OnDisconnect();
                this.Destroy();
                return;
            }

            if (!this.HandleScenario())
            {
                return;
            }

            if (this.SaveData == null)
            {
                this.SaveData = GetComponent<SaveData>();
            }

            Helper.LogSlotData(
                JsonConvert.SerializeObject(this.SlotData, Formatting.Indented),
                System.IO.Path.Combine(SaveData.GetSaveGamePath(this.GetSlotDataSeed()), "slot_data.json")
            );

            if (this.SaveData.HasFinished())
            {
                Helper.Debug($"[ArchipelagoController::Handle] HasFinished");
                this.IsReady = true;
                this.ProcessPendingLocations();
                this.GoalAchieved();
                this.SaveData.LoadItems();
                return;
            }

            this.HandleRules();
            this.HandleChallenges();

            if (!this.IsReady)
            {
                this.SaveData.LoadItems();
            }

            if (this.SaveData.GetEnabledTrapLink())
            {
                this.ArchipelagoConnector.JoinTrapLink();
            }

            List<long> locations = this.SaveData.GetPendingLocations();
            if (locations.Count > 0)
            {
                foreach (long id in locations)
                {
                    this.PendingLocations.Enqueue(id);
                }
            }

            this.ArchipelagoWindow.SetFee(this.ParkitectController.GetFee());

            foreach (EnergyLinkItem item in this.ArchipelagoWindow.EnergyLinkHistory.History)
            {
                this.ArchipelagoWindow.AddEnergyLinkMessage(item, false);
            }

            this.IsReady = true;
            this.ProcessPendingLocations();
            this.ProcessPendingItems();
        }

        private bool HandleScenario()
        {
            this.SlotData.TryGetValue("scenario", out object scenarioData);
            AP_Scenario Scenario = AP_Scenario.Init(scenarioData);

            if (!this.ParkitectController.PlayerIsInPark(Scenario.name))
            {
                Helper.Debug($"[ArchipelagoController::HandleScenario] Entered wrong Park");
                this.ParkitectController.SendMessage($"Park not recognized. Please load '{Scenario.name}'");
                this.OnDisconnect();
                this.Destroy();
                return false;
            }

            Constants.ScenarioName = Scenario.name;
            this.SaveData = GetComponent<SaveData>();

            if (!this.IsReady)
            {
                this.SaveData.Init(this.GetSlotDataSeed());
            }

            // Player had no savegame?
            if (!this.ParkitectController.PlayerHasSavegame())
            {
                this.HandleGoals();
                this.ParkitectController.PlayerAddMyGuests();
                this.ParkitectController.SendMessage("Park is freshly started! All necessary Items are removed for the Challenge", "Please save the game immediately to potentially avoid data loss :)");
            }

            return true;
        }

        private void HandleGoals()
        {
            if (this.IsReady)
            {
                return;
            }

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
                    coastersGoal.minimumRatingValue = Goals.coaster_rides.values.intensity / 100f;

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
                    coastersGoal.minimumRatingValue = Goals.coaster_rides.values.excitement / 100f;

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
   
        private void HandleRules()
        {
            if (this.IsReady)
            {
                return;
            }

            if (!this.SlotData.TryGetValue("rules", out object rulesData))
            {
                return;
            }

            this.ParkitectController.AP_Rules = AP_Rules.Init(rulesData);

            if (this.ParkitectController.AP_Rules.progressive_speedups == 1)
            {
                this.SaveData.InitMaxSpeedup();
            }

            if (this.ParkitectController.AP_Rules.utility_buildings)
            {
                this.SaveData.SetEnabledUtilityBuildings(true);
            }

            if (this.ParkitectController.AP_Rules.decorations)
            {
                this.SaveData.SetEnabledDecorations(true);
            }

            if (this.ParkitectController.AP_Rules.statistics)
            {
                this.SaveData.SetEnabledStatistics(true);
            }

            // We only set TrapLink thing once
            if (!this.SaveData.GetServerHasSetTrapLink())
            {
                this.SaveData.SetServerHasSetTrapLink();

                if (this.ParkitectController.AP_Rules.trap_link)
                {
                    this.SaveData.SetEnabledTrapLink(true);
                }
            }

            // We only set ReleaseModeDisabled thing once
            if (!this.SaveData.GetServerHasSetReleaseMode())
            {
                this.SaveData.SetServerHasSetReleaseMode();

                if (this.ParkitectController.AP_Rules.release_mode_disabled)
                {
                    this.SaveData.SetReleaseMode(true);
                }
            }
        }

        private string GetSlotDataSeed()
        {
            if (!this.SlotData.TryGetValue("seed", out object seedData))
            {
                return null;
            }

            return seedData.ToString();
        }

        private void HandleChallenges()
        {
            if (this.IsReady)
            {
                return;
            }

            if (!this.SlotData.TryGetValue("challenges", out object challengesData))
            {
                return;
            }

            AP_Challenges AP_Challenges = AP_Challenges.Init(challengesData);

            foreach (AP_Challenge ap_challenge in AP_Challenges.challenges)
            {
                Challenge challenge = new Challenge(this.ParkitectController, ap_challenge.LocationId);
                string type = ap_challenge.item.type;
                bool isCoaster = type == "Coaster Rides" || type == "coaster";
                bool hasName = !string.IsNullOrEmpty(ap_challenge.item.name);

                // Challenge is to have just shops
                if (Constants.Stall.Types.Contains(type) && !hasName)
                {
                    challenge.SetShopType(type, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddRevenueRating(ap_challenge.item.revenue);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                    challenge.AddVoucher(ap_challenge.item.vouchers);
                }

                // Challenge is to have a specific shop
                else if (Constants.Stall.Types.Contains(type))
                {
                    challenge.SetShop(ap_challenge.item.name, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddRevenueRating(ap_challenge.item.revenue);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                    challenge.AddVoucher(ap_challenge.item.vouchers);
                }

                // Challenge is to have a just an attraction
                else if (Constants.Attraction.Types.Contains(type) && !hasName)
                {
                    challenge.SetAttractionType(type, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddRevenueRating(ap_challenge.item.revenue);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                    challenge.AddDecoRating(ap_challenge.item.deco);
                    challenge.AddVoucher(ap_challenge.item.vouchers);

                    if (isCoaster)
                    {
                        challenge.AddPhoto(Helper.SafeInt(ap_challenge.item.photos));
                    }
                }

                // Challenge is to have a specific ride
                else if (Constants.Attraction.Types.Contains(type))
                {
                    challenge.SetAttraction(ap_challenge.item.name, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddRevenueRating(ap_challenge.item.revenue);
                    challenge.AddProfitRating(ap_challenge.item.profit);
                    challenge.AddDecoRating(ap_challenge.item.deco);
                    challenge.AddVoucher(ap_challenge.item.vouchers);
                }

                // Challenge is to have a specific coaster
                else if (isCoaster)
                {
                    challenge.SetAttraction(ap_challenge.item.name, ap_challenge.item.amount);
                    challenge.AddGuestsRating(ap_challenge.item.customers);
                    challenge.AddRevenueRating(ap_challenge.item.revenue);
                    challenge.AddExcitement(Helper.SafeFloat(ap_challenge.item.excitement));
                    challenge.AddIntensity(Helper.SafeFloat(ap_challenge.item.intensity));
                    challenge.AddNausea(Helper.SafeFloat(ap_challenge.item.nausea));
                    challenge.AddSatisfaction(Helper.SafeFloat(ap_challenge.item.satisfaction));
                    challenge.AddDecoRating(ap_challenge.item.deco);
                    challenge.AddPhoto(Helper.SafeInt(ap_challenge.item.photos));
                }

                // Employee
                else if (type == "Employee")
                {
                    challenge.AddEmployee(ap_challenge.item.amount, ap_challenge.item.name);
                }

                // Park Guests
                else if (type == "Guest")
                {
                    challenge.AddParkGuests(ap_challenge.item.amount);
                }

                // Park Guests
                else if (type == "Money")
                {
                    challenge.AddParkMoney(ap_challenge.item.amount);
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
                    this.SaveData.AddPendingLocation(id);
                }
                this.PendingLocations.Enqueue(id);
                Helper.Debug($"[ArchipelagoController::CompleteLocation] no Session found - offline");
                return true;
            }

            _ = this.ArchipelagoConnector.Session.Locations.CompleteLocationChecksAsync(id);

            Helper.Debug($"[ArchipelagoController::CompleteLocation] CompleteLocation {id}");
            return true;
        }

        public void Speak(string message)
        {
            string msg = message.Replace(" ", "").ToLower();

            if (!Constants.Commands.All.Contains(msg))
            {
                this.ArchipelagoConnector.Session.Say(message);
            }

            // We have a special command
            if (Constants.Commands.TrapLink.All.Contains(msg))
            {
                if (msg == Constants.Commands.TrapLink.Toggle)
                {
                    bool value = this.ArchipelagoConnector.ToggleTrapLink();
                    this.SaveData.SetEnabledTrapLink(value);
                }

                if (msg == Constants.Commands.TrapLink.Join)
                {
                    this.ArchipelagoConnector.JoinTrapLink();
                    this.SaveData.SetEnabledTrapLink(true);
                }

                if (msg == Constants.Commands.TrapLink.Leave)
                {
                    this.ArchipelagoConnector.LeaveTrapLink();
                    this.SaveData.SetEnabledTrapLink(false);
                }

                this.ParkitectController.SendMessage(this.GetChangedTrapLinkMessage());
            }

            if (Constants.Commands.ReleaseMode.All.Contains(msg))
            {
                if (msg == Constants.Commands.ReleaseMode.Toggle)
                {
                    bool value = !this.SaveData.GetReleaseMode();
                    this.SaveData.SetReleaseMode(value);

                    if (this.SaveData.GetReleaseMode())
                    {
                        this.ArchipelagoWindow.Finish();
                    }
                }

                if (msg == Constants.Commands.ReleaseMode.Disable)
                {
                    this.SaveData.SetReleaseMode(false);
                }

                if (msg == Constants.Commands.ReleaseMode.Enable)
                {
                    this.SaveData.SetReleaseMode(true);
                    this.ArchipelagoWindow.Finish();
                }

                this.ParkitectController.SendMessage(this.GetChangedReleaseModeMessage());
            }
        }

        private string GetChangedTrapLinkMessage()
        {
            return this.SaveData.GetEnabledTrapLink() ? "TrapLink enabled" : "TrapLink disabled";
        }

        private string GetChangedReleaseModeMessage()
        {
            return this.SaveData.GetReleaseMode() ? "Release Mode is enabled" : "Release Mode is disabled";
        }

        public void GoalAchieved()
        {
            Helper.Debug($"[ArchipelagoController::GoalAchieved]");
            bool releaseMode = this.SaveData.GetReleaseMode();
            string finishMsgPart = releaseMode ? "I hope you enjoyed the Game :)" : "But you are not done yet! Finish up the rest of Challenges!";
            this.ParkitectController.SendMessage($"Congratulations! You've won this Scenario. {finishMsgPart}");

            if (releaseMode)
            {
                this.ArchipelagoWindow.Finish();
            }

            this.ParkitectController.UpdateSuppressMessages();
            this.ArchipelagoConnector.GoalComplete();
        }

        public float RefreshBankAccount()
        {
            float money = this.GetMoneyFromBank();
            this.ArchipelagoWindow.UpdateBankAccount(money);
            return money;
        }

        public float GetMoneyFromBank()
        {
            Helper.Debug($"[ArchipelagoController::GetMoneyFromBank]");
            float money = 0;

            MainThreadDispatcher.Enqueue(() =>
            {
                string key = string.Format("EnergyLink{0}", this.ArchipelagoConnector.Session.ConnectionInfo.Team);
                string _money = this.ArchipelagoConnector.Session.DataStorage[Scope.Global, key].GetAsync<string>().GetAwaiter().GetResult();

                Helper.Debug($"[ArchipelagoController::GetMoneyFromBank] _money={_money}");

                if (!string.IsNullOrEmpty(_money))
                {
                    BigInteger _parsed_money = BigInteger.Parse(_money) / Constants.EnergyLink.Divider;
                    Helper.Debug($"[ArchipelagoController::GetMoneyFromBank] _parsed_money={_parsed_money}");
                    float.TryParse(_parsed_money.ToString(), out money);
                }
            });
            
            Helper.Debug($"[ArchipelagoController::GetMoneyFromBank] money={money}");
            return money;
        }

        public float WithdrawMoneyFromBank(float amount, bool hitMax = false)
        {
            float fee = this.ParkitectController.GetFee();
            float bankMoney = this.GetMoneyFromBank();

            SetPacket packet = this.ArchipelagoConnector.BuildWithdrawEnergyLink(Helper.GetMoneyForEnergyLink(amount), hitMax);
            _ = this.ArchipelagoConnector.ForwardPacket(packet, true);
         
            EnergyLinkItem item = new EnergyLinkItem(amount, Helper.GetTax(amount, fee));

            if (hitMax)
            {
                item = new EnergyLinkItem(bankMoney, Helper.GetTax(bankMoney, fee));
                this.ArchipelagoWindow.AddEnergyLinkMessage(item);
                return Helper.GetTaxedMoney(bankMoney, fee, true);
            }

            this.ArchipelagoWindow.AddEnergyLinkMessage(item);
            return Helper.GetTaxedMoney(amount, fee, true); ;
        }

        public void DepositMoneyToBank(float amount)
        {
            float fee = this.ParkitectController.GetFee();
            BigInteger money = Helper.GetTaxedMoneyForEnergyLink(amount, fee, true);
            SetPacket packet = this.ArchipelagoConnector.BuildDepositEnergyLink(money);
            _ = this.ArchipelagoConnector.ForwardPacket(packet, true);

            EnergyLinkItem item = new EnergyLinkItem(amount, Helper.GetTax(amount, fee), EnergyLinkItem.EnergyLinkType.Deposit);
            this.ArchipelagoWindow.AddEnergyLinkMessage(item);
        }
    }
}
