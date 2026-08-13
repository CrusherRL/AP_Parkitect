using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Dispatcher;
using ArchipelagoMod.Src.SlotData;
using Parkitect.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;
using static ArchipelagoMod.Src.Constants;

namespace ArchipelagoMod.Src.Controller
{
    class ParkitectController : MainThreadDispatcher
    {
        // Timescale for Speedups
        readonly float OldTimeScale = Time.timeScale;
        public AP_Rules AP_Rules = null;
        public SaveData SaveData = null;
        private float SuppressMessagesUntilTime = 0f;

        void Start()
        {
            Helper.Debug($"[ParkitectController::Start]");
            this.SaveData = GetComponent<SaveData>();
            Helper.Debug($"[ParkitectController::Start] Booted");
        }

        // -----------------------------
        // Player options
        // -----------------------------

        // Speed up gameplay
        public void PlayerRaiseSpeed (int speed)
        {
            if (!Constants.Player.SpeedupOptions.Contains(speed))
            {
                return;
            }

            Time.timeScale = speed;
            EventManager.Instance.RaiseOnTimeSpeedChanged(this.OldTimeScale, speed, true);
        }

        // Receive a small amount of money
        public void PlayerAddMoney(float money = 500f)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                if (!Constants.Player.MoneyOptions.Contains(money))
                {
                    return;
                }
                GameController.Instance.park.parkInfo.moneyTransaction(money, MonthlyTransactions.Transaction.REWARD);
            });
        }
        public void PlayerAddMoney(float money = 500f, bool force = false)
        {
            if (!force)
            {
                this.PlayerAddMoney(money);
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                Helper.Debug(money.ToString());
                GameController.Instance.park.parkInfo.moneyTransaction(money, MonthlyTransactions.Transaction.REWARD);
            });
        }

        public void PlayerRemoveMoney(double money)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                GameController.Instance.park.parkInfo.setMoney(this.GetPlayerMoney() - money);
            });
        }

        // Player has savegame?
        public bool PlayerHasSavegame()
        {
            return GameController.Instance.loadedSavegamePath != null;
        }

        public bool PlayerIsInPark(string parkName)
        {
            return GameController.Instance.park.parkName == parkName;
        }

        public bool PlayerIsInCorrectMode()
        {
            return GameController.Instance.isInNormalMode && !GameController.Instance.park.settings.isSandboxMode;
        }

        // -----------------------------
        // Guests options
        // -----------------------------

        // Spawns new Guests
        public void PlayerAddGuests (int guests = 25)
        {
            if (!Constants.Guest.SpawnOptions.Contains(guests))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                for (int i = 0; i < guests; i += 1)
                {
                    GameController.Instance.park.spawnGuest();
                }
            });
        }

        public void PlayerAddMyGuests()
        {
            ParkitectGuests.CreateAll();
        }

        public void PlayerAddGuestInventory(List<Guest> guests, Item voucher)
        {
            List<List<Guest>> chunkList = Helper.Chunk(guests);
            float nextProcessTime = Time.time;

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.addToInventory(voucher);
                    }
                });
            }
        }

        // Adds/Substracts Guests money
        public void PlayerChangeGuestsMoney (float money = 30f, float guests = 25f, string sign = "+")
        {
            if (!Constants.Guest.MoneyOptions.Contains(money) || guests < 1)
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        if (sign == "+")
                        {
                            guest.Money += money;
                        }
                        else
                        {
                            guest.Money -= money;
                        }
                    }
                });
            }
        }

        // Kills specific guests
        public void PlayerKillGuests (int guests = 50)
        {
            if (!Constants.Guest.KillOptions.Contains(guests))
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(0, guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.Kill();
                    }
                });
            }
        }

        // Set amount of guests hungry
        public void PlayerSetGuestsHungry(float guests = 15f, float hunger = 80f)
        {
            if (!Constants.Guest.HungryOptions.Contains(guests) || !Constants.Guest.HungryPercentage.Contains(hunger))
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests( guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.Hunger = hunger;
                    }
                });
            }
        }

        // Set amount of guests thirsty
        public void PlayerSetGuestsThirsty (float guests = 15f, float thirst = 80f)
        {
            if (!Constants.Guest.ThirstyOptions.Contains(guests) || !Constants.Guest.ThirstyPercentage.Contains(thirst))
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.Thirst = thirst;
                    }
                });
            }
        }

        // set amount of guests to the bathroom
        public void PlayerSetGuestsToBathroom (float guests = 80f, float bathroom = 80f)
        {
            if (!Constants.Guest.BathroomOptions.Contains(guests) || !Constants.Guest.BathroomPercentage.Contains(bathroom))
            {
                return;
            }
            
            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.ToiletUrgency = bathroom;
                    }
                });
            }
        }

        // Set amount of guests to vomit
        public void PlayerSetGuestsToVomit (float guests = 20f, float vomit = 60f)
        {
            if (!Constants.Guest.VomitOptions.Contains(guests) || !Constants.Guest.VomitPercentage.Contains(vomit))
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.Nausea = vomit;
                    }
                });
            }
        }

        // Set amount of guests happy
        public void PlayerSetGuestsHappy (float guests = 60f, float happiness = 80f)
        {
            if (!Constants.Guest.HappinessOptions.Contains(guests) || !Constants.Guest.HappinessPercentage.Contains(happiness))
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.Happiness = happiness;
                    }
                });
            }
        }

        // Set amount of guests tired
        public void PlayerSetGuestsTired(float guests = 60f, float tiredness = 80f)
        {
            if (!Constants.Guest.TirednessOptions.Contains(guests) || !Constants.Guest.TirednessPercentage.Contains(tiredness))
            {
                return;
            }

            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(guests));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.Tiredness = tiredness;
                    }
                });
            }
        }

        // Set amount of guests tired
        public void PlayerSetGuestsAsVandals((int Start, int End) range)
        {
            if (!Helper.IsRange(Constants.Guest.VandalsOptions, range))
            {
                return;
            }

            this.PlayerSetGuestsAsVandals(Randomizer.GetRandomInt(range));
        }
        public void PlayerSetGuestsAsVandals(int amount)
        {
            List<List<Guest>> chunkList = Helper.Chunk(Randomizer.GetRandomGuests(0f, amount));

            foreach (List<Guest> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Guest guest in chunks)
                    {
                        guest.setIsVandal(true);
                    }
                });
            }
        }

        // Create a Attraction Voucher and assigning the attraction to it
        public AttractionVoucher PlayerCreateAttractionVoucher(Attraction attraction)
        {
            AttractionVoucher voucher = ScriptableSingleton<AssetManager>.Instance.instantiatePrefab<AttractionVoucher>(Prefabs.AttractionVoucher);
            voucher.attraction = attraction;

            return voucher;
        }

        // Create a ProductShop Voucher and assigning the shop to it
        public ShopVoucher PlayerCreateShopVoucher(ProductShop shop)
        {
            ShopVoucher voucher = ScriptableSingleton<AssetManager>.Instance.instantiatePrefab<ShopVoucher>(Prefabs.ShopVoucher);
            voucher.productShopReferenceName = shop.getReferenceName();

            return voucher;
        }

        // -----------------------------
        // Employee options
        // -----------------------------

        // Hire new Employees
        public void PlayerHireEmployees(Prefabs employee, (int Start, int End) range, int customeIndex = 0)
        {
            if (!Helper.IsRange(Constants.Employee.SpawnRanges, range) || !Constants.Employee.Options.Contains(employee))
            {
                return;
            }

            this.PlayerHireEmployees(employee, Randomizer.GetRandomInt(range), customeIndex);
        }
        public void PlayerHireEmployees(Prefabs employee, int amount, int customeIndex = 0)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                for (int i = 0; i < amount; i += 1)
                {
                    Employee e = (Employee)GameController.Instance.park.spawnUnInitializedPerson(employee);
                    e.applyCostume(e.costumes[customeIndex]);
                    e.Initialize();
                    EventManager.Instance.RaiseOnEmployeeHired(e);
                }
            });
        }

        // Set Employees Tired
        public void PlayerSetEmployeesTired(List<Employee> employees, float number)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Employee employee in employees)
                {
                    employee.Tiredness = number;
                }
            });
        }
        public void PlayerSetEmployeesTired(List<Employee> employees)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Employee employee in employees)
                {
                    (int Start, int End) range = Constants.Employee.TirednessRanges[0];
                    employee.Tiredness = Randomizer.GetRandomFloat(range.Start, range.End);
                }
            });
        }

        public void PlayerSetEmployeesTraining(List<Employee> employees)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Employee employee in employees)
                {
                    if (!employee.isTraining && employee.getPrefabType() != Prefabs.Shopkeeper && employee.getPrefabType() != Prefabs.RideOperator)
                    {
                        employee.setIsTraining(true);
                    }
                }
            });
        }

        // -----------------------------
        // Weather options
        // -----------------------------

        // Change Weather
        public void PlayerChangeWeather (Constants.Weather.Options weatherOption)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                if (weatherOption == Constants.Weather.Options.RAINY)
                {
                    GameController.Instance.park.weatherController.setWeatherRainy();
                }
                else if (weatherOption == Constants.Weather.Options.STORMY)
                {
                    GameController.Instance.park.weatherController.setWeatherStormy();
                }
                else if (weatherOption == Constants.Weather.Options.CLOUDY)
                {
                    GameController.Instance.park.weatherController.setWeatherCloudy();
                }
                else if (weatherOption == Constants.Weather.Options.SUNNY)
                {
                    GameController.Instance.park.weatherController.setWeatherSunny();
                }
            });
        }

        // -----------------------------
        // Attraction options
        // -----------------------------

        // Break down attraction with random reason
        public void PlayerBreakAttractions (List<Attraction> attractions)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                if (attractions.Count > 0)
                {
                    foreach (Attraction attraction in attractions)
                    {
                    
                            attraction.setBroken(Randomizer.GetRandomBreakReason());
                    }
                }
            });
        }

        public void PlayerUnlockItem(AP_Item AP_Item)
        {
            if (AP_Item.IsMod)
            {
                this.SaveData.AddUnlockedItem(AP_Item.Name);

                if (Constants.Mods.Stalls.Contains(AP_Item.Name))
                {
                    this.PlayerAddStall(AP_Item.Name);
                    return;
                }

                this.PlayerAddAttraction(AP_Item.Name);
                return;
            }

            if (Constants.Stall.All.Contains(AP_Item.Name))
            {
                this.PlayerAddStall(AP_Item.PrefabName);
                this.SaveData.AddUnlockedItem(AP_Item.PrefabName);
                return;
            }
            
            if (Constants.Attraction.All.Contains(AP_Item.Name))
            {
                this.PlayerAddAttraction(AP_Item.PrefabName);
                this.SaveData.AddUnlockedItem(AP_Item.PrefabName);
                return;
            }

            if (Constants.UtilityBuilding.All.Contains(AP_Item.Name))
            {
                this.PlayerAddUtilityBuilding(AP_Item.PrefabName);
                this.SaveData.AddUnlockedItem(AP_Item.PrefabName);
                return;
            }

            if (Constants.Decorations.All.Contains(AP_Item.Name))
            {
                ThemeContainer tc = this.FindThemeContainer(AP_Item.Name);
                this.PlayerAddDecorations(tc);
                this.SaveData.AddUnlockedItem(AP_Item.Name);
                return;
            }

            if (Constants.Statistics.All.Contains(AP_Item.Name))
            {
                string referenceName = Constants.Statistics.map.FirstOrDefault(x => x.Value == AP_Item.Name).Key;
                this.ResearchRuleUpdateCanUnlock(referenceName);
                this.SaveData.AddUnlockedItem(referenceName);
                return;
            }
        }

        public bool PlayerHasUnlockedItem(AP_Item AP_Item)
        {
            if (!AP_Item.IsMod && !AP_Item.IsDeco && !AP_Item.IsStatistic)
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] AP_Item --- PrefabName -> {AP_Item.PrefabName}");

                if (this.SaveData.HasUnlockedItem(AP_Item.PrefabName))
                {
                    Helper.Debug("[ParkitectController::PlayerHasUnlockedItem] AP_Item exists");
                    return true;
                }
            } else
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] AP_Item --- Name -> {AP_Item.Name}");
            }

            if (this.SaveData.HasUnlockedItem(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] AP_Item exists");
                return true;
            }


            if (AP_Item.IsMod)
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] is Mod::{AP_Item.ModType}");

                if (Constants.Mods.Stalls.Contains(AP_Item.Name))
                {
                    return this.GetShopFromAssetManager(AP_Item.Name).isAvailableInParks;
                }

                return this.GetAttractionFromAssetManager(AP_Item.Name).isAvailableInParks;
            }

            if (Constants.Stall.All.Contains(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] is Stall");
                return this.GetAllShopsFromAssetManager(AP_Item.PrefabName).First().isAvailableInParks;
            }

            if (Constants.Attraction.All.Contains(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] is Attraction");
                return this.GetAllAttractionsFromAssetManager(AP_Item.PrefabName).First().isAvailableInParks;
            }

            if (Constants.UtilityBuilding.All.Contains(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] is Utility Building");
                return this.GetAllUtilityBuildingsFromAssetManager(AP_Item.PrefabName).First().isAvailableInParks;
            }

            if (Constants.Decorations.All.Contains(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] is Decoration");
                ThemeContainer tc = this.FindThemeContainer(AP_Item.Name);
                return this.GetAllDecorationsFromAssetManager(tc).First().isAvailableInParks;
            }

            if (Constants.Statistics.All.Contains(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerHasUnlockedItem] is Statistic");
                string referenceName = Constants.Statistics.map.FirstOrDefault(x => x.Value == AP_Item.Name).Key;
                return this.CanUnlockedResearchRule(referenceName);
            }

            return false;
        }

        public void PlayerRemoveAllRides()
        {
            List<List<Attraction>> chunkList = Helper.Chunk(this.GetAllAttractionsFromAssetManager());

            foreach (List<Attraction> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Attraction attraction in chunks)
                    {
                        attraction.isAvailableInParks = false;
                    }
                });
            }
        }
        public void PlayerAddAllRides()
        {
            List<Attraction> attractions = this.GetAllAttractionsFromAssetManager();

            foreach (Attraction attraction in attractions)
            {
                this.PlayerAddAttraction(attraction);
            }
        }

        public void PlayerAddAttraction(Prefabs prefab)
        {
            Attraction attraction = this.GetAllAttractionsFromAssetManager(prefab).First();
            this.PlayerAddAttraction(attraction);
        }
        public void PlayerAddAttraction(string prefabName)
        {
            Attraction attraction = this.GetAllAttractionsFromAssetManager(prefabName).FirstOrDefault();

            if (attraction == null)
            {
                attraction = this.GetAttractionFromAssetManager(prefabName);
            }

            this.PlayerAddAttraction(attraction);
        }
        public void PlayerAddAttraction(Attraction attraction)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                attraction.isAvailableInParks = true;
            });
        }

        // -----------------------------
        // Stall options
        // -----------------------------

        public void PlayerRemoveAllStalls()
        {
            List<List<Shop>> chunkList = Helper.Chunk(this.GetAllShopsFromAssetManager());

            foreach (List<Shop> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (Shop shop in chunks)
                    {
                        shop.isAvailableInParks = false;
                    }
                });
            }
        }

        public void PlayerAddAllStalls()
        {
            List<Shop> shops = this.GetAllShopsFromAssetManager();

            foreach (Shop shop in shops)
            {
                this.PlayerAddStall(shop);
            }
        }

        public void PlayerAddStall(Prefabs prefab)
        {
            Shop shop = this.GetAllShopsFromAssetManager(prefab).First();
            this.PlayerAddStall(shop);
        }

        public void PlayerAddStall(string prefabName)
        {
            Shop shop = this.GetAllShopsFromAssetManager(prefabName).FirstOrDefault();

            if (shop == null)
            {
                shop = this.GetShopFromAssetManager(prefabName);
            }
            this.PlayerAddStall(shop);
        }

        public void PlayerAddStall(Shop shop)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                shop.isAvailableInParks = true;
            });
        }

        // -----------------------------
        // UtilityBuilding options
        // -----------------------------
        public void PlayerAddAllUtilityBuildings()
        {
            List<UtilityBuilding> utilityBuildings = this.GetAllUtilityBuildingsFromAssetManager();

            foreach (UtilityBuilding utilityBuilding in utilityBuildings)
            {
                this.PlayerAddUtilityBuilding(utilityBuilding);
            }
        }
        public void PlayerAddUtilityBuilding(UtilityBuilding utilityBuilding)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                utilityBuilding.isAvailableInParks = true;
            });
        }
        public void PlayerAddUtilityBuilding(Prefabs prefabs)
        {
            UtilityBuilding utilityBuilding = this.GetAllUtilityBuildingsFromAssetManager(prefabs).FirstOrDefault();
            this.PlayerAddUtilityBuilding(utilityBuilding);
        }
        public void PlayerAddUtilityBuilding(string prefabName)
        {
            UtilityBuilding utilityBuilding = this.GetAllUtilityBuildingsFromAssetManager(prefabName).FirstOrDefault();
            this.PlayerAddUtilityBuilding(utilityBuilding);
        }
        public void PlayerRemoveAllUtilityBuildings()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (UtilityBuilding utilityBuilding in this.GetAllUtilityBuildingsFromAssetManager())
                {
                    utilityBuilding.isAvailableInParks = false;
                }
            });
        }

        // -----------------------------
        // Decorations options
        // -----------------------------

        public void PlayerAddDecorations(ThemeContainer themeContainer)
        {
            List<List<BuildableObject>> chunkList = Helper.Chunk(this.GetAllDecorationsFromAssetManager(themeContainer));

            foreach (List<BuildableObject> chunks in chunkList)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (BuildableObject item in chunks)
                    {
                        item.isAvailableInParks = true;
                    }
                });
            }
        }
        public void PlayerAddDecorations(string themeTag)
        {
            ThemeContainer themeContainer = this.FindThemeContainer(themeTag);
            this.PlayerAddDecorations(themeContainer);
        }
        public void PlayerRemoveAllDecorations()
        {
            List<List<BuildableObject>> chunks = Helper.Chunk(this.GetAllDecorationsFromAssetManager());
     
            foreach (List<BuildableObject> chunk in chunks)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    foreach (BuildableObject deco in chunk)
                    {
                        deco.isAvailableInParks = false;
                    }
                });
            }
        }

        // -----------------------------
        // Statistics options
        // -----------------------------

        public void PlayerAddStatistics(string statistics)
        {
            if (!Constants.Research.Rules.Statistics.Contains(statistics))
            {
                return;
            }
            this.ResearchRuleUpdateIsUnlocked(statistics, true);
            this.ResearchRuleUpdateCanUnlock(statistics, true);
        }

        public void PlayerRemoveStatistics()
        {
            foreach (string statistics in Constants.Research.Rules.Statistics)
            {
                this.ResearchRuleUpdateIsUnlocked(statistics, false);
                this.ResearchRuleUpdateCanUnlock(statistics, false);
            }
        }
        // -----------------------------
        // Shops options
        // -----------------------------

        public void PlayerSetReDeliveryForProductShops (List<ProductShop> productShops)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (ProductShop productShop in productShops)
                {
                    foreach (Product product in productShop.selectedProducts)
                    {
                        foreach (Ingredient ingredient in product.ingredients)
                        {
                            productShop.stock.modify(ingredient.resource, productShop.stock.getAmount(ingredient.resource) * -1);
                        }
                    }

                    productShop.checkOrderIngredients();
                }
            });
        }

        public void PlayerSetCleanShopJob(List<ProductShop> productShops)
        {
            if (productShops.Count <= 0)
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (ProductShop productShop in productShops)
                {
                    productShop.triggerClean();
                }
            });
        }

        // -----------------------------
        // Scenario options
        // -----------------------------

        // Add Park goal
        public void PlayerAddScenarioGoal (IScenarioGoal goal, List<IScenarioGoalReward> rewards = null, bool optional = false)
        {
            if (rewards != null && rewards.Count > 0)
            {
                foreach (IScenarioGoalReward reward in rewards)
                {
                    goal.addReward(reward);
                }
            }

            goal.isOptional = optional;

            MainThreadDispatcher.Enqueue(() =>
            {
                GameController.Instance.park.scenario.goals.addGoal(goal);
            });
        }

        public bool PlayerHasScenarioGoal (IScenarioGoal goal)
        {
            ReadOnlyCollection<IScenarioGoal> goals = GameController.Instance.park.scenario.goals.getGoals();

            return goals.Contains(goal);
        }

        // -----------------------------
        // Research options
        // -----------------------------

        public void PlayerAddToResearch(Attraction attraction)
        {
            this.ResearchRuleUpdateIsUnlocked(attraction.getResearchReferenceName());
            this.ResearchRuleUpdateCanUnlock(attraction.getResearchReferenceName());
        }
        public void PlayerAddToResearch(Shop shop)
        {
            this.ResearchRuleUpdateIsUnlocked(shop.getResearchReferenceName());
            this.ResearchRuleUpdateCanUnlock(shop.getResearchReferenceName());
        }
        public void PlayerAddToResearch(string thing)
        {
            if (!Constants.Research.Rules.Decorations.Contains(thing))
            {
                Helper.Debug($"[ParkitectController::PlayerAddToResearch] research Rule not found! {thing}");
                return;
            }
            this.ResearchRuleUpdateIsUnlocked(thing);
            this.ResearchRuleUpdateCanUnlock(thing);
        }

        public void PlayerRedeemTrap(AP_Item AP_Item)
        {
            if (AP_Item.Name == "Attraction Breakdown Trap")
            {
                float number = Randomizer.GetRandomFloat();
                List<Attraction> attractions = Randomizer.GetRandomAttractionFromPark(number);
                // well the game will say that anyway! not sure if we should do that extra
                //string[] message = Constants.Trap.GetAttractionBreakdownText(attractions.Select(a => a.getName()).ToArray());
                this.PlayerBreakAttractions(attractions);
                return;
            }
            if (AP_Item.Name == "Attraction Voucher Trap")
            {
                float number = Randomizer.GetRandomFloat();
                Attraction attraction = Randomizer.GetRandomAttractionFromPark(100f).FirstOrDefault();
                List<Guest> guests = Randomizer.GetRandomGuests(number);

                if (attraction == null)
                {
                    this.SendMessage($"{AP_Item.Name} not redeemed. No Attraction found", canBeSuppressed: true);
                    return;
                }

                this.PlayerAddGuestInventory(guests, this.PlayerCreateAttractionVoucher(attraction));

                string[] messages = Constants.Trap.GetAttractionVoucherText(attraction.getName(), number);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }

            if (AP_Item.Name == "Shop Ingredients Trap")
            {
                List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(Randomizer.GetRandomFloat());
                this.PlayerSetReDeliveryForProductShops(productShops);

                string[] messages = Constants.Trap.GetShopIngredientsText(productShops.Select(shop => shop.getName()).ToArray());
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Shop Cleaning Trap")
            {
                List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(Randomizer.GetRandomFloat());
                this.PlayerSetCleanShopJob(productShops);

                string[] messages = Constants.Trap.GetShopCleaningText(productShops.Select(shop => shop.getName()).ToArray());
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Shop Voucher Trap")
            {
                float number = Randomizer.GetRandomFloat();
                ProductShop shop = Randomizer.GetRandomProductShopsFromPark(100f).FirstOrDefault();
                List<Guest> guests = Randomizer.GetRandomGuests(number);

                if (shop == null)
                {
                    this.SendMessage($"{AP_Item.Name} not redeemed. No Shop found", canBeSuppressed: true);
                    return;
                }

                this.PlayerAddGuestInventory(guests, this.PlayerCreateShopVoucher(shop));

                string[] messages = Constants.Trap.GetShopVoucherText(shop.getName(), number);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }

            if (AP_Item.Name == "Employee Hiring Trap")
            {
                Prefabs employee = Randomizer.GetRandomEmployee();
                int amount = Randomizer.GetRandomInt(Constants.Employee.SpawnRanges[this.AP_Rules.difficulty]);
                this.PlayerHireEmployees(employee, amount);

                string[] messages = Constants.Trap.GetEmployeeHiringText(employee.ToString(), amount);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Employee Training Trap")
            {
                (int Start, int End) range = Constants.Employee.TrainingRanges[this.AP_Rules.difficulty];
                int number = Randomizer.GetRandomInt(range);
                this.PlayerSetEmployeesTraining(this.GetParkEmployees().Take(number).ToList());

                string[] messages = Constants.Trap.GetEmployeeTrainingText(number.ToString());
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Employee Tiredness Trap")
            {
                (int Start, int End) range = Constants.Employee.TirednessRanges[this.AP_Rules.difficulty];
                float number = Randomizer.GetRandomFloat(range);
                this.PlayerSetEmployeesTired(this.GetParkEmployees().Take((int)number).ToList(), number);

                string[] messages = Constants.Trap.GetEmployeeTirednessText(number.ToString());
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }

            if (AP_Item.Name == "Player Money Trap")
            {
                float money = Randomizer.GetRandomOption(Constants.Player.MoneyOptions);
                this.PlayerAddMoney(money);

                string[] messages = Constants.Trap.GetPlayerMoneyText(money);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }

            if (AP_Item.Name == "Weather Rainy Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.RAINY);

                string[] messages = Constants.Trap.GetWeatherText("Rainy");
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Weather Stormy Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.STORMY);

                string[] messages = Constants.Trap.GetWeatherText("Stormy");
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Weather Cloudy Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.CLOUDY);

                string[] messages = Constants.Trap.GetWeatherText("Cloudy");
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Weather Sunny Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.SUNNY);

                string[] messages = Constants.Trap.GetWeatherText("Sunny");
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }

            if (AP_Item.Name == "Guest Spawn Trap")
            {
                int amount = Randomizer.GetRandomOption(Constants.Guest.SpawnOptions);
                this.PlayerAddGuests(amount);

                string[] messages = Constants.Trap.GetGuestSpawnText(amount);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Kill Trap")
            {
                int amount = Randomizer.GetRandomOption(Constants.Guest.KillOptions);
                this.PlayerKillGuests(amount);

                string[] messages = Constants.Trap.GetGuestKillText(amount);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Money Trap")
            {
                string sign = AP_Rules.GetGuestsMoneyFluxSign();
                float money = Randomizer.GetRandomOption(Constants.Guest.MoneyOptions);
                float guests = Randomizer.GetRandomOption(Constants.Guest.MoneyOptions);
                this.PlayerChangeGuestsMoney(money, guests, sign);

                string[] messages = Constants.Trap.GetGuestMoneyText(money, guests, sign);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Kill Trap")
            {
                int amount = Randomizer.GetRandomOption(Constants.Guest.KillOptions);
                this.PlayerKillGuests(amount);

                string[] messages = Constants.Trap.GetGuestKillText(amount);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Hunger Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.HungryOptions);
                float hunger = Randomizer.GetRandomOption(Constants.Guest.HungryPercentage);
                this.PlayerSetGuestsHungry(guests, hunger);

                string[] messages = Constants.Trap.GetGuestHungerText(guests, hunger);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Thirst Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.ThirstyOptions);
                float thirsty = Randomizer.GetRandomOption(Constants.Guest.ThirstyPercentage);
                this.PlayerSetGuestsThirsty(guests, thirsty);

                string[] messages = Constants.Trap.GetGuestThirstText(guests, thirsty);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Bathroom Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.BathroomOptions);
                float bathroom = Randomizer.GetRandomOption(Constants.Guest.BathroomPercentage);
                this.PlayerSetGuestsToBathroom(guests, bathroom);

                string[] messages = Constants.Trap.GetGuestBathroomText(guests, bathroom);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Vomiting Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.VomitOptions);
                float vomit = Randomizer.GetRandomOption(Constants.Guest.VomitPercentage);
                this.PlayerSetGuestsToVomit(guests, vomit);

                string[] messages = Constants.Trap.GetGuestVomitingText(guests, vomit);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Happiness Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.HappinessOptions);
                float happiness = Randomizer.GetRandomOption(Constants.Guest.HappinessPercentage);
                this.PlayerSetGuestsHappy(guests, happiness);

                string[] messages = Constants.Trap.GetGuestHappinessText(guests, happiness);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Tiredness Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.TirednessOptions);
                float tiredness = Randomizer.GetRandomOption(Constants.Guest.TirednessPercentage);
                this.PlayerSetGuestsTired(guests, tiredness);

                string[] messages = Constants.Trap.GetGuestTirednessText(guests, tiredness);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }
            if (AP_Item.Name == "Guest Vandal Trap")
            {
                int amount = Randomizer.GetRandomInt(Constants.Guest.VandalsOptions[this.AP_Rules.difficulty]);
                this.PlayerSetGuestsAsVandals(amount);

                string[] messages = Constants.Trap.GetGuestVandalsTexts(amount);
                this.SendMessage(messages, canBeSuppressed: true);
                return;
            }

            if (AP_Item.Name == "Research Trap")
            {
                List<string> messages = new List<string>();
                List<string> types = new List<string>();
          
                List<Attraction> attractions = Randomizer.GetRandomAttractionFromParkForResearch(this);
                List<Shop> shops = Randomizer.GetRandomShopsFromParkForResearch(this);
                string themeTag = Randomizer.GetRandomDecorationThemeTagFromParkForResearch(this);

                if (attractions.Count > 0)
                {
                    types.Add(Constants.Research.Types[0]);
                }
                if (shops.Count > 0)
                {
                    types.Add(Constants.Research.Types[1]);
                }
                if (themeTag != null)
                {
                    types.Add(Constants.Research.Types[2]);
                }

                if (types.Count == 0)
                {
                    this.SendMessage("Research Trap activated, but it was harmless. You're lucky!", canBeSuppressed: true);
                    Helper.Debug($"[ParkitectController::PlayerRedeemTrap] Research Trap -> No Items found!");
                    return;
                }

                string trapType = Randomizer.GetRandomOption(types.ToArray());

                Helper.Debug($"[ParkitectController::PlayerRedeemTrap] Research Trap -> {trapType}");

                // Attractions
                if (trapType == Constants.Research.Types[0])
                {
                    foreach (Attraction attraction in attractions)
                    {
                        Helper.Debug($"[ParkitectController::PlayerRedeemTrap] Research Trap -> {attraction.getName()}");
                        this.PlayerAddToResearch(attraction);
                        string name = this.AttractionHasPrefabType(attraction) ? attraction.getPrefabType().ToString() : attraction.getName();
                        string type = Constants.Attraction.DetermineType(name);
                        messages.Add($"- {attraction.getName()} ({type})");
                    }
                }

                // Shops
                else if (trapType == Constants.Research.Types[1])
                {
                    foreach (Shop shop in shops)
                    {
                        Helper.Debug($"[ParkitectController::PlayerRedeemTrap] Research Trap -> {shop.getName()}");
                        this.PlayerAddToResearch(shop);
                        messages.Add($"- {shop.getName()} (Shops)");
                    }
                }

                // Decorations
                else if (trapType == Constants.Research.Types[2])
                {
                    List<string> DecorationProps = Randomizer.GetRandomDecorationPropsFromParkForResearch(themeTag);

                    foreach (string decorationProp in DecorationProps)
                    {
                        Helper.Debug($"[ParkitectController::PlayerRedeemTrap] Research Trap -> {themeTag}:{decorationProp}");
                        this.PlayerAddToResearch(decorationProp);
                        messages.Add($"- {decorationProp} (Decorations: {themeTag})");
                    }
                }

                Helper.Debug($"[ParkitectController::PlayerRedeemTrap] Research Trap -> preparing!");
                string message = Constants.Trap.GetResearchText();
                this.SendMessage(message, string.Join("\n", messages), canBeSuppressed: true);
                return;
            }

            // Below are only TrapLinks from other APWorld!
            // There may be some equal or close Traps we are currently providing, but i want a list here :)

            if (Constants.TrapLinks.Contains(AP_Item.Name))
            {
                Helper.Debug($"[ParkitectController::PlayerRedeemTrap] TrapLink -> {AP_Item.Name}");
            }

            // OpenRCT2
            if (AP_Item.Name == Constants.TrapLink.OpenRCT2.BathroomTrap)
            {
                this.TrapLinkBathroom(AP_Item.Name);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.OpenRCT2.FurryConventionTrap)
            {
                Prefabs employee = Constants.Employee.Options[3];
                int amount = Randomizer.GetRandomInt(Constants.Employee.SpawnRanges[this.AP_Rules.difficulty]);
                this.PlayerHireEmployees(employee, amount);

                this.TrapLinkActivated(AP_Item.Name);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.OpenRCT2.FoodPoisoningTrap)
            {
                this.TrapLinkPoison(AP_Item.Name);
                return;
            }

            // Pokemon
            if (AP_Item.Name == Constants.TrapLink.Pokemon.BurnTrap || AP_Item.Name == Constants.TrapLink.Pokemon.FireTrap)
            {
                this.TrapLinkThirstGuests(AP_Item.Name);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.Pokemon.PoisonTrap)
            {
                this.TrapLinkPoison(AP_Item.Name);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.Pokemon.SleepTrap)
            {
                this.TrapLinkSleepGuests(AP_Item.Name);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.Pokemon.IceTrap)
            {
                this.TrapLinkIce(AP_Item.Name, Prefabs.IceCreamStall);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.Pokemon.FreezeTrap)
            {
                this.TrapLinkIce(AP_Item.Name, Prefabs.SnowconesStall);
                return;
            }

            // Brave Fencer Musashi
            if (AP_Item.Name == Constants.TrapLink.BraveFencerMusashi.ToxinTrap)
            {
                this.TrapLinkPoison(AP_Item.Name);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.BraveFencerMusashi.StinkyTrap)
            {
                this.TrapLinkRestock(AP_Item.Name);
                return;
            }

            // Freedom Planet 2
            if (AP_Item.Name == Constants.TrapLink.FreedomPlanet2.ExpensiveStocks)
            {
                this.TrapLinkRestock(AP_Item.Name, 30);
                return;
            }
            if (AP_Item.Name == Constants.TrapLink.FreedomPlanet2.NoStocks)
            {
                this.TrapLinkRestock(AP_Item.Name, 100);
                return;
            }

            // Hammerwatch
            if (AP_Item.Name == Constants.TrapLink.Hammerwatch.FrostTrap)
            {
                this.TrapLinkIce(AP_Item.Name, Prefabs.SnowconesStall);
                return;
            }

            // Noita
            if (AP_Item.Name == Constants.TrapLink.Noita.PeaSoupTrap)
            {
                this.TrapLinkGreenPeas(AP_Item.Name);
                return;
            }

            // GTA SA
            if (AP_Item.Name == Constants.TrapLink.GTA_SA.Fat_CJ_Trap)
            {
                this.TrapLinkHungerGuests(AP_Item.Name);
                return;
            }

            // idk? -> Misc
            if (AP_Item.Name == Constants.TrapLink.Misc.FrozenTrap)
            {
                this.TrapLinkIce(AP_Item.Name, Prefabs.SnowconesStall);
                return;
            }

            Helper.Debug($"[ParkitectController::PlayerRedeemTrap] No Trap Handler found!");
        }

        private void TrapLinkBathroom(string trap)
        {
            float guests = Randomizer.GetRandomOption(Constants.Guest.BathroomOptions);
            float bathroom = Randomizer.GetRandomOption(Constants.Guest.BathroomPercentage);
            this.PlayerSetGuestsToBathroom(guests, bathroom);
            this.TrapLinkActivated(trap);
        }

        private void TrapLinkThirstGuests(string trap)
        {
            float guests = Randomizer.GetRandomOption(Constants.Guest.ThirstyOptions);
            float thirsty = Randomizer.GetRandomOption(Constants.Guest.ThirstyPercentage);
            this.PlayerSetGuestsThirsty(guests, thirsty);

            this.PlayerChangeWeather(Constants.Weather.Options.SUNNY);

            this.TrapLinkActivated(trap);
        }

        private void TrapLinkHungerGuests(string trap)
        {
            float guests = Constants.Guest.HungryOptions[Constants.Guest.HungryOptions.Length - 1];
            float hunger = Constants.Guest.HungryPercentage[Constants.Guest.HungryPercentage.Length - 1];
            this.PlayerSetGuestsHungry(guests, hunger);

            this.TrapLinkActivated(trap);
        }

        private void TrapLinkPoison(string trap)
        {
            // Guest Vomits
            float guests = Randomizer.GetRandomOption(Constants.Guest.VomitOptions);
            float vomit = Randomizer.GetRandomOption(Constants.Guest.VomitPercentage);
            this.PlayerSetGuestsToVomit(guests, vomit);

            // Shops Ingredients
            List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(Randomizer.GetRandomFloat());
            this.PlayerSetReDeliveryForProductShops(productShops);

            this.TrapLinkActivated(trap);
        }

        private void TrapLinkSleepGuests(string trap)
        {
            float guests = Randomizer.GetRandomOption(Constants.Guest.TirednessOptions);
            float tiredness = Randomizer.GetRandomOption(Constants.Guest.TirednessPercentage);
            this.PlayerSetGuestsTired(guests, tiredness);

            this.TrapLinkActivated(trap);
        }

        // rare cases only!
        private void TrapLinkIce(string trap, Prefabs prefabs)
        {
            Shop shop = Randomizer.GetRandomProductShopFromPark(prefabs);
            this.PlayerChangeWeather(Constants.Weather.Options.SUNNY);

            if (shop != null)
            {
                ShopVoucher voucher = this.PlayerCreateShopVoucher(shop as ProductShop);
                List<Guest> guests = Randomizer.GetRandomGuests(100f);
                this.PlayerAddGuestInventory(guests, voucher);
                string item = prefabs == Prefabs.IceCreamStall ? "Ice Cream" : "Snowcone";
                this.TrapLinkActivated(trap, $"Lucky Day for your Guests! Free {item} for everyone!");
                return;
            }

            this.TrapLinkActivated(trap, "but it is harmless");
        }
   
        private void TrapLinkGreenPeas(string trap)
        {
            ProductShop shop = Randomizer.GetRandomProductShopFromPark(Prefabs.HotDrinksStall) as ProductShop;

            if (shop != null)
            {
                Item tea = shop.products.ToList().Find(p => p.getPrefabType() == Prefabs.Tea);
                shop.servesPoisonedFood = true; // theoretically removed when shop has cleanup or restocks

                ShopVoucher voucher = this.PlayerCreateShopVoucher(shop);
                List<Guest> guests = Randomizer.GetRandomGuests(100f);
                this.PlayerAddGuestInventory(guests, voucher);

                this.TrapLinkActivated(trap, "Awesome, Your Tee is now made of Green Peas");
                return;
            }

            this.TrapLinkActivated(trap, "but it is harmless");
        }

        private void TrapLinkRestock(string trap, int percentage = 10)
        {
            List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(percentage);
            this.PlayerSetCleanShopJob(productShops);

            this.TrapLinkActivated(trap);
        }

        private void TrapLinkActivated(string trap, string message = "")
        {
            this.SendMessage($"TRAPLINK ACTIVATED!!! {trap}", message);
        }

        public void SendMessage(string message, string secondaryMessage = "", bool silent = false, bool canBeSuppressed = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (canBeSuppressed && this.SuppressMessagesUntilTime > Time.time)
            {
                Helper.Debug($"[ParkitectController::SendMessage] Suppressed - {message}");
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                Notification notification = new Notification(message, secondaryMessage, Notification.Type.DEFAULT);
                NotificationBar.Instance.addOngoingNotification(notification, silent);
            });
        }

        public void UpdateSuppressMessages()
        {
            this.SuppressMessagesUntilTime = Time.time + 2.1f;
        }

        public void SendMessage(string[] messages, bool silent = false, bool canBeSuppressed = false)
        {
            if (messages.Length == 2)
            {
                this.SendMessage(messages[0], messages[1], silent, canBeSuppressed);
                return;
            }

            this.SendMessage(messages[0], "", silent, canBeSuppressed);
        }

        // -----------------------------
        // Minsc :P
        // -----------------------------

        // Getter for park employees
        public List<Employee> GetParkEmployees((int Start, int End) range)
        {
            List<Employee> employees = this.GetParkEmployees();
            float percentage = Randomizer.GetRandomFloat(range.Start, range.End);

            return employees.Take(Mathf.CeilToInt(employees.Count * percentage)).ToList();
        }

        public List<Employee> GetParkEmployees ()
        {
            return GameController.Instance.park.getEmployees().ToList();
        }

        public List<Employee> GetAllCountableEmployeesFromPark(Prefabs prefabs)
        {
            int expLevel = this.GetEmployeeExperienceLevel(prefabs);
            if (prefabs == Constants.Employee.Options[0])
            {
                return this.GetAllCountableMechanicEmployeesFromPark(expLevel).Cast<Employee>().ToList();
            }

            if (prefabs == Constants.Employee.Options[1])
            {
                return this.GetAllCountableJanitorEmployeesFromPark(expLevel).Cast<Employee>().ToList();
            }

            if (prefabs == Constants.Employee.Options[2])
            {
                return this.GetAllCountableSecurityEmployeesFromPark(expLevel).Cast<Employee>().ToList();
            }

            if (prefabs == Constants.Employee.Options[3])
            {
                return this.GetAllCountableEntertainerEmployeesFromPark(expLevel).Cast<Employee>().ToList();
            }

            return this.GetAllCountableHandymanEmployeesFromPark(expLevel).Cast<Employee>().ToList();
        }

        public List<Mechanic> GetAllCountableMechanicEmployeesFromPark(int expLevel)
        {
            return this.GetParkEmployees()
                .Where(e => e.getPrefabType() == Constants.Employee.Options[0] && e.experienceLevel + 1 >= expLevel)  // UI says level 1 but its actually 0
                .Cast<Mechanic>()
                .ToList();
        }

        public List<Janitor> GetAllCountableJanitorEmployeesFromPark(int expLevel)
        {
            return this.GetParkEmployees()
                .Where(e => e.getPrefabType() == Constants.Employee.Options[1] && e.experienceLevel + 1 >= expLevel)  // UI says level 1 but its actually 0
                .Cast<Janitor>()
                .ToList();
        }

        public List<Security> GetAllCountableSecurityEmployeesFromPark(int expLevel)
        {
            return this.GetParkEmployees()
                .Where(e => e.getPrefabType() == Constants.Employee.Options[2] && e.experienceLevel + 1 >= expLevel)  // UI says level 1 but its actually 0
                .Cast<Security>()
                .ToList();
        }

        public List<Entertainer> GetAllCountableEntertainerEmployeesFromPark(int expLevel)
        {
            return this.GetParkEmployees()
                .Where(e => e.getPrefabType() == Constants.Employee.Options[3] && e.experienceLevel + 1 >= expLevel)  // UI says level 1 but its actually 0
                .Cast<Entertainer>()
                .ToList();
        }

        public List<Handyman> GetAllCountableHandymanEmployeesFromPark(int expLevel)
        {
            return this.GetParkEmployees()
                .Where(e => e.getPrefabType() == Constants.Employee.Options[4] && e.experienceLevel + 1 >= expLevel)  // UI says level 1 but its actually 0
                .Cast<Handyman>()
                .ToList();
        }

        // Attractions
        public List<Attraction> GetAllAttractionsFromAssetManager ()
        {
            return ScriptableSingleton<AssetManager>.Instance.getAttractionObjects().ToList();
        }
        public List<Attraction> GetAllAttractionsFromAssetManager(Prefabs prefab)
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => {
                try
                {
                    return a.getPrefabType() == prefab;
                } catch {
                    return false;
                }
            }).ToList();
        }
        public List<Attraction> GetAllAttractionsFromAssetManager(string prefabName)
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => {
                try
                {
                    return a.getPrefabType().ToString() == prefabName;
                }
                catch
                {
                    return false;
                }
            }).ToList();
        }
        public Attraction GetAttractionFromAssetManager(string attraction)
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => a.getName() == attraction).First();
        }
        public List<Attraction> GetAllAttractionsFromPark ()
        {
            return GameController.Instance.park.getAttractions().ToList();
        }
        public List<Attraction> GetAllCountableAttractionsFromPark(string attractionPrefab)
        {
            bool prefabIsMod = Constants.Mods.All.Contains(attractionPrefab);
            string prefabName = prefabIsMod ? this.GetSerializedFromPrefabs(attractionPrefab) : Helper.GetPrefabsFromString(attractionPrefab).ToString();
            
            return this.GetAllAttractionsFromPark()
                .Where(a => {
                    bool attractionIsMod = !this.AttractionHasPrefabType(a);
                    string aName = attractionIsMod ? a.getName() : a.getPrefabType().ToString();

                    return aName == prefabName
                        && a.state == Attraction.State.OPENED
                        && a.customersCount > 0
                        && !a.statsAreOutdated;
                })
                .ToList();
        }
        public List<Attraction> GetAllCountableAttractionsTypeFromPark(string type, DecoRating decoRating = null)
        {
            string[] attractions = Constants.Attraction.All.Concat(Constants.Mods.Attractions).ToArray();

            if (type == "Calm Rides")
            {
                attractions = Constants.Attraction.CalmRides.Concat(Constants.Mods.CalmRides).ToArray();
            }
            else if (type == "Thrill Rides")
            {
                attractions = Constants.Attraction.ThrillRides.Concat(Constants.Mods.ThrillRides).ToArray();
            }
            else if (type == "Coaster Rides")
            {
                attractions = Constants.Attraction.CoasterRides.Concat(Constants.Mods.CoasterRides).ToArray();
            }
            else if (type == "Transport Rides")
            {
                attractions = Constants.Attraction.TransportRides.Concat(Constants.Mods.TransportRides).ToArray();
            }
            else if (type == "Water Rides")
            {
                attractions = Constants.Attraction.WaterRides.Concat(Constants.Mods.WaterRides).ToArray();
            }

            return this.GetAllAttractionsFromPark()
                .Where(a => {
                    bool isAttraction = false;

                    if (this.AttractionHasPrefabType(a))
                    {
                        isAttraction = attractions.Contains(a.getPrefabType().ToString());
                    }
                    else
                    {
                        isAttraction = attractions.Contains(a.getName());
                    }

                    if (decoRating != null && !decoRating.Check(a.getDecoResultScore()))
                    {
                        return false;
                    }

                    return isAttraction
                        && a.state == Attraction.State.OPENED
                        && a.customersCount > 0
                        && !a.statsAreOutdated;
                })
                .ToList();
        }
        public List<Attraction> GetAllAvailableAttractions()
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => {
                string referenceName = a.getResearchReferenceName();
                return a.isAvailableInParks && this.HasUnlockedResearchRule(referenceName) && !this.CanUnlockedResearchRule(referenceName);
            }).ToList();
        }

        // Stalls/Shops
        public List<Shop> GetAllShopsFromAssetManager()
        {
            return ScriptableSingleton<AssetManager>.Instance.getShopObjects().ToList();
        }
        public List<Shop> GetAllShopsFromAssetManager(Prefabs prefab)
        {
            return this.GetAllShopsFromAssetManager().Where(s =>
            {
                try
                {
                    return s.getPrefabType() == prefab;
                }
                catch {
                    return false;
                }
            }).ToList();
        }
        public List<Shop> GetAllShopsFromAssetManager(string prefabName)
        {
            return this.GetAllShopsFromAssetManager().Where(s =>
            {
                try
                {
                    return s.getPrefabType().ToString() == prefabName;
                }
                catch
                {
                    return false;
                }
            }).ToList();
        }
        public Shop GetShopFromAssetManager(string shop)
        {
            return this.GetAllShopsFromAssetManager().Where(s => s.getName() == shop).First();
        }

        public List<Shop> GetAllShopsFromPark()
        {
            return GameController.Instance.park.getShops().ToList();
        }

        public List<Shop> GetAllCountableShopsFromPark (string shopPrefab)
        {
            bool prefabIsMod = Constants.Mods.All.Contains(shopPrefab);
            string prefabName = prefabIsMod ? this.GetSerializedFromPrefabs(shopPrefab) : Helper.GetPrefabsFromString(shopPrefab).ToString();

            return this.GetAllShopsFromPark()
                .Where(s =>
                {
                    bool shopIsMod = !this.ShopHasPrefabType(s);
                    string sName = shopIsMod ? s.getName() : s.getPrefabType().ToString();

                    return sName == prefabName
                        && s.opened
                        && s.customersCount > 0;
                })
                .ToList();
        }
        public List<Shop> GetAllCountableShopsTypeFromPark(string type)
        {
            string[] shops = Constants.Stall.Food.Concat(Constants.Mods.Food).ToArray();

            // For the future !
            if (type == "Drinks")
            {
                shops = Constants.Stall.Drinks.Concat(Constants.Mods.Drinks).ToArray();
            }
            if (type == "Facilities")
            {
                shops = Constants.Stall.Facilities.Concat(Constants.Mods.Facilities).ToArray();
            }
            if (type == Constants.Stall.GenericType)
            {
                shops = Constants.Stall.All.Concat(Constants.Mods.Stalls).ToArray();
            }

            return this.GetAllShopsFromPark()
                .Where(s =>
                {
                    bool isShop = false;

                    if (this.ShopHasPrefabType(s))
                    {
                        isShop = shops.Contains(s.getPrefabType().ToString());
                    } else
                    {
                        isShop = shops.Contains(s.getName());
                    }

                    return isShop
                        && s.opened
                        && s.customersCount > 0;
                })
                .ToList();
        }
        public List<Shop> GetAllAvailableShops()
        {
            return this.GetAllShopsFromAssetManager().Where(s =>
            {
                string referenceName = s.getResearchReferenceName();
                return s.isAvailableInParks && this.HasUnlockedResearchRule(referenceName) && !this.CanUnlockedResearchRule(referenceName);
            }).ToList();
        }

        public string GetSerializedFromPrefabs (string prefabs)
        {
            // Counts for all items except ingame items :)
            if (Constants.AllNonItemTypes.Contains(prefabs) || Constants.Mods.All.Contains(prefabs))
            {
                return prefabs;
            }

            return Constants.AllGameItems[Helper.GetPrefabsFromString(prefabs)];
        }

        protected bool AttractionHasPrefabType(Attraction attraction)
        {
            try
            {
                Prefabs prefabs = attraction.getPrefabType();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected bool ShopHasPrefabType(Shop shop)
        {
            try
            {
                Prefabs prefab = shop.getPrefabType();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Utility Buildings
        public List<UtilityBuilding> GetAllUtilityBuildingsFromAssetManager()
        {
            return ScriptableSingleton<AssetManager>.Instance.getUtilityBuildingObjects()
                .Where(u => Constants.UtilityBuilding.All.Contains(u.getPrefabType().ToString()))
                .ToList();
        }
    
        public List<UtilityBuilding> GetAllUtilityBuildingsFromAssetManager(string prefabName)
        {
            return this.GetAllUtilityBuildingsFromAssetManager().Where(s =>
            {
                try
                {
                    return s.getPrefabType().ToString() == prefabName;
                }
                catch
                {
                    return false;
                }
            }).ToList();
        }
        public List<UtilityBuilding> GetAllUtilityBuildingsFromAssetManager(Prefabs prefab)
        {
            return this.GetAllUtilityBuildingsFromAssetManager().Where(s =>
            {
                try
                {
                    return s.getPrefabType() == prefab;
                }
                catch
                {
                    return false;
                }
            }).ToList();
        }

        public List<UtilityBuilding> GetAllUtilityBuildingsFromPark()
        {
            return this.GetAllUtilityBuildingsFromAssetManager().Where(z => z.isAvailableInParks).ToList();
        }

        // Decorations
        public List<Deco> GetAllDecosFromAssetManager()
        {
            return ScriptableSingleton<AssetManager>.Instance.getDecoObjects().Where(d => !Constants.Decorations.Excludes.Contains(d.getName())).ToList();
        }

        public List<Deco> GetAllAvailableDecosFromPark()
        {
            return this.GetAllDecosFromAssetManager().Where(d => d.isAvailableInParks).ToList();
        }

        public List<PathAttachment> GetAllPathAttachmentsFromAssetManager()
        {
            return ScriptableSingleton<AssetManager>.Instance.getPathAttachmentObjects().ToList();
        }

        public List<BuildableObject> GetAllDecorationsFromAssetManager()
        {
            return this.GetAllDecosFromAssetManager()
                .Cast<BuildableObject>()
                .Concat(this.GetAllPathAttachmentsFromAssetManager())
                .ToList();
        }
        public List<BuildableObject> GetAllDecorationsFromAssetManager(ThemeContainer themeContainer)
        {
            return this.GetAllDecorationsFromAssetManager()
                .Where(d => d.themeTag == themeContainer.themeTag)
                .ToList();
        }

        // Research
        public void ResearchRuleUpdateIsUnlocked(string referenceName, bool isUnlocked = false)
        {
            if (this.HasUnlockedResearchRule(referenceName) == isUnlocked)
            {
                Helper.Debug($"[ParkitectController::ResearchRuleUpdateIsUnlocked] isUnlocked already {isUnlocked} = {referenceName}");
                return;
            }

            ResearchRule rule = GameController.Instance.park.scenario.research.getRule(referenceName);

            if (rule == null)
            {
                Helper.Debug($"[ParkitectController::ResearchRuleUpdateIsUnlocked] No Rule found for {referenceName}");
                return;
            }

            GameController.Instance.park.scenario.research.removeRule(rule);
            rule.isUnlocked = isUnlocked;
            GameController.Instance.park.scenario.research.addRule(rule);
            GameController.Instance.park.scenario.research.unlockNewContentInNewParks = true;

            this.UpdateResearchTeams();
        }

        public void ResearchRuleUpdateCanUnlock(string referenceName, bool canUnlock = true)
        {
            ResearchRule rule = GameController.Instance.park.scenario.research.getRule(referenceName);

            if (rule == null)
            {
                Helper.Debug($"[ParkitectController::ResearchRuleUpdateCanUnlock] No Rule found for {referenceName}");
                return;
            }

            GameController.Instance.park.scenario.research.removeRule(rule);
            rule.canUnlock = canUnlock;
            GameController.Instance.park.scenario.research.addRule(rule);
            GameController.Instance.park.scenario.research.unlockNewContentInNewParks = true;

            this.UpdateResearchTeams();
        }

        private void UpdateResearchTeams()
        {
            foreach (ResearchTeam researchTeam in GameController.Instance.park.scenario.research.getTeams())
            {
                researchTeam.updateResearchableState();
            }
        }

        public bool HasUnlockedResearchRule(string referenceName)
        {
            if (Constants.Decorations.Excludes.Contains(referenceName))
            {
                return false;
            }

            ResearchRule rule = GameController.Instance.park.scenario.research.getRule(referenceName);

            return rule != null ? rule.isUnlocked : false;
        }

        public bool CanUnlockedResearchRule(string referenceName)
        {
            if (Constants.Decorations.Excludes.Contains(referenceName))
            {
                return false;
            }
            return GameController.Instance.park.scenario.research.getRule(referenceName)?.canUnlock ?? false;
        }

        public ThemeContainer FindThemeContainer(string themeTag)
        {
            return ScriptableSingleton<AssetManager>.Instance.getTheme(themeTag);
        }

        // Helper

        public int GetParkGuestCount()
        {
            return GameController.Instance.park.getGuestCount();
        }

        public int GetEmployeeExperienceLevel(Prefabs prefabs)
        {
            return Constants.Employee.ExperienceLevels[prefabs][this.AP_Rules.difficulty];
        }

        public double GetPlayerMoney()
        {
            return GameController.Instance.park.parkInfo.money;
        }

        public float GetFee()
        {
            switch (this.AP_Rules.difficulty)
            {
                case 1: // medium = 1
                    return 20f;

                case 2: // hard = 2
                    return 30f;

                case 3: // extreme = 3
                    return 40f;

                default:
                    return 10f;
            }
        }

        public static void UnlockMissingScenarios()
        {
            Helper.Debug($"[ParkitectController::UnlockMissingScenarios]");
            CampaignProgress cp = CampaignProgress.Instance;
            bool updated = false;

            if (!File.Exists(CampaignProgress.campaignProgressFilePath + "_" + Constants.ParkitectCampaignBackupFilenameSuffix))
            {
                cp.createBackupFile(Constants.ParkitectCampaignBackupFilenameSuffix);
            }

            // main campaign
            int mainCompletedScenarios = cp.getMainCampaignCompletedScenariosCount();
            if (mainCompletedScenarios < Constants.Scenario.MainCampaignScenarios.Length)
            {
                ArchipelagoMod.Src.Scenario[] missingScenarios = Constants.Scenario.MainCampaignScenarios.Skip(mainCompletedScenarios).ToArray();

                foreach (ArchipelagoMod.Src.Scenario scenario in missingScenarios)
                {
                    updated = true;
                    cp.setScenarioCompleted(scenario.CampaignGUID, scenario.ScenarioGUID);
                }
            }

            // main bonus campaign
            if (!cp.bonus1Revealed)
            {
                updated = true;
                ArchipelagoMod.Src.Scenario bonusScenario = Constants.Scenario.MainBonusCampaignScenarios[0];
                cp.setScenarioCompleted(bonusScenario.CampaignGUID, bonusScenario.ScenarioGUID);
                cp.bonus1Revealed = true;
            }

            // dlc campaign
            int dlc1CompletedScenarios = cp.getDLC1CampaignCompletedScenariosCount();
            if (dlc1CompletedScenarios < Constants.Scenario.DLC1CampaignScenarios.Length)
            {
                ArchipelagoMod.Src.Scenario[] missingScenarios = Constants.Scenario.DLC1CampaignScenarios.Skip(dlc1CompletedScenarios).ToArray();

                foreach (ArchipelagoMod.Src.Scenario scenario in missingScenarios)
                {
                    updated = true;
                    cp.setScenarioCompleted(scenario.CampaignGUID, scenario.ScenarioGUID);
                }
            }

            if (updated && !cp.mapRevealed)
            {
                cp.mapRevealed = true;
            }
        }
    }
}
