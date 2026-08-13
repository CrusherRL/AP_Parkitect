using Photon.Realtime;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArchipelagoMod.Src
{
    class Constants
    {
        public const string IDENTIFIER = "com.parkitectCommunity.Archipelago";
        public const string VERSION = "1.6.2";
        public static float[] AllOptions = { 0f, 5f, 10f, 15f, 20f, 25f, 30f, 35f, 40f, 45f, 50f, 55f, 60f, 65f, 70f, 75f, 80f, 85f, 90f, 95f, 100f };
        public static float[] BetweenOptions = { 40f, 50f, 60f, 70f, 80f, 90f, 100f };

        public static string Playername = null;
        public static int ArchipelagoBaseId = 3000000;

		public static string ParkitectAPFilename = "ap_config.json";
		public static string ParkitectAPFolder = "Archipelago";
        public static string ParkitectSavegamesFolder = "Savegames";
        public static string ParkitectDebugLogFilename = "debug.log.txt";

        public static string ParkitectCampaignBackupFilenameSuffix = "before_archipelago";

        public static string ModPath = null;
        public static string ConfigPath = null;
        public static string SaveGamesPath = null;
        public static string ScenarioName = null;
        public static bool Debug = true;
        public static bool DontConnect = false;
        public static bool LogStats = false;

        public static readonly CultureInfo GermanCulture = new CultureInfo("de-DE");

        public static class Commands
        {
            public class TrapLink
            {
                public static string Toggle = "!!toggleTrapLink".ToLower();
                public static string Join = "!!joinTrapLink".ToLower();
                public static string Leave = "!!leaveTrapLink".ToLower();

                public static string[] All =
                {
                    Constants.Commands.TrapLink.Toggle,
                    Constants.Commands.TrapLink.Join,
                    Constants.Commands.TrapLink.Leave,
                };
            }

            public class ReleaseMode
            {
                public static string Toggle = "!!toggleReleaseMode".ToLower();
                public static string Disable = "!!disableReleaseMode".ToLower();
                public static string Enable = "!!enableReleaseMode".ToLower();

                public static string[] All =
                {
                    Constants.Commands.ReleaseMode.Toggle,
                    Constants.Commands.ReleaseMode.Disable,
                    Constants.Commands.ReleaseMode.Enable,
                };
            }

            public static string[] All = (new[]
            {
                Constants.Commands.TrapLink.All,
                Constants.Commands.ReleaseMode.All,
            })
                .SelectMany(a => a).ToArray();
        }

        public static float NextCheckTimeDelay = .45f;

        public static class TrapLink
        {
            public static class OpenRCT2
            {
                public static string BathroomTrap = "Bathroom Trap";
                public static string FurryConventionTrap = "Furry Convention Trap";
                public static string FoodPoisoningTrap = "Food poisoning Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.OpenRCT2.BathroomTrap,
                    Constants.TrapLink.OpenRCT2.FurryConventionTrap,
                    Constants.TrapLink.OpenRCT2.FoodPoisoningTrap,
                })
                    .ToArray();
            }
            public static class Pokemon
            {
                public static string BurnTrap = "Burn Trap";
                public static string FireTrap = "Fire Trap";
                public static string PoisonTrap = "Poison Trap";
                public static string SleepTrap = "Sleep Trap";
                public static string IceTrap = "Ice Trap";
                public static string FreezeTrap = "Freeze Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.Pokemon.BurnTrap,
                    Constants.TrapLink.Pokemon.FireTrap,
                    Constants.TrapLink.Pokemon.PoisonTrap,
                    Constants.TrapLink.Pokemon.SleepTrap,
                    Constants.TrapLink.Pokemon.IceTrap,
                    Constants.TrapLink.Pokemon.FreezeTrap,
                })
                    .ToArray();
            }
            public static class BraveFencerMusashi
            {
                public static string StinkyTrap = "Stinky Trap";
                public static string ToxinTrap = "Toxin Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.BraveFencerMusashi.StinkyTrap,
                    Constants.TrapLink.BraveFencerMusashi.ToxinTrap,
                })
                    .ToArray();
            }
            public static class FreedomPlanet2
            {
                public static string ExpensiveStocks = "Expensive Stocks";
                public static string NoStocks = "No Stocks";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.FreedomPlanet2.ExpensiveStocks,
                    Constants.TrapLink.FreedomPlanet2.NoStocks,
                })
                    .ToArray();
            }
            public static class Hammerwatch
            {
                public static string FrostTrap = "Frost Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.Hammerwatch.FrostTrap,
                })
                    .ToArray();
            }
            public static class Noita
            {
                public static string PeaSoupTrap = "Pea Soup Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.Noita.PeaSoupTrap,
                })
                    .ToArray();
            }
            public static class GTA_SA
            {
                public static string Fat_CJ_Trap = "Fat CJ Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.GTA_SA.Fat_CJ_Trap,
                })
                    .ToArray();
            }
            public static class Misc
            {
                public static string FrozenTrap = "Frozen Trap";

                public static string[] All = (new[]
                {
                    Constants.TrapLink.Misc.FrozenTrap,
                })
                    .ToArray();
            }
        }

        public static string[] TrapLinks = (new[]
        {
            Constants.TrapLink.OpenRCT2.All,
            Constants.TrapLink.Pokemon.All,
            Constants.TrapLink.BraveFencerMusashi.All,
            Constants.TrapLink.FreedomPlanet2.All,
            Constants.TrapLink.Hammerwatch.All,
            Constants.TrapLink.Noita.All,
            Constants.TrapLink.GTA_SA.All,
            Constants.TrapLink.Misc.All,
        })
            .SelectMany(a => a)
            .ToArray();

        // Decorations and Statistics are not listen here, since there is not prefabs for that
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
            { Prefabs.VendingMachine, "Vending Machine"},
            { Prefabs.Depot, "Depot"},
            { Prefabs.StaffRoom, "Staff Room"},
            { Prefabs.TrainingRoom, "Training Room"},
            { Prefabs.TrashChute, "Trash Chute"},
        };

        public static class Player
        {
			public static int[] SpeedupOptions = { 4, 5, 6, 7, 8, 9 };
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

            public static Dictionary<Prefabs, List<int>> ExperienceLevels = new Dictionary<Prefabs, List<int>>()
            {
                { Prefabs.Mechanic, new List<int> { 2, 3, 4, 5 } },
                { Prefabs.Janitor, new List<int> { 2, 3, 4, 5 } },
                { Prefabs.Security, new List<int> { 1, 2, 3, 4 } },
                { Prefabs.Entertainer, new List<int> { 4, 5, 7, 9 } },
                { Prefabs.Handyman, new List<int> { 2, 3, 4, 5 } }
            };
		}

		public static class Attraction
        {
            public static string GenericType = "Rides";

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
				Prefabs.Pulsar.ToString(),
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
				Prefabs.PlaneCoaster.ToString(),
				Prefabs.PoweredCoaster.ToString(),
                Prefabs.SpinningCoaster.ToString(),
				Prefabs.StandupCoaster.ToString(),
				Prefabs.SteelCoaster.ToString(),
				Prefabs.Steeplechase.ToString(),
				Prefabs.SuspendedCoaster.ToString(),
				Prefabs.SwingingCoaster.ToString(),
                Prefabs.SwingingMineTrainCoaster.ToString(),
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
                Constants.Attraction.GenericType,
            };

            public static string[] All = (new[]
            {
                Constants.Attraction.CalmRides,
                Constants.Attraction.ThrillRides,
                Constants.Attraction.CoasterRides,
                Constants.Attraction.TransportRides,
                Constants.Attraction.WaterRides
            })
                .SelectMany(a => a)
                .ToArray();

            public static string DetermineType(string name)
            {
                if (Constants.Attraction.CalmRides.Contains(name))
                {
                    return Constants.Attraction.Types[0];
                }

                if (Constants.Attraction.ThrillRides.Contains(name))
                {
                    return Constants.Attraction.Types[1];
                }
                
                if (Constants.Attraction.CoasterRides.Contains(name))
                {
                    return Constants.Attraction.Types[2];
                }

                if (Constants.Attraction.TransportRides.Contains(name))
                {
                    return Constants.Attraction.Types[3];
                }

                if (Constants.Attraction.WaterRides.Contains(name))
                {
                    return Constants.Attraction.Types[4];
                }

                return Constants.Mods.GetType(name);
            }

            public static string[] DecoRatings =
            { // Order is important here!
                "Amazing",
                "High",
                "Medium",
                "Low",
                "Bad",
                "Very low", // should never happen
            };
        }

        public static class Stall
		{
            public static string GenericType = "Shops";
            public static string FacilityType = "Facilities";
            public static string FacilityTypeLabel = "Non-Food + Non-Drink Shops";

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
			public static string[] Facilities =
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
                "Drinks",
                "Food",
                Constants.Stall.FacilityType,
                Constants.Stall.GenericType,
            };

            public static string[] All = (new[]
            {
                Constants.Stall.Drinks,
                Constants.Stall.Food,
                Constants.Stall.Facilities
            })
                .SelectMany(a => a).ToArray();

            public static string DetermineType(string name)
            {
                if (Constants.Stall.Drinks.Contains(name))
                {
                    return Constants.Stall.Types[0];
                }

                if (Constants.Stall.Food.Contains(name))
                {
                    return Constants.Stall.Types[1];
                }

                if (Constants.Stall.Facilities.Contains(name))
                {
                    return Constants.Stall.Types[2];
                }

                return Constants.Mods.GetType(name);
            }
        }

        public static class UtilityBuilding
        {
            public static string[] All = (new[]
            {
                Prefabs.Depot.ToString(),
                Prefabs.StaffRoom.ToString(),
                Prefabs.TrainingRoom.ToString(),
                Prefabs.TrashChute.ToString(),
            });
        }

        public static class Decorations
        {
            public static string[] Excludes =
            {
                "Scenario Marker",
            };

            public static string[] ThemeTags = {
                "Generic",
                "Spooky",
                "Medieval",
                "Steamworks",
                "Science Fiction",
                "Western",
                "Fantasy",
                "Candyland",
                "Adventure",
                "Classic", // Ancient World
                "Dino",
            };

            public static Dictionary<string, List<string>> ThemeTagMapToProps = new Dictionary<string, List<string>>()
            {
                { "Generic", new List<string>() { "Effects", "Race Props", "Sculptures and Statues", "Topiaries" } },
                { "Spooky", new List<string>() { "Spooky Props", "Spooky Structures", } },
                { "Medieval", new List<string>() { "Medieval Props", "Medieval Structures", } },
                { "Steamworks", new List<string>() { "Steam Pipes", "Steamworks Props", "Industrial Structures", } },
                { "Science Fiction", new List<string>() { "Sci-Fi Props", "Sci-Fi Structures", } },
                { "Western", new List<string>() { "Western Props" } },
                { "Fantasy", new List<string>() { "Fantasy" } },
                { "Candyland", new List<string>() { "Candyland" } },
                { "Adventure", new List<string>() { "Adventure" } },
                { "Classic", new List<string>() { "Ancient World" } },
                { "Dino", new List<string>() { "Dino" } },
            };

            public static string[] All = (new[]
            {
                Constants.Decorations.ThemeTags,
            })
            .SelectMany(a => a)
            .ToArray();
        }

        public static class Statistics
            {
                public static readonly Dictionary<string, string> map = new Dictionary<string, string>()
                {
                    { "statAvgAttractionsVisited", "Average attractions visited" },
                    { "statAvgFoodConsumed", "Average food consumed" },
                    { "statAvgMoneySpent", "Average money spent" },
                    { "statAvgQueueTime", "Average queue time" },
                    { "statAvgTimeInPark", "Average time in park" },
                    { "statsCustomersLastMonth", "Customers last month" },
                    { "statDecoPrice", "Deco construction costs" },
                    { "statDecoValue", "Deco remaining value" },
                    { "statMissedCustomersLastMonth", "Missed customers last month" },
                    { "statMostProfitableAttraction", "Most profitable attraction" },
                    { "statMostProfitableShop", "Most profitable shop" },
                    { "statVouchersRedeemed", "Redeemed vouchers" },
                    { "statRidesPrice", "Rides construction costs" },
                    { "statRidesValue", "Rides remaining value" },
                    { "statShopsPrice", "Shops construction costs" },
                    { "statShopsValue", "Shops remaining value" },
                    { "statTotalAttractionBreakdowns", "Total attraction breakdowns" },
                    { "statTotalAttractionCustomers", "Total attraction customers" },
                    { "statTotalAttractionProfit", "Total attraction profit" },
                    { "statTotalAttractionRevenue", "Total attraction revenue" },
                    { "statTotalAttractionsMaintained", "Total attractions maintained" },
                    { "statTotalAttractionsRepaired", "Total attractions repaired" },
                    { "statTotalCratesDelivered", "Total crates delivered" },
                    { "statTotalFootpathsSwept", "Total footpaths swept" },
                    { "statTotalGuests", "Total guests" },
                    { "statTotalGuestsEntertained", "Total guests entertained" },
                    { "statTotalShopProfit", "Total shop profit" },
                    { "statTotalShopRevenue", "Total shop revenue" },
                    { "statTotalToiletsScrubbed", "Total toilets scrubbed" },
                    { "statTotalTrashBinsEmptied", "Total trash bins emptied" },
                    { "statTotalPathAttachmentsRepaired", "Total vandalized objects repaired" },
                    { "statTotalVandalsCaught", "Total vandals caught" }
                };

                public static string[] All = Constants.Statistics.map.Values.ToArray();
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
                "Employee Tiredness Trap",
                "Employee Training Trap",
            };
            public static string[] Player =
            {
                "Player Money Trap"
            };
            public static string[] Weather =
            {
                "Weather Cloudy Trap",
                "Weather Rainy Trap",
                "Weather Stormy Trap",
                "Weather Sunny Trap",
            };
            public static string[] Guest =
            {
                "Guest Bathroom Trap",
                "Guest Happiness Trap",
                "Guest Hunger Trap",
                "Guest Kill Trap",
                "Guest Money Trap",
                "Guest Spawn Trap",
                "Guest Thirst Trap",
                "Guest Tiredness Trap",
                "Guest Vandal Trap",
                "Guest Vomiting Trap",
            };
            public static string[] Research =
            {
                "Research Trap",
            };

            public static string[] All = (new[]
            {
                Constants.Trap.Attraction,
                Constants.Trap.Shop,
                Constants.Trap.Employee,
                Constants.Trap.Player,
                Constants.Trap.Weather,
                Constants.Trap.Guest,
                Constants.Trap.Research,
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
			public static string[] AttractionVoucherTexts =
            {
				"You decided to make a giveaway for Attraction Vouchers",
                "Someone gave {{PERCENTAGE}}% of Guests a free Vouchers for a Attraction...",
				"A Box full of old Attraction Vouchers appeared out of nowhere",
				"It's a special day! Guests can enjoy free Attraction Vouchers",
				"You rewarded your Guests with complimentary Vouchers for {{NAME}}",
				"Vouchers are raining down in your Park|A chance to enjoy attractions for free!",
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
				"Rotten Ingredients detected in {{SHOPS}}|Your reputation is at risk",
				"Guests are complaining about strange tastes from {{SHOPS}}|Check your Ingredients quality",
				"The latest delivery for {{SHOPS}} contained bad Ingredients|Inspect your stock carefully",
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
                "A surprise giveaway for hungry/thirsty Guests|Vouchers available at: {{SHOP}}",
                "Someone donated vouchers|{{PERCENTAGE}}% of Guests can spend them at {{SHOP}}",
                "Vouchers for {{SHOP}} appeared in the Park gift shop|Everyone is excited",
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
                "New staff joined your Park!|Hired {{AMOUNT}} {{EMPLOYEE}} employees",
                "Your team just got bigger|{{AMOUNT}} New hires: {{EMPLOYEE}}",
                "Talent arrives at the Park|Employees added: {{AMOUNT}} {{EMPLOYEE}}",
            };
            public static string[] GetEmployeeHiringText(string employeeType, int amount)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.EmployeeHiringTexts);
                return textBlock.Replace("{{EMPLOYEE}}", employeeType).Replace("{{AMOUNT}}", amount.ToString()).Split(Constants.Trap.TextDivider);
            }
      
			public static string[] EmployeeTrainingTexts =
            {
                "Employees will attend training|{{PERCENTAGE}}% of staff scheduled for training",
                "Training day ahead!|{{PERCENTAGE}}% of your employees will go for training",
                "Staff training upcoming|{{PERCENTAGE}}% of employees will attend sessions",
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
                "Guests are running for cover|A {{WEATHER}} weather has started",
                "Nature strikes again|Watch out for the {{WEATHER}} weather",
                "Umbrellas everywhere!|The Park is affected by {{WEATHER}} weather",
                "Rain puddles everywhere|A sudden {{WEATHER}} weather caught your Guests off guard",
                "Hold onto your hats!|The Park is experiencing {{WEATHER}} weather",
                "Weather alert!|Prepare for {{WEATHER}} weather",
                "The sky darkens ominously|A {{WEATHER}} weather is brewing",
                "Guests are slipping and sliding|Thanks to a {{WEATHER}} weather"
            };
            public static string[] WeatherGoodTexts =
            {
                "The sun shines brightly over the Park|It's a beautiful {{WEATHER}} day",
                "Guests are smiling everywhere|Perfect weather for a {{WEATHER}} day",
                "The Park feels alive|Enjoy the wonderful {{WEATHER}} weather",
                "A calm breeze passes through|It’s a lovely {{WEATHER}} weather today",
                "Nature is at peace|The {{WEATHER}} weather couldn’t be better",
                "Everything sparkles under the light|A perfect day of {{WEATHER}} weather",
                "Visitors are taking off their jackets|What a pleasant {{WEATHER}} weather",
                "Music and laughter fill the air|The {{WEATHER}} weather sets the perfect mood",
                "Time for a stroll in the Park|The {{WEATHER}} weather is just right",
                "Guests are relaxing happily|Enjoying the gentle {{WEATHER}} weather"
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
                "The gates swing open magically|{{AMOUNT}} Guests appear from nowhere",
                "Visitors multiply mysteriously|{{AMOUNT}} new Guests just arrived",
                "Guest chaos!|{{AMOUNT}} new arrivals are making the Park lively",
                "A sudden rush of Guests|The Park population grows by {{AMOUNT}}",
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
                "The Park just got quieter|{{AMOUNT}} Guests are no longer here",
                "Tragedy strikes!|{{AMOUNT}} Guests have disappeared mysteriously",
                "Visitors vanished suddenly|{{AMOUNT}} Guests removed from the Park",
                "A grim turn of events|{{AMOUNT}} Guests didn’t make it",
                "Park attendance drops|{{AMOUNT}} Guests are gone",
                "Something went horribly wrong|{{AMOUNT}} Guests were taken away",
                "The crowd thins unexpectedly|{{AMOUNT}} Guests are missing",
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
                "Money chaos!|{{PERCENTAGE_GUESTS}}% of Guests gain {{AMOUNT}} coins",
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
                "Money chaos!|{{PERCENTAGE_GUESTS}}% of Guests lose {{AMOUNT}} coins",
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
                "Guests are feeling hungry|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Hunger",
                "A rumbling in their stomachs|{{PERCENTAGE_GUESTS}}% of Guests are now at {{PERCENTAGE_VALUE}}% Hunger",
                "Snack time crisis!|Hunger is now {{PERCENTAGE_VALUE}}% for {{PERCENTAGE_GUESTS}}% of Guests",
                "Guests are craving food|{{PERCENTAGE_GUESTS}}% of Visitors are set to {{PERCENTAGE_VALUE}}% Hunger",
                "The Park’s smell of food backfires|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Hunger"
            };
            public static string[] GetGuestHungerText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestHungerTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestThirstTexts =
            {
                "Guests are parched|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Thirst",
                "Sudden thirst hits the Park|{{PERCENTAGE_GUESTS}}% of Guests are now at {{PERCENTAGE_VALUE}}% Thirst",
                "Visitors need a drink|{{PERCENTAGE_GUESTS}}% of Guests’ Thirst is now {{PERCENTAGE_VALUE}}%",
                "A dry spell in the Park|{{PERCENTAGE_GUESTS}}% of Guests now sit at {{PERCENTAGE_VALUE}}% Thirst",
                "Guests are complaining|{{PERCENTAGE_GUESTS}}% of Guests currently have {{PERCENTAGE_VALUE}}% Thirst"
            };
            public static string[] GetGuestThirstText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestThirstTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestBathroomTexts =
            {
                "Nature calls!|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Bathroom Urgency",
                "Guests are panicking|{{PERCENTAGE_GUESTS}}% of Guests’ Bathroom Urgency is now {{PERCENTAGE_VALUE}}%",
                "A restroom emergency!|{{PERCENTAGE_GUESTS}}% of Guests are now at {{PERCENTAGE_VALUE}}% Urgency",
                "Bathroom chaos in the Park|{{PERCENTAGE_GUESTS}}% of Guests currently have {{PERCENTAGE_VALUE}}% Urgency",
                "Guests are uncomfortable|{{PERCENTAGE_GUESTS}}% of Visitors’ Bathroom Urgency is now {{PERCENTAGE_VALUE}}%",
                "Hurry, the lines are long!|{{PERCENTAGE_GUESTS}}% of Guests have reached {{PERCENTAGE_VALUE}}% Urgency",
                "Visitors are squirming|{{PERCENTAGE_GUESTS}}% of Guests now sit at {{PERCENTAGE_VALUE}}% Urgency",
                "Restroom mayhem!|{{PERCENTAGE_GUESTS}}% of Guests’ Urgency level is now {{PERCENTAGE_VALUE}}%",
                "The Park’s hygiene alert|{{PERCENTAGE_GUESTS}}% of Guests currently have {{PERCENTAGE_VALUE}}% Bathroom Urgency",
                "Guests are in distress|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Urgency"
            };

            public static string[] GetGuestBathroomText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestBathroomTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestVomitingTexts =
            {
                "Disaster in the Park!|{{PERCENTAGE_GUESTS}}% of Guests are vomiting at {{PERCENTAGE_VALUE}}% severity",
                "Gross!|{{PERCENTAGE_GUESTS}}% of Guests are now experiencing {{PERCENTAGE_VALUE}}% Vomiting",
                "Guests are sickened|{{PERCENTAGE_GUESTS}}% of Visitors now have {{PERCENTAGE_VALUE}}% Vomiting",
                "Chaos in the queues!|{{PERCENTAGE_GUESTS}}% of Guests are currently at {{PERCENTAGE_VALUE}}% Vomiting",
                "Park hygiene crisis|{{PERCENTAGE_GUESTS}}% of Guests are vomiting with {{PERCENTAGE_VALUE}}% intensity",
                "An unpleasant turn of events|{{PERCENTAGE_GUESTS}}% of Guests now suffer {{PERCENTAGE_VALUE}}% Vomiting",
                "Guests feel ill|{{PERCENTAGE_GUESTS}}% of Visitors currently have {{PERCENTAGE_VALUE}}% Vomiting",
                "Nausea spreads through the Park|{{PERCENTAGE_GUESTS}}% of Guests are now at {{PERCENTAGE_VALUE}}% Vomiting",
                "Visitors are queasy|{{PERCENTAGE_GUESTS}}% of Guests’ Vomiting level is now {{PERCENTAGE_VALUE}}%",
                "A stomach-churning moment!|{{PERCENTAGE_GUESTS}}% of Guests are vomiting at {{PERCENTAGE_VALUE}}% intensity"
            };

            public static string[] GetGuestVomitingText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestVomitingTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestHappinessTexts =
            {
                "Guests are feeling uneasy|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Happiness",
                "A sour mood spreads|{{PERCENTAGE_GUESTS}}% of Guests’ Happiness is now {{PERCENTAGE_VALUE}}%",
                "Visitors are grumpy|{{PERCENTAGE_GUESTS}}% of Guests are now at {{PERCENTAGE_VALUE}}% Happiness",
                "Cheerfulness drops|{{PERCENTAGE_GUESTS}}% of Guests currently have {{PERCENTAGE_VALUE}}% Happiness",
                "Guests are less thrilled|{{PERCENTAGE_GUESTS}}% of Visitors’ Happiness level is {{PERCENTAGE_VALUE}}%",
                "Mood swings in the Park|{{PERCENTAGE_GUESTS}}% of Guests’ Happiness is now set to {{PERCENTAGE_VALUE}}%",
                "Visitors are less content|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Happiness",
                "A shift in feelings|{{PERCENTAGE_GUESTS}}% of Guests are currently at {{PERCENTAGE_VALUE}}% Happiness",
                "Guests react unpredictably|{{PERCENTAGE_GUESTS}}% of Visitors’ Happiness is now {{PERCENTAGE_VALUE}}%",
                "Park mood alert|{{PERCENTAGE_GUESTS}}% of Guests currently have {{PERCENTAGE_VALUE}}% Happiness"
            };

            public static string[] GetGuestHappinessText(float guests, float percentage)
            {
                string textBlock = Constants.Trap.GetRandomText(Constants.Trap.GuestHappinessTexts);
                return textBlock.Replace("{{PERCENTAGE_GUESTS}}", guests.ToString()).Replace("{{PERCENTAGE_VALUE}}", percentage.ToString()).Split(Constants.Trap.TextDivider);
            }

            public static string[] GuestTirednessTexts =
            {
                "Guests are getting exhausted|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Tiredness",
                "Visitors feel sleepy|{{PERCENTAGE_GUESTS}}% of Guests are currently at {{PERCENTAGE_VALUE}}% Tiredness",
                "Fatigue spreads through the Park|{{PERCENTAGE_GUESTS}}% of Guests’ Tiredness is now {{PERCENTAGE_VALUE}}%",
                "Guests are dragging their feet|{{PERCENTAGE_GUESTS}}% of Visitors now sit at {{PERCENTAGE_VALUE}}% Tiredness",
                "A sleepy crowd|{{PERCENTAGE_GUESTS}}% of Guests currently have {{PERCENTAGE_VALUE}}% Tiredness",
                "Visitors need rest|{{PERCENTAGE_GUESTS}}% of Guests’ Tiredness level is {{PERCENTAGE_VALUE}}%",
                "Guests are worn out|{{PERCENTAGE_GUESTS}}% of Guests now have {{PERCENTAGE_VALUE}}% Tiredness",
                "Fatigue alert!|{{PERCENTAGE_GUESTS}}% of Guests are now at {{PERCENTAGE_VALUE}}% Tiredness",
                "Guests struggle to stay awake|{{PERCENTAGE_GUESTS}}% affected, currently {{PERCENTAGE_VALUE}}% Tiredness",
                "Park visitors are exhausted|{{PERCENTAGE_GUESTS}}% of Guests’ Tiredness is now {{PERCENTAGE_VALUE}}%"
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

            // -----------------------------
            // General Traps
            // -----------------------------
            public static string[] ResearchTexts =
            {
                "The prototype looked promising until it exploded. Research for:",
                "The team requires additional time. Research for:",
                "The team is exploring new directions. Research for:",
                "Results were... unexpected. Research for:",
                "Great effort, questionable results. Research for:",
                "More testing is required after an unfortunate incident. Research for:",
                "Progress has been made, but more work remains. Research for:",
                "New concepts are under review. Research for:",
                "Turns out research is hard. Research for:",
                "We found several new problems. Research for:",
            };
            public static string GetResearchText()
            {
                return Constants.Trap.GetRandomText(Constants.Trap.ResearchTexts);
            }
        }

        public static class Skips
        {
            public static string[] Types = { "Skip" };
        }

        public static class ProgressiveSpeed
        {
            public static string[] Types = { "Progressive Speed" };
        }

        public static class Scenario
        {
            public static Dictionary<int, string> Maps = new Dictionary<int, string>
            {
                // Custom Campaign
                { 0, "Archipelago - Lakeside Gardens" },
                { 1, "Archipelago - Dusty Ridge Ranch" },
                { 2, "Archipelago - The Broken Atoll" },
                { 3, "Archipelago - Magma Falls" },

                // Main Campaign
                { 100, "Maple Meadows" },
                { 101, "Chanute Airfield" },
                { 102, "Victoria Lake" },
                { 103, "Western Roundup" },
                { 104, "Coral Caldera" },
                { 105, "Mystic Oasis" },
                { 106, "Nova Labs" },
                { 107, "Archipelago Adventures" },
                { 108, "Adventure Island" },
                { 109, "Batavia Cay" },
                { 110, "Ice-Shelf Islands" },
                { 111, "HappyCo Harbor" },
                { 112, "Biscayne Beach" },
                { 113, "Highway Hijinks" },
                { 114, "Honey Hills" },
                { 115, "Orchard Acres" },
                { 116, "Coaster Canyon" },
                { 117, "Hickory Hill" },
                { 118, "Pagoda Valley" },
                { 119, "Kaiserberg" },
                { 120, "Sakura Gardens" },
                { 121, "Silica Slopes" },
                { 122, "Disaster Peaks" },
                { 123, "Robopark" },
                { 124, "Sheer Cliffs" },
                { 125, "Zalgonia" },
                { 126, "HappyCo. Bakery" },

                // Taste of Adventure Campaign
                { 200, "Yucatán Ridge" },
                { 201, "Brimstone Peak" },
                { 202, "Candyland" },
                { 203, "Timber Creek" },
                { 204, "Jungle Adventure" },
                { 205, "Technopolis" },
                { 206, "Dragon Valley" },
                { 207, "Victoria Island" },
                { 208, "Celeste Mountain" },
                { 209, "The Moon" },
            };

            public static ArchipelagoMod.Src.Scenario[] MainCampaignScenarios =
            {
                // Main Campaign
                new ArchipelagoMod.Src.Scenario("Maple Meadows", "5300258a-eb65-4a6a-b1fe-3978bdfcaad5"),
                new ArchipelagoMod.Src.Scenario("Chanute Airfield", "0584c095-4f88-4d66-a242-a4a8102cf68b"),
                new ArchipelagoMod.Src.Scenario("Victoria Lake", "0d71680c-3aee-4939-a9fd-07abbea4745e"),
                new ArchipelagoMod.Src.Scenario("Western Roundup", "5f43f0ca-9a18-4c07-9c9f-beccc592678a"),
                new ArchipelagoMod.Src.Scenario("Coral Caldera", "d37f6097-24e3-4c5a-973f-78bbbeaa7f7b"),
                new ArchipelagoMod.Src.Scenario("Mystic Oasis", "2af10b6b-a89e-45fa-bd2d-ab06b3797c68"),
                new ArchipelagoMod.Src.Scenario("Nova Labs", "e522fd03-6373-4458-aaef-b9c32689e639"),
                new ArchipelagoMod.Src.Scenario("Archipelago Adventures", "5134d65c-5805-4c49-9a61-856116a0dcfa"),
                new ArchipelagoMod.Src.Scenario("Adventure Island", "37fe35a8-24f0-4f19-91af-cf4dc169c23a"),
                new ArchipelagoMod.Src.Scenario("Batavia Cay", "b89d25a5-5d87-4ed5-814e-1ff36d1dbce7"),
                new ArchipelagoMod.Src.Scenario("Ice-Shelf Islands", "b5fc824b-9d50-49fc-aa47-410b103d7be0"),
                new ArchipelagoMod.Src.Scenario("HappyCo Harbor", "b4de5b28-1b0f-451c-9b18-df6556252d3c"),
                new ArchipelagoMod.Src.Scenario("Biscayne Beach", "42c2cb55-5f16-426c-8479-5cc1ccbe886f"),
                new ArchipelagoMod.Src.Scenario("Highway Hijinks", "f92adf70-5e8b-49d0-99e5-69980ac13597"),
                new ArchipelagoMod.Src.Scenario("Honey Hills", "b1fd47c0-4f79-4913-8355-9c6d02e288ac"),
                new ArchipelagoMod.Src.Scenario("Orchard Acres", "9d4f0810-679a-4b37-9924-9ca95afeefc2"),
                new ArchipelagoMod.Src.Scenario("Coaster Canyon", "1a92021c-b956-4213-a81a-0f6664456f53"),
                new ArchipelagoMod.Src.Scenario("Hickory Hill", "afb627b5-2c63-49bf-a408-bfcc17b726fb"),
                new ArchipelagoMod.Src.Scenario("Pagoda Valley", "3e9de9aa-403f-4aed-a81e-1a95b1538ac9"),
                new ArchipelagoMod.Src.Scenario("Kaiserberg", "aa73bd39-cd49-42f2-8fec-25697cf29839"),
                new ArchipelagoMod.Src.Scenario("Sakura Gardens", "3fb26b42-9ef7-455c-a330-37ef45a8f4ca"),
                new ArchipelagoMod.Src.Scenario("Silica Slopes", "b1d26e1b-ec06-49d6-82b8-b04baaa048ba"),
                new ArchipelagoMod.Src.Scenario("Disaster Peaks", "5de1e2b1-0370-4bbe-a360-e3640e175f9a"),
                new ArchipelagoMod.Src.Scenario("Robopark", "fb38cc6d-1fce-4ad3-acdd-49743bdc487e"),
                new ArchipelagoMod.Src.Scenario("Sheer Cliffs", "da6fbfe3-5bc7-4ba4-8309-58037ae4ac12"),
                new ArchipelagoMod.Src.Scenario("Zalgonia", "1d849f2e-196d-4700-8a27-ea138741260d"),
            };

            public static ArchipelagoMod.Src.Scenario[] MainBonusCampaignScenarios =
            {
                // Main Bonus Campaign
                new ArchipelagoMod.Src.Scenario("HappyCo. Bakery", "243568cd-0c0d-49e9-9b95-78c598cca161"),
            };

            public static ArchipelagoMod.Src.Scenario[] DLC1CampaignScenarios =
            {
                // Taste of Adventure Campaign
                new ArchipelagoMod.Src.Scenario("Yucatán Ridge", "1ad9669e-ced9-433e-8345-049bfb8948b7"),
                new ArchipelagoMod.Src.Scenario("Brimstone Peak", "dcc04848-ee5c-4f7e-95ac-f63291c59f00"),
                new ArchipelagoMod.Src.Scenario("Candyland", "d17a72ba-f1ea-4b9a-8429-f1ceaa2598cd"),
                new ArchipelagoMod.Src.Scenario("Timber Creek", "26ea68a8-d6f2-4a09-a7bd-ea2cafcfd27f"),
                new ArchipelagoMod.Src.Scenario("Jungle Adventure", "58ebf9de-eb16-4126-b242-45b68282bcab"),
                new ArchipelagoMod.Src.Scenario("Technopolis", "6dd92d76-3682-4b0f-bec7-40618d6b6250"),
                new ArchipelagoMod.Src.Scenario("Dragon Valley", "6666e9e8-b11e-4976-867a-d018acbd2234"),
                new ArchipelagoMod.Src.Scenario("Victoria Island", "973ab00e-1b03-47f4-b3e7-b694d993ebfa"),
                new ArchipelagoMod.Src.Scenario("Celeste Mountain", "55346780-e09a-469a-bea7-de72aef0969b"),
                new ArchipelagoMod.Src.Scenario("The Moon", "dbff47f8-996c-4bca-a00a-a4e280562e56"),
            };
        }

        public static string[] AllItems = (new[]
        {
            Constants.Attraction.All,
            Constants.Stall.All,
            Constants.Mods.All,
        })
            .SelectMany(a => a).ToArray();

        public static class Mods
        {
            public static string[] Food =
            {
                "Dragon Shop",
                "Taco Shop",
                "Pancake Shop",
            };
            public static string[] Drinks = {};
            public static string[] Facilities = {};
            public static string[] CalmRides =
            {
                "Hopper",
                "Rockin' Tug",
                "Circus Show",
            };
            public static string[] ThrillRides =
            {
                "Power Swing",
                "Mega Swing",
                "Kraken Attack",
                "Hexentanz",
                "RotoShake",
                "Demon Drop",
                "Fish In A Barrel",
                "Jump²",
                "Inverter",
                "Somersault",
                "Monster",
                "Revolution",
            };
            public static string[] CoasterRides =
            {
                "Corkscrew Coaster",
                "Inverted Launch Coaster",
                "Quadruple Rail Coaster",
                "Retro Steel Coaster",
            };
            public static string[] TransportRides =
            {};
            public static string[] WaterRides =
            {};

            public static string[] Stalls = (new[]
            {
                Constants.Mods.Food,
                Constants.Mods.Drinks,
                Constants.Mods.Facilities,
            })
                .SelectMany(a => a).ToArray();

            public static string[] Attractions = (new[]
            {
                Constants.Mods.CalmRides,
                Constants.Mods.ThrillRides,
                Constants.Mods.CoasterRides,
                Constants.Mods.TransportRides,
                Constants.Mods.WaterRides
            })
                .SelectMany(a => a).ToArray();

            public static string[] All = (new[]
            {
                Constants.Mods.Stalls,
                Constants.Mods.Attractions,
            })
                .SelectMany(a => a).ToArray();

            public static string GetType(string thing)
            {
                if (Constants.Mods.CalmRides.Contains(thing))
                {
                    return Constants.Attraction.Types[0];
                }

                if (Constants.Mods.ThrillRides.Contains(thing))
                {
                    return Constants.Attraction.Types[1];
                }

                if (Constants.Mods.CoasterRides.Contains(thing))
                {
                    return Constants.Attraction.Types[2];
                }

                if (Constants.Mods.TransportRides.Contains(thing))
                {
                    return Constants.Attraction.Types[3];
                }

                if (Constants.Mods.WaterRides.Contains(thing))
                {
                    return Constants.Attraction.Types[4];
                }

                // Shops
                if (Constants.Mods.Drinks.Contains(thing))
                {
                    return Constants.Stall.Types[0];
                }

                if (Constants.Mods.Food.Contains(thing))
                {
                    return Constants.Stall.Types[1];
                }

                if (Constants.Mods.Facilities.Contains(thing))
                {
                    return Constants.Stall.Types[2];
                }

                return "unknown";
            }
        }

        public static class Research
        {
            public static int MaxAttractions = 4;
            public static int MaxShops = 3;
            public static int MaxDeco = 1;

            public static string[] Types =
            {
                "Attraction",
                "Shop",
                "Decorations"
            };

            public static class Rules
            {
                public static string[] Decorations =
                {
                    "Adventure",
                    "Ancient World",
                    "Candyland",
                    "Dino",
                    "Effects",
                    "Fantasy",
                    "Industrial Structures",
                    "Medieval Props",
                    "Medieval Structures",
                    "Race Props",
                    "Sci-Fi Props",
                    "Sci-Fi Structures",
                    "Sculptures and Statues",
                    "Spooky Props",
                    "Spooky Structures",
                    "Steam Pipes",
                    "Steamworks Props",
                    "Topiaries",
                    "Western Props",
                };

                public static string[] Statistics = Constants.Statistics.map.Keys.ToArray();


                public static string[] All = (new[]
                {
                    Constants.Research.Rules.Decorations,
                    Constants.Research.Rules.Statistics,
                })
                    .SelectMany(a => a)
                    .ToArray();
            }
        }

        public static string[] AllNonItemTypes = (new[]
        {
            Constants.Trap.All,
            Constants.Attraction.Types,
            Constants.Stall.Types,
            Constants.Skips.Types,
            Constants.ProgressiveSpeed.Types,
            Constants.Decorations.All,
            Constants.Statistics.All,
            Constants.Research.Rules.All,
        })
            .SelectMany(a => a).ToArray();

        public class EnergyLink
        {
            public static int Divider = 50000000;
            public static float MinDepositMoney= 100f;
        }
    }
}
