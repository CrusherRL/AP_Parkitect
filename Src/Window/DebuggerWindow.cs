using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using ArchipelagoMod.Src.SlotData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ArchipelagoMod.Src.Constants;

namespace ArchipelagoMod.Src.Window
{
    class DebuggerWindow : MonoBehaviour
    {
        protected int id = 1;
        protected ParkitectController Controller = null;
        protected string windowName = "Archipelago Debugger";
        public Rect WindowRect = new Rect(20, 20, 200, 200);
        public Rect TitleBarRect = new Rect(0, 0, 200000000, 20);
        public bool isOpen = false;
        private KeyCode KeyCode = KeyCode.F12;

        void Awake()
        {
            Helper.Debug($"[DebuggerWindow::Awake]");
            this.Controller = GetComponent<ParkitectController>();
            WindowRect = new Rect(20, 20, 700, 200);
            Helper.Debug($"[DebuggerWindow::Awake] Booted");
        }

        void Update()
        {
            // Debugger
            if (Input.GetKeyDown(this.KeyCode))
            {
                this.ToggleWindowState();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.CloseWindow();
            }
        }

        void OnDestroy() { }

        void OnGUI()
        {
            if (this.isOpen) {
                this.DrawWindow();
            }
        }

        public void DrawWindow()
        {
            WindowRect = GUILayout.Window(this.id, this.WindowRect, DrawMain, windowName);
        }

        public void ToggleWindowState()
        {
            this.isOpen = !this.isOpen;
        }
      
        public void OpenWindow()
        {
            this.isOpen = true;
        }
      
        public void CloseWindow()
        {
            this.isOpen = false;
        }

        // Content for this Window

        public void DrawMain(int windowId)
        {
            if (GUI.Button(new Rect(WindowRect.width - 21, 6, 15, 15), "x"))
            {
                CloseWindow();
            }

            GUI.BeginGroup(new Rect(0, /*27*/0, WindowRect.width, WindowRect.height/* - 33*/));
            DrawContent();

            GUI.EndGroup();
            GUI.DragWindow(TitleBarRect);
        }
     
        public void DrawContent ()
        {
            this.DrawPlayerSpeedUpsOptions();
            this.DrawPlayerMoneyOptions();
            this.DrawGuestsOptions();
            this.DrawEmployeeOptions();
            this.DrawWeatherOptions();
            this.DrawAttractionOptions();
            this.DrawStallOptions();
            this.DrawTraps();
            this.DrawChallengeOptions();
        }

        // -----------------------------
        // Player options
        // -----------------------------
        public void DrawPlayerSpeedUpsOptions ()
        {
            this.SetLabel("Set Speed:");
            GUILayout.BeginHorizontal();

            foreach (int speed in Constants.Player.SpeedupOptions)
            {
                if (GUILayout.Button(speed + "x"))
                {
                    Controller.PlayerRaiseSpeed(speed);
                }
            }

            GUILayout.EndHorizontal();
        }

        public void DrawPlayerMoneyOptions ()
        {
            this.SetLabel("Add Money:");
            GUILayout.BeginHorizontal();

            foreach (int money in Constants.Player.MoneyOptions)
            {
                if (GUILayout.Button("$" + money))
                {
                    Controller.PlayerAddMoney(money);
                }
            }

            GUILayout.EndHorizontal();
        }

        // -----------------------------
        // Guests options
        // -----------------------------

        public void DrawGuestsOptions ()
        {
            this.SetLabel("Guests:");
            GUILayout.BeginHorizontal();

            foreach (int guests in Constants.Guest.SpawnOptions)
            {
                if (GUILayout.Button("Spawn " + guests))
                {
                    Controller.PlayerAddGuests(guests);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            foreach (int kills in Constants.Guest.KillOptions)
            {
                if (GUILayout.Button("Kill " + kills))
                {
                    Controller.PlayerKillGuests(kills);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            foreach (int money in Constants.Guest.MoneyOptions)
            {
                if (GUILayout.Button("+ $" + money))
                {
                    Controller.PlayerChangeGuestsMoney(money, 25f);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            foreach (int money in Constants.Guest.MoneyOptions)
            {
                if (GUILayout.Button("- $" + money))
                {
                    Controller.PlayerChangeGuestsMoney(money, 25f, "-");
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Hungry"))
            {
                Controller.PlayerSetGuestsHungry();
            }

            if (GUILayout.Button("Thirsty"))
            {
                Controller.PlayerSetGuestsThirsty();
            }

            if (GUILayout.Button("Bathroom"))
            {
                Controller.PlayerSetGuestsToBathroom();
            }

            if (GUILayout.Button("Vomiting"))
            {
                Controller.PlayerSetGuestsToVomit();
            }

            if (GUILayout.Button("Happiness"))
            {
                Controller.PlayerSetGuestsHappy();
            }

            if (GUILayout.Button("Tiredness"))
            {
                Controller.PlayerSetGuestsTired();
            }

            if (GUILayout.Button("Vandals"))
            {
                Controller.PlayerSetGuestsAsVandals(Constants.Guest.VandalsOptions[0]);
            }

            GUILayout.EndHorizontal();
        }

        // -----------------------------
        // Employee options
        // -----------------------------

        public void DrawEmployeeOptions ()
        {
            this.SetLabel("Staff:");
            GUILayout.BeginHorizontal();

            foreach (Prefabs employee in Constants.Employee.Options)
            {
                if (GUILayout.Button("Hire " + employee.ToString()))
                {
                    Controller.PlayerHireEmployees(employee, Constants.Employee.SpawnRanges[0]);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Tired"))
            {
                Controller.PlayerSetEmployeesTired(Controller.GetParkEmployees(Constants.Employee.TirednessRanges[0]));
            }

            if (GUILayout.Button("Training"))
            {
                Controller.PlayerSetEmployeesTraining(Controller.GetParkEmployees(Constants.Employee.TrainingRanges[0]));
            }

            GUILayout.EndHorizontal();
        }

        // -----------------------------
        // Minsc
        // -----------------------------

        public void DrawWeatherOptions ()
        {
            this.SetLabel("Set Weather:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Rainy"))
            {
                Controller.PlayerChangeWeather(Constants.Weather.Options.RAINY);
            }

            if (GUILayout.Button("Stormy"))
            {
                Controller.PlayerChangeWeather(Constants.Weather.Options.STORMY);
            }

            if (GUILayout.Button("Cloudy"))
            {
                Controller.PlayerChangeWeather(Constants.Weather.Options.CLOUDY);
            }

            if (GUILayout.Button("Sunny"))
            {
                Controller.PlayerChangeWeather(Constants.Weather.Options.SUNNY);
            }

            GUILayout.EndHorizontal();
        }

        public void DrawAttractionOptions ()
        {
            this.SetLabel("Attractions:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Break random"))
            {
                List<Attraction> attractions = Randomizer.GetRandomAttractionFromPark(20f);

                Controller.PlayerBreakAttractions(attractions);
            }

            if (GUILayout.Button("Remove all Rides"))
            {
                Controller.PlayerRemoveAllRides();
            }

            if (GUILayout.Button("Add all Rides"))
            {
                Controller.PlayerAddAllRides();
            }

            if (GUILayout.Button("Add Free-Ride Voucher"))
            {
                Attraction attraction = Randomizer.GetRandomAttractionFromPark(0f, 1).First();
                List<Guest> guests = Randomizer.GetRandomGuests(1f);

                foreach (Guest guest in guests)
                {
                    guest.addToInventory(Controller.PlayerCreateAttractionVoucher(attraction));
                }
            }

            GUILayout.EndHorizontal();
        }

        public void DrawStallOptions ()
        {
            this.SetLabel("Shops:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Re-deliver Shops"))
            {
                List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(50f);

                Controller.PlayerSetReDeliveryForProductShops(productShops);
            }

            if (GUILayout.Button("Cleanup Shops"))
            {
                List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(50f);

                Controller.PlayerSetCleanShopJob(productShops);
            }

            if (GUILayout.Button("Remove all Shops"))
            {
                Controller.PlayerRemoveAllStalls();
            }

            if (GUILayout.Button("Add all Shops"))
            {
                Controller.PlayerAddAllStalls();
            }

            if (GUILayout.Button("Add Product Voucher"))
            {
                ProductShop shop = Randomizer.GetRandomProductShopsFromPark(0f, 1).First();
                List<Guest> guests = Randomizer.GetRandomGuests(1f);

                foreach (Guest guest in guests)
                {
                    guest.addToInventory(Controller.PlayerCreateShopVoucher(shop));
                }
            }

            GUILayout.EndHorizontal();
        }

        public void DrawTraps()
        {
            this.SetLabel("Attraction and Shop Traps:");
            GUILayout.BeginHorizontal();

            foreach (string trap in Constants.Trap.Attraction)
            {
                if (GUILayout.Button(trap))
                {
                    this.FakeRedeemTrap(trap);
                }
            }

            foreach (string trap in Constants.Trap.Shop)
            {
                if (GUILayout.Button(trap))
                {
                    this.FakeRedeemTrap(trap);
                }
            }

            GUILayout.EndHorizontal();
            this.SetLabel("Player and Employee Traps:");
            GUILayout.BeginHorizontal();

            foreach (string trap in Constants.Trap.Player)
            {
                if (GUILayout.Button(trap))
                {
                    this.FakeRedeemTrap(trap);
                }
            }

            foreach (string trap in Constants.Trap.Employee)
            {
                if (GUILayout.Button(trap))
                {
                    this.FakeRedeemTrap(trap);
                }
            }

            GUILayout.EndHorizontal();
            this.SetLabel("Guest Traps:");
            GUILayout.BeginHorizontal();
            int i = 0;

            foreach (string trap in Constants.Trap.Guest)
            {
                if (i % 5 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(trap))
                {
                    this.FakeRedeemTrap(trap);
                }

                i += 1;
            }

            GUILayout.EndHorizontal();
        }

        public void DrawChallengeOptions()
        {
            this.SetLabel("Challenges:");
            GUILayout.BeginHorizontal();

            string[] challenges = new string[3] { "Challenge 1", "Challenge 2", "Challenge 3" };
            foreach (string challenge in challenges)
            {
                ArchipelagoWindow archipelagoWindow = GetComponent<ArchipelagoWindow>();
                if (GUILayout.Button($"Skip {challenge}"))
                {
                    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Skip {challenge}");
                    archipelagoWindow.SkipChallenge(challenge, true);
                }
            }

            if (GUILayout.Button("Complete all Locations"))
            {
                Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Completion");
                GetComponent<ArchipelagoController>().GoalAchieved();
            }

            GUILayout.EndHorizontal();
        }
     
        public void DrawTestingOptions()
        {
            this.SetLabel("Testing:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Log all rides and shops"))
            {
                List<Attraction> atts = this.Controller.GetAllAttractionsFromAssetManager();
                List<Shop> shops = this.Controller.GetAllShopsFromAssetManager();

                Helper.Debug("===================================");
                Helper.Debug("===================================");
                Helper.Debug("===================================");
                foreach (Attraction att in atts)
                {
                    Helper.Debug($"{{{att.getPrefabType()}}}, {{{att.getName()}}}");
                }
                foreach (Shop shop in shops)
                {
                    Helper.Debug($"{{{shop.getPrefabType()}}}, {{{shop.getName()}}}");
                }
                Helper.Debug("===================================");
                Helper.Debug("===================================");
                Helper.Debug("===================================");
            }

            GUILayout.EndHorizontal();
        }

        private void FakeRedeemTrap(string trap)
        {
            Helper.Debug($"executing {trap}");
            AP_Item AP_Item = AP_Item.Init(trap, Constants.Playername, -1);
            this.Controller.PlayerRedeemTrap(AP_Item);
        }

        protected void SetLabel(string label, bool newLineAfter = true)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);

            if (newLineAfter)
            {
                GUILayout.EndHorizontal();
            }
        }
    }
}
