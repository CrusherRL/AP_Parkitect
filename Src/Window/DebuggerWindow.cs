using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using ArchipelagoMod.Src.SlotData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArchipelagoMod.Src.Window
{
    class DebuggerWindow : MonoBehaviour
    {
        protected int id = 1;
        protected ParkitectController Controller = null;
        protected string windowName = "Archipelago Debugger";
        public Rect WindowRect = new Rect(40, 40, 200, 200);
        public Rect TitleBarRect = new Rect(0, 0, 200000000, 20);
        public bool isOpen = false;
        private KeyCode KeyCode = KeyCode.F12;

        void Awake()
        {
            Helper.Debug($"[DebuggerWindow::Awake]");
            this.Controller = GetComponent<ParkitectController>();
            this.WindowRect = new Rect(40, 40, 700, 200);
            Helper.Debug($"[DebuggerWindow::Awake] Booted");

            if (Constants.LogStats)
            {
                this.LogParkitectArchipelagoData();
            }
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
            if (this.isOpen)
            {
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

            this.DrawContent();

            GUI.DragWindow(TitleBarRect);
        }

        public void DrawContent(bool debug = false)
        {
            this.DrawPlayerSpeedUpsOptions(debug);
            this.DrawPlayerMoneyOptions(debug);
            this.DrawGuestsOptions(debug);
            this.DrawEmployeeOptions(debug);
            this.DrawWeatherOptions(debug);
            this.DrawAttractionOptions(debug);
            this.DrawStallOptions(debug);
            this.DrawUtilityBuildingOptions(debug);
            this.DrawTraps(debug);
            this.DrawChallengeOptions(debug);
            this.DrawTestingOptions(debug);
        }

        // -----------------------------
        // Player options
        // -----------------------------
        public void DrawPlayerSpeedUpsOptions(bool debug = false)
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

        public void DrawPlayerMoneyOptions(bool debug = false)
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

        public void DrawGuestsOptions(bool debug = false)
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

            if (debug)
            {
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
            }

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

        public void DrawEmployeeOptions(bool debug = false)
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

        public void DrawWeatherOptions(bool debug = false)
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

        public void DrawAttractionOptions(bool debug = false)
        {
            this.SetLabel("Attractions:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Break random"))
            {
                List<Attraction> attractions = Randomizer.GetRandomAttractionFromPark(20f);

                Controller.PlayerBreakAttractions(attractions);
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

            if (GUILayout.Button("Remove all"))
            {
                Controller.PlayerRemoveAllRides();
            }

            if (GUILayout.Button("Add all"))
            {
                Controller.PlayerAddAllRides();
            }

            GUILayout.EndHorizontal();
        }

        public void DrawStallOptions(bool debug = false)
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

            if (GUILayout.Button("Add Product Voucher"))
            {
                ProductShop shop = Randomizer.GetRandomProductShopsFromPark(0f, 1).First();
                List<Guest> guests = Randomizer.GetRandomGuests(1f);

                foreach (Guest guest in guests)
                {
                    guest.addToInventory(Controller.PlayerCreateShopVoucher(shop));
                }
            }

            if (GUILayout.Button("Remove all"))
            {
                Controller.PlayerRemoveAllStalls();
            }

            if (GUILayout.Button("Add all"))
            {
                Controller.PlayerAddAllStalls();
            }

            GUILayout.EndHorizontal();
        }
    
        public void DrawUtilityBuildingOptions(bool debug = false)
        {
            this.SetLabel("Utility Building:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove all"))
            {
                Controller.PlayerRemoveAllUtilityBuildings();
            }

            if (GUILayout.Button("Add all"))
            {
                Controller.PlayerAddAllUtilityBuildings();
            }

            GUILayout.EndHorizontal();
        }

        public void DrawTraps(bool debug = false)
        {
            if (!debug)
            {
                return;
            }

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
            this.SetLabel("Research Traps:");
            GUILayout.BeginHorizontal();
            int j = 0;

            foreach (string trap in Constants.Trap.Research)
            {
                if (j % 5 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(trap))
                {
                    this.FakeRedeemTrap(trap);
                }

                j += 1;
            }

            GUILayout.EndHorizontal();
        }

        public void DrawChallengeOptions(bool debug = false)
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

            if (GUILayout.Button("Achieve Goal"))
            {
                Helper.Debug($"[DebuggerWindow::DrawTestingOptions] GoalAchieved");
                GetComponent<ArchipelagoController>().GoalAchieved();
            }

            GUILayout.EndHorizontal();
        }

        public void DrawTestingOptions(bool debug = false)
        {
            if (!debug)
            {
                return;
            }

            this.SetLabel("Testing:");
            GUILayout.BeginHorizontal();
            ParkitectController parkitectController = GetComponent<ParkitectController>();

            if (GUILayout.Button("Check Photos"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttraction(Constants.Attraction.CoasterRides[19], 1);
                challenge.AddPhoto(10);
                challenge.Check();
            }

            if (GUILayout.Button("Check Photos - type"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttractionType(Constants.Attraction.Types[2], 1);
                challenge.AddPhoto(10);
                challenge.Check();
            }

            if (GUILayout.Button("Check Vouchers - a"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttraction(Constants.Attraction.ThrillRides[13], 1);
                challenge.AddVoucher(10);
                challenge.Check();
            }

            if (GUILayout.Button("Check Vouchers - s"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetShop(Constants.Stall.Food[10], 1);
                challenge.AddVoucher(10);
                challenge.Check();
            }

            if (GUILayout.Button("Check Vouchers - a type"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttractionType(Constants.Attraction.Types[2], 1);
                challenge.AddVoucher(10);
                challenge.Check();
            }

            if (GUILayout.Button("Check Vouchers - s type"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetShopType(Constants.Stall.Types[1], 1);
                challenge.AddVoucher(10);
                challenge.Check();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Check Profit all - a"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttractionType(Constants.Attraction.Types[2], 3);
                challenge.AddProfitRating(10000);
                challenge.Check();
            }
            if (GUILayout.Button("Check Profit all - s"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetShopType(Constants.Stall.Types[0], 3);
                challenge.AddProfitRating(10000);
                challenge.Check();
            }

            if (GUILayout.Button("Check Revenue all - a"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttractionType(Constants.Attraction.Types[2], 3);
                challenge.AddRevenueRating(10000);
                challenge.Check();
            }
            if (GUILayout.Button("Check Revenue all - s"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetShopType(Constants.Stall.Types[0], 3);
                challenge.AddRevenueRating(10000);
                challenge.Check();
            }

            if (GUILayout.Button("Check Photo all - a"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttractionType(Constants.Attraction.Types[2], 3);
                challenge.AddPhoto(10000);
                challenge.Check();
            }

            if (GUILayout.Button("Check Vouchers all - a"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetAttractionType(Constants.Attraction.Types[2], 3);
                challenge.AddVoucher(10000);
                challenge.Check();
            }
            if (GUILayout.Button("Check Vouchers all - s"))
            {
                Challenge challenge = new Challenge(parkitectController, -1);
                challenge.SetShopType(Constants.Stall.Types[0], 3);
                challenge.AddVoucher(10000);
                challenge.Check();
            }

            //if (GUILayout.Button("Log All unlocked Items for AP"))
            //{
            //    Helper.Debug($"==========");
            //    Helper.Debug($"Park: {GameController.Instance.park.parkName}");
            //    Helper.Debug("----------");
            //    List<ResearchRule> rules = GameController.Instance.park.scenario.research.getRules().Where(r => r.isUnlocked).OrderBy(r => r.name).ToList();

            //    foreach (ResearchRule r in rules)
            //    {
            //        Helper.Debug(r.name);
            //    }
            //    Helper.Debug($"==========");
            //}

            //if (GUILayout.Button("Log All unlocked Items for AP - Starters"))
            //{
            //    Helper.Debug($"==========");
            //    Helper.Debug($"Park: {GameController.Instance.park.parkName}");
            //    Helper.Debug("----------");
            //    List<ResearchRule> rules = GameController.Instance.park.scenario.research.getRules().Where(r => r.isUnlocked && (Constants.Attraction.All.Contains(r.name) || Constants.Stall.All.Contains(r.name))).OrderBy(r => r.name).ToList();

            //    foreach (ResearchRule r in rules)
            //    {
            //        Helper.Debug(r.name);
            //    }
            //    Helper.Debug($"==========");
            //}

            if (GUILayout.Button("Log All Items for AP"))
            {
                this.LogParkitectArchipelagoData();
            }

            //if (GUILayout.Button("Log All Decorations"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Log All Decorations");
            //    List<Deco> things = ScriptableSingleton<AssetManager>.Instance.getDecoObjects().ToList();
            //    foreach (Deco thing in things)
            //    {
            //        try
            //        {
            //            Helper.Debug($"{thing.themeTag} - {thing.getResearchReferenceName()} = {thing.getName()}");
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //if (GUILayout.Button("Log All research rules"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Log All research rules");
            //    List<ResearchRule> things = GameController.Instance.park.scenario.research.getRules().ToList();
            //    foreach (ResearchRule thing in things)
            //    {
            //        try
            //        {
            //            Helper.Debug($"{thing.getReferenceName()} = {thing.name}");
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //int l = 0;
            //foreach (string trap in Constants.TrapLinks)
            //{
            //    if (l % 5 == 0)
            //    {
            //        GUILayout.EndHorizontal();
            //        GUILayout.BeginHorizontal();
            //    }

            //    if (GUILayout.Button(trap))
            //    {
            //        Helper.Debug($"[DebuggerWindow::DrawTestingOptions] {trap}");
            //        this.FakeRedeemTrap(trap);
            //    }

            //    l += 1;
            //}

            //if (GUILayout.Button("Log All Park Employees"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Log All Park Employees");
            //    List<Employee> employees = parkitectController.GetParkEmployees();
            //    foreach (Employee employee in employees)
            //    {
            //        try
            //        {
            //            Helper.Debug($"name: {employee.getNameForUI()}");
            //            Helper.Debug($"experienceLevel: {employee.experienceLevel}");
            //            Helper.Debug($"getPrefabType: {employee.getPrefabType().ToString()}");
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //if (GUILayout.Button("Log All Research Rules"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Log All Research Rules");
            //    List<ResearchRule> attractions = GameController.Instance.park.scenario.research.getRules().ToList();
            //    foreach (ResearchRule attraction in attractions)
            //    {
            //        try
            //        {
            //            Helper.Debug(attraction.name);
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //if (GUILayout.Button("Log All Attractions"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Attractions");
            //    List<Attraction> attractions = parkitectController.GetAllAttractionsFromAssetManager();
            //    foreach (Attraction attraction in attractions)
            //    {
            //        try
            //        {
            //            Helper.Debug(attraction.getName());
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //if (GUILayout.Button("Log All Shops"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] Shops");
            //    List<Shop> shops = parkitectController.GetAllShopsFromAssetManager();
            //    foreach (Shop shop in shops)
            //    {
            //        try
            //        {
            //            Helper.Debug(shop.getName());
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //if (GUILayout.Button("Log All UtilityBuildings"))
            //{
            //    Helper.Debug($"[DebuggerWindow::DrawTestingOptions] UtilityBuilding");
            //    List<UtilityBuilding> utilityBuildings = parkitectController.GetAllUtilityBuildingsFromAssetManager();
            //    foreach (UtilityBuilding utilityBuilding in utilityBuildings)
            //    {
            //        try
            //        {
            //            Helper.Debug(utilityBuilding.getPrefabType().ToString());
            //        }
            //        catch
            //        {
            //            Helper.Debug($"--- Failed ---");
            //        }
            //    }
            //}

            //GUILayout.EndHorizontal();
            //this.SetLabel("Theme Tags:");
            //GUILayout.BeginHorizontal();

            //int l = 0;
            //foreach (string themeTag in Constants.Decorations.ThemeTags)
            //{
            //    if (l % 5 == 0)
            //    {
            //        GUILayout.EndHorizontal();
            //        GUILayout.BeginHorizontal();
            //    }

            //    if (GUILayout.Button(themeTag))
            //    {
            //        parkitectController.PlayerRemoveAllDecorations();
            //        ThemeContainer themeContainer = parkitectController.FindThemeContainer(themeTag);
            //        parkitectController.PlayerAddDecorations(themeContainer);
            //    }

            //    l += 1;
            //}
            GUILayout.EndHorizontal();
        }

        private void FakeRedeemTrap(string trap)
        {
            Helper.Debug($"executing {trap}");
            //ArchipelagoController controller = GetComponent<ArchipelagoController>();
            //controller.OnTrapReceived(trap, "me!");
            Controller.PlayerRedeemTrap(this.CreateFakeAPItem(trap));
        }

        private AP_Item CreateFakeAPItem(string thing)
        {
            return AP_Item.Init(thing, Constants.Playername, -1);
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

        protected void LogParkitectArchipelagoData()
        {
            Helper.Debug($"==========");
            Helper.Debug($"Park: {GameController.Instance.park.parkName}");
            Helper.Debug("----------");

            List<ResearchRule> rules = GameController.Instance.park.scenario.research.getRules().ToList();

            List<ResearchRule> calms = rules.Where(r => Constants.Attraction.CalmRides.Contains(r.name)).OrderBy(r => r.name).ToList();
            List<ResearchRule> thrills = rules.Where(r => Constants.Attraction.ThrillRides.Contains(r.name)).OrderBy(r => r.name).ToList();
            List<ResearchRule> coasters = rules.Where(r => Constants.Attraction.CoasterRides.Contains(r.name)).OrderBy(r => r.name).ToList();
            List<ResearchRule> transports = rules.Where(r => Constants.Attraction.TransportRides.Contains(r.name)).OrderBy(r => r.name).ToList();
            List<ResearchRule> waters = rules.Where(r => Constants.Attraction.WaterRides.Contains(r.name)).OrderBy(r => r.name).ToList();
            List<ResearchRule> stats = rules.Where(r => Constants.Research.Rules.Statistics.Contains(r.name)).OrderBy(r => r.name).ToList();
            List<ResearchRule> shops = rules.Where(r => Constants.Stall.All.Contains(r.name)).OrderBy(r => r.name).ToList();

            List<ResearchRule> decorations = rules.Where(r => Constants.Research.Rules.Decorations.Contains(r.name)).ToList();
            List<string> decos = new List<string>();
            foreach (ResearchRule d in decorations)
            {
                string key = Constants.Decorations.ThemeTagMapToProps
                    .FirstOrDefault(x => x.Value.Contains(d.name))
                    .Key;
                decos.Add(key);
            }

            Helper.Debug($"{Constants.Attraction.Types[0]}:");
            foreach (ResearchRule thing in calms)
            {
                try
                {
                    Helper.Debug($"{thing.name} - {thing.isUnlocked}");
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug($"{Constants.Attraction.Types[1]}:");
            foreach (ResearchRule thing in thrills)
            {
                try
                {
                    Helper.Debug($"{thing.name} - {thing.isUnlocked}");
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug($"{Constants.Attraction.Types[2]}:");
            foreach (ResearchRule thing in coasters)
            {
                try
                {
                    Helper.Debug($"{thing.name} - {thing.isUnlocked}");
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug($"{Constants.Attraction.Types[3]}:");
            foreach (ResearchRule thing in transports)
            {
                try
                {
                    Helper.Debug($"{thing.name} - {thing.isUnlocked}");
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug($"{Constants.Attraction.Types[4]}:");
            foreach (ResearchRule thing in waters)
            {
                try
                {
                    Helper.Debug($"{thing.name} - {thing.isUnlocked}");
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug("Decoration Theme Tags:");
            foreach (string deco in decos.Distinct().ToList())
            {
                try
                {
                    Helper.Debug(deco);
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug("Statistics:");
            foreach (ResearchRule stat in stats)
            {
                try
                {
                    Helper.Debug(stat.name);
                }
                catch { }
            }
            Helper.Debug("----------");

            Helper.Debug("Shops:");
            foreach (ResearchRule thing in shops)
            {
                try
                {
                    Helper.Debug($"{thing.name} - {thing.isUnlocked}");
                }
                catch { }
            }
            Helper.Debug($"==========");
        }
    }
}
