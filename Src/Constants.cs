using System.Collections.Generic;
using System.Linq;

namespace ArchipelagoMod.Src
{
    class Constants
    {
        public static float[] AllOptions = { 0f, 5f, 10f, 15f, 20f, 25f, 30f, 35f, 40f, 45f, 50f, 55f, 60f, 65f, 70f, 75f, 80f, 85f, 90f, 95f, 100f };
        public static float[] BetweenOptions = { 20f, 30f, 40f, 50f, 60f, 70f, 80f, 90f, 100f };

        public static int ArchipelagoBaseId = 3000000;
		public static string ParkitectAPFilename = "config_parkitect.json";
		public static string ParkitectAPFolder = "Parkitect_Archipelago";

        public static string ModPath = null;
        public static string ScenarioName = null;
        public static bool Debug = true;

        public static Dictionary<Prefabs, string> AllGameItems = new Dictionary<Prefabs, string>
        {
            { Prefabs.AcceleratorCoaster, "Hydraulically-Launched Coaster"},
            { Prefabs.AlpineCoaster, "Alpine Coaster"},
            { Prefabs.AxisCoaster, "Pivot Coaster"},
            { Prefabs.BoatDarkRide, "Boat Dark Ride"},
            { Prefabs.BoatTransport, "Boat Transport"},
            { Prefabs.BobsledCoaster, "Bobsled Coaster"},
            { Prefabs.BumperBoats, "Bumper Boats"},
            { Prefabs.BumperCars, "Bumper Cars"},
            { Prefabs.CalmRiverRide, "Calm River Ride"},
            { Prefabs.Carousel, "Carousel"},
            { Prefabs.CarRide, "Car Ride"},
            { Prefabs.Cinema4D, "4D Cinema"},
            { Prefabs.Clockwork, "Clockwork"},
            { Prefabs.DoubleFerrisWheel, "Double Ferris Wheel"},
            { Prefabs.Elevator, "Elevator"},
            { Prefabs.Enterprise, "Enterprise"},
            { Prefabs.ExperienceRide, "Experience"},
            { Prefabs.FerrisWheel, "Ferris Wheel"},
            { Prefabs.FloorlessCoaster, "Floorless Coaster"},
            { Prefabs.FlyingCoaster, "Flying Coaster"},
            { Prefabs.FourDCoaster, "Vertical Spinning Coaster"},
            { Prefabs.GentleMonorailRide, "Gentle Monorail Ride"},
            { Prefabs.GhostMansionRide, "Ghost Mansion Ride"},
            { Prefabs.GigaCoaster, "Giga Coaster"},
            { Prefabs.GLock, "G-Lock"},
            { Prefabs.GoKarts, "Go-Karts"},
            { Prefabs.Gravitron, "Gravitron"},
            { Prefabs.GyroDropTower, "Gyro Drop Tower"},
            { Prefabs.HauntedHouse, "Haunted House"},
            { Prefabs.HeartBreaker, "Heart Breaker"},
            { Prefabs.HyperCoaster, "Hyper Coaster"},
            { Prefabs.InvertedCoaster, "Inverted Coaster"},
            { Prefabs.InvertedDarkRide, "Inverted Dark Ride"},
            { Prefabs.InvertingSpinningCoaster, "Inverting Spinning Coaster"},
            { Prefabs.InvertingWoodenCoaster, "Inverting Wooden Coaster"},
            { Prefabs.JeepRide, "Safari Ride"},
            { Prefabs.Jumper, "Jumper"},
            { Prefabs.JuniorCoaster, "Junior Coaster"},
            { Prefabs.LaunchedDropTower, "Launched Drop Tower"},
            { Prefabs.LogFlume, "Log Flume"},
            { Prefabs.MagicCarpet, "Magic Carpet"},
            { Prefabs.MineTrainCoaster, "Mine Train Coaster"},
            { Prefabs.MiniatureRailway, "Miniature Railway"},
            { Prefabs.MiniCoaster, "Mini Coaster"},
            { Prefabs.MiniMonorail, "Mini Monorail"},
            { Prefabs.Monorail, "Monorail"},
            { Prefabs.MonorailCoaster, "Monorail Coaster"},
            { Prefabs.MotionSimulator, "Motion Simulator"},
            { Prefabs.ObservationTower, "Observation Tower"},
            { Prefabs.Orbiter, "Orbiter"},
            { Prefabs.Paddleboats, "Paddleboats"},
            { Prefabs.PlaneCarousel, "Plane Carousel"},
            { Prefabs.PlaneCoaster, "Plane Coaster"},
            { Prefabs.PoweredCoaster, "Powered Coaster"},
            { Prefabs.PowerSurge, "Power Surge"},
            { Prefabs.Pulsar, "Pulsar"},
            { Prefabs.RiverRapids, "River Rapids"},
            { Prefabs.Rowboats, "Rowboats"},
            { Prefabs.Skyfall, "Skyfall"},
            { Prefabs.SpinningCoaster, "Spinning Coaster"},
            { Prefabs.SpiralSlide, "Spiral Slide"},
            { Prefabs.SplashBattle, "Splash Battle"},
            { Prefabs.StandupCoaster, "Stand-up Coaster"},
            { Prefabs.StarFlyer, "Star Flyer"},
            { Prefabs.StarShape, "Star Shape"},
            { Prefabs.SteelCoaster, "Steel Coaster"},
            { Prefabs.Steeplechase, "Steeplechase"},
            { Prefabs.SubmarineRide, "Submarines"},
            { Prefabs.SuspendedCoaster, "Suspended Coaster"},
            { Prefabs.SuspendedTrain, "Suspended Monorail"},
            { Prefabs.SwingingCoaster, "Swinging Coaster"},
            { Prefabs.SwingingMineTrainCoaster, "Swinging Mine Train Coaster"},
            { Prefabs.SwingingShip, "Swinging Ship"},
            { Prefabs.Teacups, "Teacups"},
            { Prefabs.TiltCoaster, "Tilt Coaster"},
            { Prefabs.ToppleTower, "Topple Tower"},
            { Prefabs.TopScan, "Top Scan"},
            { Prefabs.TopSpin, "Top Spin"},
            { Prefabs.Tourbillon, "Tourbillon"},
            { Prefabs.Transformer, "Transformer"},
            { Prefabs.Turbine, "Turbine"},
            { Prefabs.TwinHammer, "Inverted Double Swing"},
            { Prefabs.Twister, "Twister"},
            { Prefabs.VerticalDropCoaster, "Vertical Drop Coaster"},
            { Prefabs.WaterCoaster, "Water Coaster"},
            { Prefabs.WaveSwinger, "Wave Swinger"},
            { Prefabs.WildMouse, "Wild Mouse"},
            { Prefabs.WingCoaster, "Wing Coaster"},
            { Prefabs.WipeOut, "WipeOut"},
            { Prefabs.WoodenCoaster, "Wooden Coaster"},
            { Prefabs.BalloonShop, "Balloons"},
            { Prefabs.BubbleTeaStall, "Bubble Tea"},
            { Prefabs.BurgerStall, "Burgers"},
            { Prefabs.CandyStall, "Candy"},
            { Prefabs.CashMachine, "Cash Machine"},
            { Prefabs.ChineseFoodStall, "Chinese Food"},
            { Prefabs.Cookies, "Cookies"},
            { Prefabs.Corndogs, "Corndogs"},
            { Prefabs.CottonCandyStall, "Cotton Candy"},
            { Prefabs.CustomizableShop, "Customizable Shop"},
            { Prefabs.FirstAidRoom, "First Aid Room"},
            { Prefabs.FruitJuiceStall, "Fruit Juices"},
            { Prefabs.FunnelCakes, "Funnel Cakes"},
            { Prefabs.HotDogStall, "Hot Dogs"},
            { Prefabs.HotDrinksStall, "Hot Drinks"},
            { Prefabs.IceCreamStall, "Ice Cream"},
            { Prefabs.InfoKiosk, "Info Kiosk"},
            { Prefabs.MiniDonutsStall, "Mini Donuts"},
            { Prefabs.PizzaStall, "Pizza"},
            { Prefabs.PopcornStall, "Popcorn"},
            { Prefabs.PretzelStall, "Pretzels"},
            { Prefabs.ShirtsStall, "Shirts"},
            { Prefabs.SnowconesStall, "Snowcones"},
            { Prefabs.SoftDrinkStall, "Soft Drinks"},
            { Prefabs.SouvenirShop, "Souvenirs"},
            { Prefabs.SubSandwiches, "Sub Sandwiches"},
            { Prefabs.Toilets, "Toilets"},
            { Prefabs.TurkeyLegStall, "Turkey Legs"},
            { Prefabs.UmbrellaStall, "Umbrellas"},
            { Prefabs.VendingMachine, "Vending Machine"}
        };

        public static string[] AllNonItemTypes = (new[]
        {
            Constants.Trap.All,
            Constants.Attraction.Types,
            Constants.Stall.Types,
        })
            .SelectMany(a => a).ToArray();

        public static class Player
        {
			public static int[] SpeedupOptions = { 5, 7, 9 };
			public static float[] MoneyOptions = { 1000f, 2000f, 3000f, 4000f, 5000f, 10000f };
		}
	
		public static class Weather
        {
			public enum Options
			{
				RAINY,
				STORMY,
                CLOUDY,
                SUNNY
			}
		}

		public static class Guest
        {
			public static int[] SpawnOptions = { 25, 50, 75, 100, 125, 150 };
			public static int[] KillOptions = { 25, 50, 75, 100, 125, 150 };

			public static List<(int Start, int End)> VandalsOptions = new List<(int Start, int End)>
			{
				(3, 9),
				(3, 12),
				(6, 15),
                (9, 21)
			};

			// Guests Options - Chances how many of them will be
			public static float[] MoneyOptions = BetweenOptions;
            public static float[] HungryOptions = BetweenOptions;
            public static float[] ThirstyOptions = BetweenOptions;
            public static float[] BathroomOptions = BetweenOptions;
            public static float[] VomitOptions = BetweenOptions;
            public static float[] HappinessOptions = BetweenOptions;
            public static float[] TirednessOptions = BetweenOptions;

            // Guest Options - Ranges of its value
            public static float[] MoneyPercentage = Constants.AllOptions;
            public static float[] HungryPercentage = Constants.AllOptions;
			public static float[] ThirstyPercentage = Constants.AllOptions;
			public static float[] BathroomPercentage = Constants.AllOptions;
			public static float[] VomitPercentage = Constants.AllOptions;
			public static float[] HappinessPercentage = Constants.AllOptions;
			public static float[] TirednessPercentage = Constants.AllOptions;
		}

		public static class Employee
        {
			public static List<(int Start, int End)> TirednessRanges = new List<(int Start, int End)>
            {
                (45, 85),
                (60, 85),
                (75, 100),
                (90, 100),
            };
			public static List<(int Start, int End)> TrainingRanges = new List<(int Start, int End)>
			{
				(45, 85),
				(60, 85),
				(75, 100),
				(90, 100),
            };
			public static List<(int Start, int End)> SpawnRanges = new List<(int Start, int End)>
			{
				(1, 4),
				(2, 8),
				(3, 12),
                (4, 16)
			};
			public static Prefabs[] Options =
			{
				Prefabs.Mechanic,
				Prefabs.Janitor,
				Prefabs.Security,
				Prefabs.Entertainer,
				Prefabs.Handyman
			};
		}

		public static class Attraction
        {
			public static string[] BreakReason =
			{
				"RESTRAINTS_STUCK_OPEN",
				"RESTRAINTS_STUCK_CLOSED",
				"BRAKE_FAILURE",
				"BLOCK_BRAKE_FAILURE",
				"STATION_BRAKE_FAILURE",
				"CONTROL_FAILURE",
				"HYDRAULIC_LAUNCH_FAILURE",
				"STUCK"
			};

            public static string[] CalmRides =
            {
                Prefabs.Carousel.ToString(),
                Prefabs.Cinema4D.ToString(),
                Prefabs.BumperCars.ToString(),
                Prefabs.CarRide.ToString(),
                Prefabs.DoubleFerrisWheel.ToString(),
                Prefabs.FerrisWheel.ToString(),
                Prefabs.GentleMonorailRide.ToString(),
                Prefabs.GhostMansionRide.ToString(),
                Prefabs.HauntedHouse.ToString(),
                Prefabs.MagicCarpet.ToString(),
                Prefabs.MotionSimulator.ToString(),
                Prefabs.ObservationTower.ToString(),
                Prefabs.PlaneCarousel.ToString(),
                Prefabs.JeepRide.ToString(),
                Prefabs.SpiralSlide.ToString(),
                Prefabs.Teacups.ToString(),
                Prefabs.WaveSwinger.ToString(),
            };
			public static string[] ThrillRides =
			{
				Prefabs.Clockwork.ToString(),
				Prefabs.Enterprise.ToString(),
				Prefabs.ExperienceRide.ToString(),
				Prefabs.GLock.ToString(),
				Prefabs.GoKarts.ToString(),
				Prefabs.Gravitron.ToString(),
				Prefabs.GyroDropTower.ToString(),
				Prefabs.HeartBreaker.ToString(),
				Prefabs.InvertedDarkRide.ToString(),
				Prefabs.TwinHammer.ToString(), // Inverted Double Swing
				Prefabs.Jumper.ToString(),
				Prefabs.LaunchedDropTower.ToString(),
				Prefabs.Orbiter.ToString(),
				Prefabs.PowerSurge.ToString(),
				Prefabs.Skyfall.ToString(),
				Prefabs.StarFlyer.ToString(),
				Prefabs.StarShape.ToString(),
				Prefabs.SwingingShip.ToString(),
				Prefabs.ToppleTower.ToString(),
                Prefabs.TopScan.ToString(),
				Prefabs.TopSpin.ToString(),
				Prefabs.Tourbillon.ToString(),
				Prefabs.Transformer.ToString(),
				Prefabs.Turbine.ToString(),
				Prefabs.Twister.ToString(),
				Prefabs.WipeOut.ToString(),
			};
			public static string[] CoasterRides =
			{
				Prefabs.AlpineCoaster.ToString(),
				Prefabs.BobsledCoaster.ToString(),
				Prefabs.FloorlessCoaster.ToString(),
				Prefabs.FlyingCoaster.ToString(),
				Prefabs.GigaCoaster.ToString(),
				Prefabs.AcceleratorCoaster.ToString(),
				Prefabs.HyperCoaster.ToString(),
				Prefabs.InvertedCoaster.ToString(),
				Prefabs.InvertingSpinningCoaster.ToString(),
				Prefabs.InvertingWoodenCoaster.ToString(),
				Prefabs.JuniorCoaster.ToString(),
				Prefabs.MiniCoaster.ToString(),
                Prefabs.MineTrainCoaster.ToString(),
				Prefabs.MonorailCoaster.ToString(),
				Prefabs.AxisCoaster.ToString(), // Pivot Coaster
				Prefabs.PoweredCoaster.ToString(),
				Prefabs.SpinningCoaster.ToString(),
				Prefabs.StandupCoaster.ToString(),
				Prefabs.SteelCoaster.ToString(),
				Prefabs.Steeplechase.ToString(),
				Prefabs.SuspendedCoaster.ToString(),
				Prefabs.SwingingCoaster.ToString(),
                Prefabs.TiltCoaster.ToString(),
                Prefabs.VerticalDropCoaster.ToString(),
                Prefabs.FourDCoaster.ToString(), // Vertical Spinning Coaster
                Prefabs.WaterCoaster.ToString(),
                Prefabs.WildMouse.ToString(),
                Prefabs.WingCoaster.ToString(),
				Prefabs.WoodenCoaster.ToString(),
			};
			public static string[] TransportRides =
			{
				Prefabs.BoatTransport.ToString(),
				Prefabs.Elevator.ToString(),
				Prefabs.MiniatureRailway.ToString(),
				Prefabs.MiniMonorail.ToString(),
				Prefabs.Monorail.ToString(),
                Prefabs.SuspendedTrain.ToString(),
			};
			public static string[] WaterRides =
			{
				Prefabs.BoatDarkRide.ToString(),
				Prefabs.BumperBoats.ToString(),
				Prefabs.CalmRiverRide.ToString(),
				Prefabs.LogFlume.ToString(),
				Prefabs.Paddleboats.ToString(),
				Prefabs.RiverRapids.ToString(),
				Prefabs.Rowboats.ToString(),
				Prefabs.SplashBattle.ToString(),
				Prefabs.SubmarineRide.ToString(),
			};

			public static string[] Types =
			{
				"Calm Rides",
				"Thrill Rides",
				"Coaster Rides",
				"Transport Rides",
				"Water Rides",
            };

            public static string[] All = (new[]
            {
                Constants.Attraction.CalmRides,
                Constants.Attraction.ThrillRides,
                Constants.Attraction.CoasterRides,
                Constants.Attraction.TransportRides,
                Constants.Attraction.WaterRides
            })
                .SelectMany(a => a).ToArray();
        }

        public static class Stall
		{
			public static string[] Drinks =
			{
				Prefabs.BubbleTeaStall.ToString(),
				Prefabs.FruitJuiceStall.ToString(),
				Prefabs.HotDrinksStall.ToString(),
				Prefabs.SoftDrinkStall.ToString()
            };
			public static string[] Food =
			{
				Prefabs.BurgerStall.ToString(),
				Prefabs.CandyStall.ToString(),
				Prefabs.ChineseFoodStall.ToString(),
				Prefabs.Cookies.ToString(),
				Prefabs.Corndogs.ToString(),
				Prefabs.CottonCandyStall.ToString(),
				Prefabs.FunnelCakes.ToString(),
				Prefabs.HotDogStall.ToString(),
				Prefabs.IceCreamStall.ToString(),
				Prefabs.MiniDonutsStall.ToString(),
				Prefabs.PizzaStall.ToString(),
				Prefabs.PopcornStall.ToString(),
				Prefabs.PretzelStall.ToString(),
				Prefabs.SnowconesStall.ToString(),
				Prefabs.SubSandwiches.ToString(),
				Prefabs.TurkeyLegStall.ToString(),
			};
			public static string[] Shops =
			{
				Prefabs.BalloonShop.ToString(),
				Prefabs.CashMachine.ToString(),
				Prefabs.CustomizableShop.ToString(),
				Prefabs.FirstAidRoom.ToString(),
				Prefabs.InfoKiosk.ToString(),
				Prefabs.ShirtsStall.ToString(),
				Prefabs.SouvenirShop.ToString(),
				Prefabs.Toilets.ToString(),
				Prefabs.UmbrellaStall.ToString(),
				Prefabs.VendingMachine.ToString()
            };

            public static string[] Types =
            {
                "Shops"
            };

            public static string[] All = (new[]
            {
                Constants.Stall.Drinks,
                Constants.Stall.Food,
                Constants.Stall.Shops
            })
                .SelectMany(a => a).ToArray();
        }

		public static class Trap
        {
            public static string[] Attraction =
            {
                "Attraction Breakdown Trap",
                "Attraction Voucher Trap",
            };
            public static string[] Shop =
            {
                "Shop Ingredients Trap",
                "Shop Cleaning Trap",
                "Shop Voucher Trap",
            };
            public static string[] Employee =
            {
                "Employee Hiring Trap",
                "Employee Training Trap",
                "Employee Tiredness Trap",
            };
            public static string[] Player =
            {
                "Player Money Trap"
            };
            public static string[] Weather =
            {
                "Weather Rainy Trap",
                "Weather Stormy Trap",
                "Weather Cloudy Trap",
                "Weather Sunny Trap",
            };
            public static string[] Guest =
            {
                "Guest Spawn Trap",
                "Guest Kill Trap",
                "Guest Money Trap",
                "Guest Hunger Trap",
                "Guest Thirst Trap",
                "Guest Bathroom Trap",
                "Guest Vomiting Trap",
                "Guest Happiness Trap",
                "Guest Tiredness Trap",
                "Guest Vandal Trap",
            };

            public static string[] All = (new[]
            {
                Constants.Trap.Attraction,
                Constants.Trap.Shop,
                Constants.Trap.Employee,
                Constants.Trap.Player,
                Constants.Trap.Weather,
                Constants.Trap.Guest,
            })
                .SelectMany(a => a).ToArray();

            // "|" for secondary message
            public static char[] TextDivider = new char[] { '|' };
   
            public static string GetRandomText(string[] texts)
			{
                return texts[Randomizer.GetRandomInt(0, texts.Length - 1)];
            }

            // -----------------------------
            // Attraction Traps
            // -----------------------------
            public static string[] AttractionBreakdownTexts =
            {
				"Your Park Guests found some metal pieces|Following rides broke down: {{NAMES}}",
				"Some of your Attractions are very rusty|Get your mechanics over to {{NAMES}}",
				"Guests sabotaged your Attractions and they broke down|{{NAMES}} needs to be repaired",
				"A sudden storm hit your Park|Attractions damaged: {{NAMES}}",
				"The gears of progress stopped turning|Check on these Attractions: {{NAMES}}",
				"Maintenance neglected too long|These Attractions are out of order: {{NAMES}}",
				"Guests were not gentle with your rides|Repair needed for: {{NAMES}}",
				"Mechanical failure!|The following Attractions are temporarily closed: {{NAMES}}"
			};
            public static string[] GetAttractionBreakdownText(string[] attractions)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.AttractionBreakdownTexts);
                string tokenNames = Helper.SerializeText(attractions);

                return textBlock.Replace("{{NAMES}}", tokenNames).Split(Constants.Trap.TextDivider);
            }
   
			public static string[] AttractionVoucherTexts =
            {
				"You decided to make a giveaway for Attraction Vouchers",
                "Someone gave {{PERCENTAGE}}% of Guests a free Vouchers for Attractions...",
				"A Box full of old Attraction Vouchers appeared out of nowhere",
				"It's a special day! Guests can enjoy free Attraction Vouchers",
				"You rewarded your Guests with complimentary Vouchers for {{NAME}}",
				"Vouchers are raining down in your Park|A chance to enjoy attractions for free!",
				"Guests rejoice!|Some received {{PERCENTAGE}}% off on Attraction Vouchers",
                "An unexpected gift for Park Guests|Vouchers for {{NAME}} are available now"
            };
            public static string[] GetAttractionVoucherText(string attractions, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.AttractionVoucherTexts);
                return textBlock.Replace("{{NAME}}", attractions).Replace("{{PERCENTAGE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            // -----------------------------
            // Shop Traps
            // -----------------------------
            public static string[] ShopIngredientsTexts =
            {
				"Guests noticed the poor quality of {{SHOPS}} Ingredients|Time to restock with fresh supplies",
				"{{SHOPS}} Ingredients are running out of freshness|Better get a delivery going soon!",
				"Guests refuse to buy from {{SHOPS}} due to spoiled Ingredients|Order replacements immediately",
				"The smell of bad Ingredients is spreading from {{SHOPS}}|Replace them before guests start leaving!",
				"{{SHOPS}} have low-quality Ingredients in stock|Your guests deserve better food",
				"Rotten Ingredients detected in {{SHOPS}}|Your reputation is at risk — fix it fast!",
				"Guests are complaining about strange tastes from {{SHOPS}}|Check your Ingredients quality",
				"The latest delivery for {{SHOPS}} contained bad Ingredients|Inspect your stock carefully",
				"{{SHOPS}} received expired Ingredients batches|Get replacements before guests notice",
				"Suppliers reported an issue with Ingredients|Fresh ones are on their way"
			};
			public static string[] GetShopIngredientsText(string[] shops)
			{
				string textBlock = Constants.Trap.GetRandomText(Constants.Trap.ShopIngredientsTexts);
				string tokenShops = Helper.SerializeText(shops);

				return textBlock.Replace("{{SHOPS}}", tokenShops).Split(Constants.Trap.TextDivider);
			}
    
			public static string[] ShopCleaningTexts =
            {
				"Guests are complaining about the dirt in {{SHOPS}}|Maybe it’s time for some cleaning!",
				"{{SHOPS}} look filthy and uninviting|Your staff should grab some mops right now!",
				"The floors in {{SHOPS}} are sticky and gross|Clean them up before guests stop coming!",
				"Guests noticed trash piling up around {{SHOPS}}|Hire more janitors or clean it manually",
				"{{SHOPS}} are starting to smell bad|A good cleaning will fix that!",
				"Dirt is building up around {{SHOPS}}|Guests won’t enjoy buying there anymore",
				"Guests say {{SHOPS}} are disgusting|Better clean them before health inspectors arrive!",
				"{{SHOPS}} are getting unhygienic|Cleanliness affects guest happiness!",
				"Your staff forgot to clean {{SHOPS}} after closing|Guests will notice that mess!",
				"{{SHOPS}} look dull and grimy|A proper cleaning will freshen them up"
			};
			public static string[] GetShopCleaningText(string[] shops)
			{
				string textBlock = Constants.Trap.GetRandomText(Constants.Trap.ShopCleaningTexts);
				string tokenShops = Helper.SerializeText(shops);

				return textBlock.Replace("{{SHOPS}}", tokenShops).Split(Constants.Trap.TextDivider);
			}
 
			public static string[] ShopVoucherTexts =
            {
                "You decided to give out vouchers for your shops|Guests can enjoy them at: {{SHOP}}",
                "{{PERCENTAGE}}% of your Guests received free vouchers at {{SHOP}}",
                "A mysterious benefactor handed out vouchers|Redeemable at: {{SHOP}}",
                "Today only!|Visitors get {{PERCENTAGE}}% off at {{SHOP}}",
                "A surprise giveaway for hungry Guests|Vouchers available at: {{SHOP}}",
                "Someone donated vouchers|{{PERCENTAGE}}% of Guests can spend them at {{SHOP}}",
                "Vouchers for {{SHOP}} appeared in the Park gift shop|Everyone is excited",
                "Guests rejoice!|They received {{PERCENTAGE}}% worth of vouchers for {{SHOP}}",
                "Your Park is feeling generous|Visitors can grab vouchers at {{SHOP}} today",
                "An unexpected windfall|{{PERCENTAGE}}% of Guests can use vouchers at {{SHOP}}"
            };
			public static string[] GetShopVoucherText(string shop, float percentage)
			{
				string textBlock = Constants.Trap.GetRandomText(Constants.Trap.ShopVoucherTexts);
				return textBlock.Replace("{{SHOP}}", shop).Replace("{{PERCENTAGE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
			}

            // -----------------------------
            // Employee Traps
            // -----------------------------
            public static string[] EmployeeHiringTexts =
            {
                "New staff join your Park!|Hired {{AMOUNT}} {{EMPLOYEES}} employees",
                "Your team just got bigger|{{AMOUNT}} New hires: {{EMPLOYEES}}",
                "Talent arrives at the Park|Employees added: {{AMOUNT}} {{EMPLOYEES}}",
            };
            public static string[] GetEmployeeHiringText(string employeeType, int amount)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.EmployeeHiringTexts);
                return textBlock.Replace("{{EMPLOYEES}}", employeeType).Replace("{{AMOUNT}}", amount.ToString()).Split(Constants.Trap.TextDivider);
            }
      
			public static string[] EmployeeTrainingTexts =
            {
				"Employees are improving their skills|{{PERCENTAGE}}% of staff completed training",
                "Training day!|{{PERCENTAGE}}% of your employees leveled up",
                "Staff development in progress|{{PERCENTAGE}}% of employees trained successfully",
            };
            public static string[] GetEmployeeTrainingText(string percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.EmployeeTrainingTexts);
                return textBlock.Replace("{{PERCENTAGE}}", percentage).Split(Constants.Trap.TextDivider);
            }
      
			public static string[] EmployeeTirednessTexts =
            {
				"Exhaustion spreads among your staff|{{PERCENTAGE}}% of employees are tired",
                "Staff energy drops|{{PERCENTAGE}}% of employees need rest",
                "Overworked team members|{{PERCENTAGE}}% of employees are fatigued"
            };
            public static string[] GetEmployeeTirednessText(string percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.EmployeeTirednessTexts);
                return textBlock.Replace("{{PERCENTAGE}}", percentage).Split(Constants.Trap.TextDivider);
            }

            // -----------------------------
            // Player Traps
            // -----------------------------
            public static string[] PlayerMoneyTexts =
			{
				"Your wallet feels different|You gained {{AMOUNT}}",
				"Money comes and goes|Player balance changed by {{AMOUNT}}",
				"A sudden financial surprise|You made {{AMOUNT}}",
				"Cash flow alert!|Your money raised by {{AMOUNT}}",
				"Your bank account reacts|Change: +{{AMOUNT}}",
				"Coins appear mysteriously|{{AMOUNT}} appeared in your pocket",
				"A twist of fate affects your money|You gained {{AMOUNT}}",
				"Luck strikes your wallet|You found {{AMOUNT}} on the ground"
			};
            public static string[] GetPlayerMoneyText(float amount)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.PlayerMoneyTexts);
                string tokenAmount = $"{ amount }$";
                return textBlock.Replace("{{AMOUNT}}", tokenAmount).Split(Constants.Trap.TextDivider);
            }

            // -----------------------------
            // Weather Traps
            // -----------------------------
            public static string[] WeatherBadTexts =
			{
				"Dark clouds gather over the Park|It's getting {{WEATHER}}",
				"Guests are running for cover|A {{WEATHER}} has started",
				"Nature strikes again|Watch out for the {{WEATHER}}",
				"Umbrellas everywhere!|The Park is affected by {{WEATHER}}",
				"Lightning flashes and thunder rolls|A {{WEATHER}} is upon you",
				"Rain puddles everywhere|A sudden {{WEATHER}} caught your Guests off guard",
				"Hold onto your hats!|The Park is experiencing {{WEATHER}}",
				"Weather alert!|Prepare for {{WEATHER}}",
				"The sky darkens ominously|A {{WEATHER}} is brewing",
				"Guests are slipping and sliding|Thanks to a {{WEATHER}}"
			};
            public static string[] WeatherGoodTexts =
            {
                "The sun shines brightly over the Park|It's a beautiful {{WEATHER}} day",
                "Guests are smiling everywhere|Perfect weather for a {{WEATHER}}",
                "The Park feels alive|Enjoy the wonderful {{WEATHER}}",
                "A calm breeze passes through|It’s a lovely {{WEATHER}} today",
                "Nature is at peace|The {{WEATHER}} couldn’t be better",
                "Everything sparkles under the light|A perfect day of {{WEATHER}}",
                "Visitors are taking off their jackets|What a pleasant {{WEATHER}}",
                "Music and laughter fill the air|The {{WEATHER}} sets the perfect mood",
                "Time for a stroll in the Park|The {{WEATHER}} is just right",
                "Guests are relaxing happily|Enjoying the gentle {{WEATHER}}"
            };
            public static string[] GetWeatherText(string weather)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.WeatherBadTexts);

                if (weather == "Cloudy" || weather == "Sunny")
                {
                    textBlock = Constants.Trap.GetRandomText(Constants.Trap.WeatherGoodTexts);
                }
                return textBlock.Replace("{{WEATHER}}", weather).Split(Constants.Trap.TextDivider);
            }

            // -----------------------------
            // Guest Traps
            // -----------------------------
            public static string[] GuestSpawnTexts =
            {
                "Guests are flooding in!|{{AMOUNT}} new Guests have appeared",
                "A wave of visitors arrives|{{AMOUNT}} Guests spawned in the Park",
                "Unexpected crowd!|{{AMOUNT}} more Guests have entered",
                "The gates swing open magically|{{AMOUNT}} Guests appear from nowhere",
                "Visitors multiply mysteriously|{{AMOUNT}} new Guests just arrived",
                "Guest chaos!|{{AMOUNT}} new arrivals are making the Park lively",
                "A sudden rush of Guests|The Park population grows by {{AMOUNT}}",
                "Visitors appear out of thin air|{{AMOUNT}} Guests are filling up the Park",
                "Crowds gather unexpectedly|{{AMOUNT}} new Guests are here",
                "Your Park is bustling|{{AMOUNT}} Guests have spawned everywhere"
            };
            public static string[] GetGuestSpawnText(int amount)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestSpawnTexts);
                return textBlock.Replace("{{AMOUNT}}", amount.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestKillTexts =
            {
                "Something terrible happened!|{{AMOUNT}} Guests were lost",
                "Chaos in the Park!|{{AMOUNT}} Guests met an unfortunate fate",
                "The Park just got quieter|{{AMOUNT}} Guests are no longer here",
                "Tragedy strikes!|{{AMOUNT}} Guests have disappeared mysteriously",
                "Visitors vanished suddenly|{{AMOUNT}} Guests removed from the Park",
                "A grim turn of events|{{AMOUNT}} Guests didn’t make it",
                "Park attendance drops|{{AMOUNT}} Guests are gone",
                "Something went horribly wrong|{{AMOUNT}} Guests were taken away",
                "The crowd thins unexpectedly|{{AMOUNT}} Guests are missing",
                "A shocking event!|{{AMOUNT}} Guests have been eliminated"
            };
            public static string[] GetGuestKillText(int amount)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestKillTexts);
                return textBlock.Replace("{{AMOUNT}}", amount.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestMoneyGainTexts =
            {
                "Guests are feeling lucky|{{PERCENTAGE_GUESTS}}% of your Guests gain {{AMOUNT}}",
                "Unexpected fortunes!|{{PERCENTAGE_GUESTS}}% of Guests gain {{AMOUNT}} each",
                "Cash flows weirdly|{{PERCENTAGE_GUESTS}}% of Guests each gain {{AMOUNT}}",
                "Visitors react to sudden wealth changes|{{PERCENTAGE_GUESTS}}% of Guests gain {{AMOUNT}}",
                "A twist of fate hits your Guests|{{PERCENTAGE_GUESTS}}% of Guests each gain {{AMOUNT}}",
                "Guests cheer or groan|{{PERCENTAGE_GUESTS}}% of Guests have their money increased by {{AMOUNT}}",
                "Coin chaos!|{{PERCENTAGE_GUESTS}}% of Guests gain {{AMOUNT}} coins",
                "Guests’ wallets fluctuate|{{PERCENTAGE_GUESTS}}% of them gain {{AMOUNT}}",
                "Luck strikes!|{{PERCENTAGE_GUESTS}}% of Guests gain {{AMOUNT}}",
                "A financial surprise for Guests|{{PERCENTAGE_GUESTS}}% of them gain {{AMOUNT}} more"
            };

            public static string[] GuestMoneyLoseTexts =
            {
                "Guests are feeling unlucky|{{PERCENTAGE_GUESTS}}% of your Guests lose {{AMOUNT}}",
                "Unexpected misfortunes!|{{PERCENTAGE_GUESTS}}% of Guests lose {{AMOUNT}} each",
                "Cash flows weirdly|{{PERCENTAGE_GUESTS}}% of Guests each lose {{AMOUNT}}",
                "Visitors react to sudden wealth changes|{{PERCENTAGE_GUESTS}}% of Guests lose {{AMOUNT}}",
                "A twist of fate hits your Guests|{{PERCENTAGE_GUESTS}}% of Guests each lose {{AMOUNT}}",
                "Guests groan in frustration|{{PERCENTAGE_GUESTS}}% of Guests have their money decreased by {{AMOUNT}}",
                "Coin chaos!|{{PERCENTAGE_GUESTS}}% of Guests lose {{AMOUNT}} coins",
                "Guests’ wallets fluctuate|{{PERCENTAGE_GUESTS}}% of them lose {{AMOUNT}}",
                "Misfortune strikes!|{{PERCENTAGE_GUESTS}}% of Guests lose {{AMOUNT}}",
                "A financial setback for Guests|{{PERCENTAGE_GUESTS}}% of them lose {{AMOUNT}}"
            };
            public static string[] GetGuestMoneyText(float amount, float percentage, string sign)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestMoneyGainTexts);
                string tokenAmount = $"{amount}$";

                if (sign == "-")
                {
                    textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestMoneyLoseTexts);
                }

                return textBlock.Replace("{{AMOUNT}}", tokenAmount).Replace("{{PERCENTAGE_GUESTS}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestHungerTexts =
			{
                "Guests are feeling hungry|{{PERCENTAGE_GUESTS}}% of Guests gain {{PERCENTAGE_VALUE}}% Hunger",
				"A rumbling in their stomachs|{{PERCENTAGE_GUESTS}}% of Guests affected by {{PERCENTAGE_VALUE}}% Hunger",
				"Snack time crisis!|Hunger increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
				"Guests are craving food|{{PERCENTAGE_GUESTS}}% of Visitors gain {{PERCENTAGE_VALUE}}% Hunger",
				"The Park’s smell of food backfires|{{PERCENTAGE_VALUE}}% Hunger applied to {{PERCENTAGE_GUESTS}}% of Guests",
            };
            public static string[] GetGuestHungerText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestHungerTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }
          
			public static string[] GuestThirstTexts =
            {
				"Guests are parched|{{PERCENTAGE_GUESTS}}% of Guests gain {{PERCENTAGE_VALUE}}% Thirst",
				"Sudden thirst hits the Park|{{PERCENTAGE_VALUE}}% Thirst added to {{PERCENTAGE_GUESTS}}% of Guests",
				"Visitors need a drink|{{PERCENTAGE_GUESTS}}% of Guests affected by {{PERCENTAGE_VALUE}}% Thirst",
				"A dry spell in the Park|{{PERCENTAGE_VALUE}}% Thirst for {{PERCENTAGE_GUESTS}}% of Guests",
				"Guests are complaining|Thirst increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
			};
            public static string[] GetGuestThirstText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestThirstTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestBathroomTexts =
            {
                "Nature calls!|{{PERCENTAGE_GUESTS}}% of Guests need to go {{PERCENTAGE_VALUE}}% more urgently",
                "Guests are panicking|Bathroom urgency rises by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "A restroom emergency!|{{PERCENTAGE_GUESTS}}% of Guests affected, urgency +{{PERCENTAGE_VALUE}}%",
                "Bathroom chaos in the Park|{{PERCENTAGE_VALUE}}% urgency increase for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests are uncomfortable|{{PERCENTAGE_GUESTS}}% of Visitors need the bathroom {{PERCENTAGE_VALUE}}% more",
                "Hurry, the lines are long!|Bathroom need +{{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Visitors are squirming|{{PERCENTAGE_GUESTS}}% of Guests have +{{PERCENTAGE_VALUE}}% bathroom urgency",
                "Restroom mayhem!|{{PERCENTAGE_VALUE}}% urgency applied to {{PERCENTAGE_GUESTS}}% of Guests",
                "The Park’s hygiene alert|{{PERCENTAGE_GUESTS}}% of Guests need the bathroom more urgently (+{{PERCENTAGE_VALUE}}%)",
                "Guests are in distress|Bathroom need increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests"
            };
            public static string[] GetGuestBathroomText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestBathroomTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestVomitingTexts =
            {
                "Disaster in the Park!|{{PERCENTAGE_GUESTS}}% of Guests are vomiting (+{{PERCENTAGE_VALUE}}%)",
                "Gross!|Vomiting increases by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests are sickened|{{PERCENTAGE_GUESTS}}% affected, Vomiting +{{PERCENTAGE_VALUE}}%",
                "Chaos in the queues!|Vomiting rises by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Visitors",
                "Park hygiene crisis|{{PERCENTAGE_GUESTS}}% of Guests now vomiting (+{{PERCENTAGE_VALUE}}%)",
                "An unpleasant turn of events|Vomiting increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests feel ill|{{PERCENTAGE_VALUE}}% Vomiting applied to {{PERCENTAGE_GUESTS}}% of Visitors",
                "Nausea spreads through the Park|{{PERCENTAGE_GUESTS}}% of Guests affected (+{{PERCENTAGE_VALUE}}%)",
                "Visitors are queasy|Vomiting stat +{{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "A stomach-churning moment!|{{PERCENTAGE_GUESTS}}% of Guests vomiting increased by {{PERCENTAGE_VALUE}}%"
            };
            public static string[] GetGuestVomitingText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestVomitingTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestHappinessTexts =
            {
                "Guests are feeling uneasy|Happiness changed by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "A sour mood spreads|{{PERCENTAGE_GUESTS}}% of Guests affected, Happiness +{{PERCENTAGE_VALUE}}%",
                "Visitors are grumpy|Happiness adjusted by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Cheerfulness drops|{{PERCENTAGE_VALUE}}% Happiness change for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests are less thrilled|Happiness altered by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Visitors",
                "Mood swings in the Park|{{PERCENTAGE_GUESTS}}% of Guests have Happiness changed by {{PERCENTAGE_VALUE}}%",
                "Visitors are less content|Happiness +{{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "A shift in feelings|{{PERCENTAGE_VALUE}}% Happiness applied to {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests react unpredictably|Happiness changes by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Visitors",
                "Park mood alert|{{PERCENTAGE_GUESTS}}% of Guests Happiness adjusted by {{PERCENTAGE_VALUE}}%"
            };
            public static string[] GetGuestHappinessText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestHappinessTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestTirednessTexts =
            {
                "Guests are getting exhausted|Tiredness increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Visitors feel sleepy|{{PERCENTAGE_GUESTS}}% of Guests Tiredness +{{PERCENTAGE_VALUE}}%",
                "Fatigue spreads through the Park|{{PERCENTAGE_VALUE}}% Tiredness applied to {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests are dragging their feet|Tiredness rises by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Visitors",
                "A sleepy crowd|{{PERCENTAGE_GUESTS}}% of Guests affected, Tiredness +{{PERCENTAGE_VALUE}}%",
                "Visitors need rest|Tiredness increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests are worn out|{{PERCENTAGE_VALUE}}% Tiredness added to {{PERCENTAGE_GUESTS}}% of Guests",
                "Fatigue alert!|Tiredness +{{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests struggle to stay awake|{{PERCENTAGE_GUESTS}}% affected, Tiredness +{{PERCENTAGE_VALUE}}%",
                "Park visitors are exhausted|Tiredness increased by {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests"
            };
            public static string[] GetGuestTirednessText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestTirednessTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }
       
            public static string[] GuestVandalsTexts =
            {
                "Chaos in the Park!|{{AMOUNT}} Guests are vandalizing",
                "Park mischief!|{{AMOUNT}} Guests causing damage",
                "Guests are causing trouble|{{AMOUNT}} Visitors misbehaving",
                "Graffiti and destruction everywhere|{{AMOUNT}} Guests running amok",
                "Visitors run wild|{{AMOUNT}} Guests vandalizing the Park",
                "Park chaos alert|{{AMOUNT}} Guests are breaking things",
                "Guests misbehave|{{AMOUNT}} causing havoc",
                "Trouble in the Park|{{AMOUNT}} Guests vandalizing",
                "A wild crowd|{{AMOUNT}} Guests are creating chaos",
                "Park property suffers|{{AMOUNT}} Guests causing damage"
            };
            public static string[] GetGuestVandalsTexts(int amount)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestVandalsTexts);
                return textBlock.Replace("{{AMOUNT}}", amount.ToString()).Split(Constants.Trap.TextDivider);
            }
        }

        public static class Scenario
		{
			public static Dictionary<int, string> Maps = new Dictionary<int, string>
			{
                { 0, "Archipelago - Lakeside Gardens" },
                { 1, "Archipelago - Dusty Ridge Ranch" }
            };
		}
    }
}
