using ArchipelagoMod.Src.Dispatcher;
using ArchipelagoMod.Src.SlotData;
using Parkitect.UI;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static ArchipelagoMod.Src.Constants;

namespace ArchipelagoMod.Src.Controller
{
    class ParkitectController : MainThreadDispatcher
    {
        // Timescale for Speedups
        readonly float OldTimeScale = Time.timeScale;
        private List<_ResearchItem> ResearchItems = new List<_ResearchItem>();
        public AP_Rules AP_Rules = null;
        public SaveData SaveData = null;

        void Start()
        {
            Helper.Debug($"[ParkitectController::Start]");
            this.UpdateResearchItems();
            Constants.ScenarioName = GameController.Instance.park.parkName;
            this.SaveData = GetComponent<SaveData>();
            Helper.Debug($"[ParkitectController::Start] Booted");
        }

        public void UpdateResearchItems ()
        {
            this.ResearchItems = new List<_ResearchItem>();
            int length = GameController.Instance.park.scenario.research.getItems().Count;

            for (int i = 0; i < length; i++)
            {
                string referenceName = GameController.Instance.park.scenario.research.getItemReferenceNames()[i];
                this.ResearchItems.Add(new _ResearchItem(i, referenceName));
            }
        }

        public bool HasResearchItem (string ReferenceName)
        {
            _ResearchItem item = this.ResearchItems.Find(i => i.ReferenceName == ReferenceName);
            return item != null;
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
        public void PlayerAddMoney (float money = 500)
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

        // Adds/Substracts Guests money
        public void PlayerChangeGuestsMoney (float money = 30f, float guests = 25f, string sign = "+")
        {
            if (!Constants.Guest.MoneyOptions.Contains(money) || guests < 1)
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
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

        // Kills specific guests
        public void PlayerKillGuests (int guests = 50)
        {
            if (!Constants.Guest.KillOptions.Contains(guests))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(0, guests))
                {
                    guest.Kill();
                }
            });
        }

        // Set amount of guests hungry
        public void PlayerSetGuestsHungry(float guests = 15f, float hunger = 80f)
        {
            if (!Constants.Guest.HungryOptions.Contains(guests) || !Constants.Guest.HungryPercentage.Contains(hunger))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
                {
                    guest.Hunger = hunger;
                }
            });
        }

        // Set amount of guests thirsty
        public void PlayerSetGuestsThirsty (float guests = 15f, float thirst = 80f)
        {
            if (!Constants.Guest.ThirstyOptions.Contains(guests) || !Constants.Guest.ThirstyPercentage.Contains(thirst))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
                {
                    guest.Thirst = thirst;
                }
            });
        }

        // set amount of guests to the bathroom
        public void PlayerSetGuestsToBathroom (float guests = 80f, float bathroom = 80f)
        {
            if (!Constants.Guest.BathroomOptions.Contains(guests) || !Constants.Guest.BathroomPercentage.Contains(bathroom))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
                {
                    guest.ToiletUrgency = bathroom;
                }
            });
        }

        // Set amount of guests to vomit
        public void PlayerSetGuestsToVomit (float guests = 20f, float vomit = 60f)
        {
            if (!Constants.Guest.VomitOptions.Contains(guests) || !Constants.Guest.VomitPercentage.Contains(vomit))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
                {
                    if (!guest.isBusy())
                    {
                        guest.standUp();
                    }
                    guest.Nausea = vomit;
                }
            });
        }

        // Set amount of guests happy
        public void PlayerSetGuestsHappy (float guests = 60f, float happiness = 80f)
        {
            if (!Constants.Guest.HappinessOptions.Contains(guests) || !Constants.Guest.HappinessPercentage.Contains(happiness))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
                {
                    guest.Happiness = happiness;
                }
            });
        }

        // Set amount of guests tired
        public void PlayerSetGuestsTired(float guests = 60f, float tiredness = 80f)
        {
            if (!Constants.Guest.TirednessOptions.Contains(guests) || !Constants.Guest.TirednessPercentage.Contains(tiredness))
            {
                return;
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                foreach (Guest guest in Randomizer.GetRandomGuests(guests))
                {
                    guest.Tiredness = tiredness;
                }
            });
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
            MainThreadDispatcher.Enqueue(() =>
            {
                List<Guest> guests = Randomizer.GetRandomGuests(0f, amount);

                foreach (Guest guest in guests)
                {
                    guest.setIsVandal(true);
                }
            });
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
        public void PlayerHireEmployees(Prefabs employee, (int Start, int End) range, int customeIndex = 0) // Note: the last Hired Employeee will be picked up always
        {
            // fix position they spawn
            if (!Helper.IsRange(Constants.Employee.SpawnRanges, range) || !Constants.Employee.Options.Contains(employee))
            {
                return;
            }

            this.PlayerHireEmployees(employee, Randomizer.GetRandomInt(range), customeIndex);
        }
        public void PlayerHireEmployees(Prefabs employee, int amount, int customeIndex = 0) // Note: the last Hired Employeee will be picked up always
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                for (int i = 0; i < amount; i += 1)
                {
                    EmployeeHireCommand hireCommand = new EmployeeHireCommand(employee, customeIndex);
                    hireCommand.isOwnCommand = true;
                    hireCommand.run();
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

        // Send Employee to Trainingsroom
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
            this.SaveData.AddUnlockedItem(AP_Item.PrefabName);

            if (Constants.Stall.All.Contains(AP_Item.Name))
            {
                this.PlayerAddStall(AP_Item.PrefabName);
                return;
            }

            this.PlayerAddAttraction(AP_Item.PrefabName);
        }

        public bool PlayerHasUnlockedItem(AP_Item AP_Item)
        {
            if (this.SaveData.HasUnlockedItem(AP_Item.PrefabName))
            {
                return true;
            }

            if (Constants.Stall.All.Contains(AP_Item.Name))
            {
                return this.GetAllShopsFromAssetManager(AP_Item.PrefabName).First().isAvailableInParks;
            }

            return this.GetAllAttractionsFromAssetManager(AP_Item.PrefabName).First().isAvailableInParks;
        }

        // Remove all Rides from List
        public void PlayerRemoveAllRides()
        {
            List<Attraction> attractions = this.GetAllAttractionsFromAssetManager();

            foreach (Attraction attraction in attractions)
            {
                attraction.isAvailableInParks = false;
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

        // Add a new Ride to List
        public void PlayerAddAttraction(Prefabs prefab)
        {
            Attraction attraction = this.GetAllAttractionsFromAssetManager(prefab).First();
            this.PlayerAddAttraction(attraction);
        }
        public void PlayerAddAttraction(string prefabName)
        {
            Attraction attraction = this.GetAllAttractionsFromAssetManager(prefabName).First();
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

        // Remove all Rides
        public void PlayerRemoveAllStalls()
        {
            List<Shop> shops = this.GetAllShopsFromAssetManager();

            foreach (Shop shop in shops)
            {
                shop.isAvailableInParks = false;
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
            Shop shop = this.GetAllShopsFromAssetManager(prefabName).First();
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
        // Shops options
        // -----------------------------

        // Re-deliver Ingredients for a shop
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
                            //productShop.stock.resourceNameContentAssoc =
                            //GameController.Instance.park.orderResources(productShop, ingredient.resource, 1);
                        }
                    }

                    productShop.checkOrderIngredients();
                }
            });
        }

        // Clean up product shops
        public void PlayerSetCleanShopJob(List<ProductShop> productShops)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                if (productShops.Count > 0)
                {
                    foreach (ProductShop productShop in productShops)
                    {
                        productShop.triggerClean();
                    }
                }
            });
        }

        // -----------------------------
        // Scenario options
        // -----------------------------

        // Add Park goal
        public void PlayerAddScenarioGoal (IScenarioGoal goal, List<IScenarioGoalReward> rewards = null, bool optional = false) // Note: Change the value before calling this method!
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
        public void PlayerChangeResearchState (bool flag = false)
        {

        }

        public void PlayerRedeemTrap (AP_Item AP_Item)
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
                Attraction attraction = Randomizer.GetRandomAttractionFromPark(100f).First();
                List<Guest> guests = Randomizer.GetRandomGuests(number);

                foreach (Guest guest in guests)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        guest.addToInventory(this.PlayerCreateAttractionVoucher(attraction));
                    });
                }

                string[] messages = Constants.Trap.GetAttractionVoucherText(attraction.getName(), number);
                this.SendMessage(messages);
                return;
            }

            if (AP_Item.Name == "Shop Ingredients Trap")
            {
                List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(Randomizer.GetRandomFloat());
                this.PlayerSetReDeliveryForProductShops(productShops);

                string[] messages = Constants.Trap.GetShopIngredientsText(productShops.Select(shop => shop.getName()).ToArray());
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Shop Cleaning Trap")
            {
                List<ProductShop> productShops = Randomizer.GetRandomProductShopsFromPark(Randomizer.GetRandomFloat());
                this.PlayerSetCleanShopJob(productShops);

                string[] messages = Constants.Trap.GetShopCleaningText(productShops.Select(shop => shop.getName()).ToArray());
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Shop Voucher Trap")
            {
                float number = Randomizer.GetRandomFloat();
                ProductShop shop = Randomizer.GetRandomProductShopsFromPark(100f).First();
                List<Guest> guests = Randomizer.GetRandomGuests(number);

                foreach (Guest guest in guests)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        guest.addToInventory(this.PlayerCreateShopVoucher(shop));
                    });
                }

                string[] messages = Constants.Trap.GetShopVoucherText(shop.getName(), number);
                this.SendMessage(messages);
                return;
            }

            if (AP_Item.Name == "Employee Hiring Trap")
            {
                Prefabs employee = Randomizer.GetRandomEmployee();
                int amount = Randomizer.GetRandomInt(Constants.Employee.SpawnRanges[this.AP_Rules.difficulty]);
                this.PlayerHireEmployees(employee, amount);

                string[] messages = Constants.Trap.GetEmployeeHiringText(employee.ToString(), amount);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Employee Training Trap")
            {
                (int Start, int End) range = Constants.Employee.TrainingRanges[this.AP_Rules.difficulty];
                int number = Randomizer.GetRandomInt(range);
                this.PlayerSetEmployeesTraining(this.GetParkEmployees().Take(number).ToList());

                string[] messages = Constants.Trap.GetEmployeeTrainingText(number.ToString());
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Employee Tiredness Trap")
            {
                (int Start, int End) range = Constants.Employee.TirednessRanges[this.AP_Rules.difficulty];
                float number = Randomizer.GetRandomFloat(range);
                this.PlayerSetEmployeesTired(this.GetParkEmployees().Take((int)number).ToList(), number);

                string[] messages = Constants.Trap.GetEmployeeTirednessText(number.ToString());
                this.SendMessage(messages);
                return;
            }

            if (AP_Item.Name == "Player Money Trap")
            {
                float money = Randomizer.GetRandomOption(Constants.Player.MoneyOptions);
                this.PlayerAddMoney(money);

                string[] messages = Constants.Trap.GetPlayerMoneyText(money);
                this.SendMessage(messages);
                return;
            }

            if (AP_Item.Name == "Weather Rainy Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.RAINY);

                string[] messages = Constants.Trap.GetWeatherText("Rainy");
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Weather Stormy Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.STORMY);

                string[] messages = Constants.Trap.GetWeatherText("Stormy");
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Weather Cloudy Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.CLOUDY);

                string[] messages = Constants.Trap.GetWeatherText("Cloudy");
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Weather Sunny Trap")
            {
                this.PlayerChangeWeather(Constants.Weather.Options.SUNNY);

                string[] messages = Constants.Trap.GetWeatherText("Sunny");
                this.SendMessage(messages);
                return;
            }

            if (AP_Item.Name == "Guest Spawn Trap")
            {
                int amount = Randomizer.GetRandomOption(Constants.Guest.SpawnOptions);
                this.PlayerAddGuests(amount);

                string[] messages = Constants.Trap.GetGuestSpawnText(amount);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Kill Trap")
            {
                int amount = Randomizer.GetRandomOption(Constants.Guest.KillOptions);
                this.PlayerKillGuests(amount);

                string[] messages = Constants.Trap.GetGuestKillText(amount);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Money Trap")
            {
                string sign = AP_Rules.GetGuestsMoneyFluxSign();
                float money = Randomizer.GetRandomOption(Constants.Guest.MoneyOptions);
                float guests = Randomizer.GetRandomOption(Constants.Guest.MoneyOptions);
                this.PlayerChangeGuestsMoney(money, guests, sign);

                string[] messages = Constants.Trap.GetGuestMoneyText(money, guests, sign);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Kill Trap")
            {
                int amount = Randomizer.GetRandomOption(Constants.Guest.KillOptions);
                this.PlayerKillGuests(amount);

                string[] messages = Constants.Trap.GetGuestKillText(amount);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Hunger Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.HungryOptions);
                float hunger = Randomizer.GetRandomOption(Constants.Guest.HungryPercentage);
                this.PlayerSetGuestsHungry(guests, hunger);

                string[] messages = Constants.Trap.GetGuestHungerText(guests, hunger);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Thirst Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.ThirstyOptions);
                float thirsty = Randomizer.GetRandomOption(Constants.Guest.ThirstyPercentage);
                this.PlayerSetGuestsThirsty(guests, thirsty);

                string[] messages = Constants.Trap.GetGuestThirstText(guests, thirsty);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Bathroom Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.BathroomOptions);
                float bathroom = Randomizer.GetRandomOption(Constants.Guest.BathroomPercentage);
                this.PlayerSetGuestsToBathroom(guests, bathroom);

                string[] messages = Constants.Trap.GetGuestBathroomText(guests, bathroom);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Vomiting Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.VomitOptions);
                float vomit = Randomizer.GetRandomOption(Constants.Guest.VomitPercentage);
                this.PlayerSetGuestsToVomit(guests, vomit);

                string[] messages = Constants.Trap.GetGuestVomitingText(guests, vomit);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Happiness Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.HappinessOptions);
                float happiness = Randomizer.GetRandomOption(Constants.Guest.HappinessPercentage);
                this.PlayerSetGuestsHappy(guests, happiness);

                string[] messages = Constants.Trap.GetGuestHappinessText(guests, happiness);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Tiredness Trap")
            {
                float guests = Randomizer.GetRandomOption(Constants.Guest.TirednessOptions);
                float tiredness = Randomizer.GetRandomOption(Constants.Guest.TirednessPercentage);
                this.PlayerSetGuestsTired(guests, tiredness);

                string[] messages = Constants.Trap.GetGuestTirednessText(guests, tiredness);
                this.SendMessage(messages);
                return;
            }
            if (AP_Item.Name == "Guest Vandal Trap")
            {
                int amount = Randomizer.GetRandomInt(Constants.Guest.VandalsOptions[this.AP_Rules.difficulty]);
                this.PlayerSetGuestsAsVandals(amount);

                string[] messages = Constants.Trap.GetGuestVandalsTexts(amount);
                this.SendMessage(messages);
                return;
            }

            Helper.Debug($"[ParkitectController::PlayerRedeemTrap] No Trap Handler found!");
        }

        public void SendMessage(string message, string secondaryMessage = "", bool silent = false, Notification.Type type = Notification.Type.DEFAULT)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Notification notification = new Notification(message, secondaryMessage, Notification.Type.DEFAULT, null);
                NotificationBar.Instance.addOngoingNotification(notification, silent);
            });
        }

        public void SendMessage(string[] messages, bool silent = false, Notification.Type type = Notification.Type.DEFAULT)
        {
            if (messages.Length == 2)
            {
                this.SendMessage(messages[0], messages[1], silent, type);
                return;
            }

            this.SendMessage(messages[0], "", silent, type);
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

        // Gets all Attractions
        public List<Attraction> GetAllAttractionsFromAssetManager ()
        {
            return ScriptableSingleton<AssetManager>.Instance.getAttractionObjects().ToList();
        }
        public List<Attraction> GetAllAttractionsFromAssetManager(Prefabs prefab)
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => a.getPrefabType() == prefab).ToList();
        }
        public List<Attraction> GetAllAttractionsFromAssetManager(string prefabName)
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => a.getPrefabType().ToString() == prefabName).ToList();
        }
        public Attraction GetAttractionFromAssetManager(string attraction)
        {
            return this.GetAllAttractionsFromAssetManager().Where(a => a.getName() == attraction).First();
        }

        public List<Attraction> GetAllAttractionsFromPark ()
        {
            return GameController.Instance.park.getAttractions().ToList();
        }
        public List<Attraction> GetAllAttractionsFromPark (Prefabs prefab)
        {
            return this.GetAllAttractionsFromPark().Where(a => a.getPrefabType() == prefab).ToList();
        }
        public List<Attraction> GetAllCountableAttractionsFromPark(string attractionPrefab)
        {
            Prefabs prefabs = Helper.GetPrefabsFromString(attractionPrefab);

            return this.GetAllAttractionsFromPark()
                .Where(a =>
                    a.getPrefabType() == prefabs &&
                    a.state == Attraction.State.OPENED &&
                    a.customersCount > 0 &&
                    !a.statsAreOutdated
                ).ToList();
        }
        public List<Attraction> GetAllCountableAttractionsTypeFromPark(string type)
        {
            string[] attractions = Constants.Attraction.WaterRides;

            if (type == "Calm Rides")
            {
                attractions = Constants.Attraction.CalmRides;
            }
            if (type == "Thrill Rides")
            {
                attractions = Constants.Attraction.ThrillRides;
            }
            if (type == "Coaster Rides")
            {
                attractions = Constants.Attraction.CoasterRides;
            }
            if (type == "Transport Rides")
            {
                attractions = Constants.Attraction.TransportRides;
            }

            return this.GetAllAttractionsFromPark()
                .Where(a =>
                    attractions.Contains(a.getPrefabType().ToString()) &&
                    a.state == Attraction.State.OPENED &&
                    a.customersCount > 0 &&
                    !a.statsAreOutdated
                ).ToList();
        }

        // Gets all Stalls
        public List<Shop> GetAllShopsFromAssetManager()
        {
            return ScriptableSingleton<AssetManager>.Instance.getShopObjects().ToList();
        }
        public List<Shop> GetAllShopsFromAssetManager(Prefabs prefab)
        {
            return this.GetAllShopsFromAssetManager().Where(a => a.getPrefabType() == prefab).ToList();
        }
        public List<Shop> GetAllShopsFromAssetManager(string prefabName)
        {
            return this.GetAllShopsFromAssetManager().Where(a => a.getPrefabType().ToString() == prefabName).ToList();
        }

        public List<Shop> GetAllShopsFromPark ()
        {
            return GameController.Instance.park.getShops().ToList();
        }
        public List<Shop> GetAllShopsFromPark (Prefabs prefab)
        {
            return this.GetAllShopsFromPark().Where(a => a.getPrefabType() == prefab).ToList();
        }

        public List<Shop> GetAllCountableShopsFromPark (string shopPrefab)
        {
            Prefabs prefabs = Helper.GetPrefabsFromString(shopPrefab);

            return this.GetAllShopsFromPark()
                .Where(s =>
                    s.getPrefabType() == prefabs &&
                    s.opened &&
                    s.customersCount > 0
                ).ToList();
        }
        public List<Shop> GetAllCountableShopsTypeFromPark(string type)
        {
            string[] shops = Constants.Stall.All;

            // For the future !
            //if (type == "Drinks")
            //{
            //    shops = Constants.Stall.Drinks;
            //}
            //if (type == "Food")
            //{
            //    shops = Constants.Stall.Food;
            //}
            //if (type == "Shops")
            //{
            //    shops = Constants.Stall.Shops;
            //}

            return this.GetAllShopsFromPark()
                .Where(s =>
                    shops.Contains(s.getPrefabType().ToString()) &&
                    s.opened &&
                    s.customersCount > 0
                ).ToList();
        }

        public _ResearchItem GetResearchItem (string referenceName)
        {
            return this.ResearchItems.Where(i => i.ReferenceName == referenceName).First();
        }

        public string GetSerializedFromPrefabs (string prefabs)
        {
            // Counts for all items except ingame items :)
            if (Constants.AllNonItemTypes.Contains(prefabs))
            {
                return prefabs;
            }

            return Constants.AllGameItems[Helper.GetPrefabsFromString(prefabs)];
        }
    }
}
