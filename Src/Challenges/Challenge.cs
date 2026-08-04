using ArchipelagoMod.Src.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchipelagoMod.Src.Challenges
{
    class Challenge
    {
        private ParkitectController ParkitectController = null;

        public string SerializedPanelId = null;

        protected NauseaRating NauseaRating = null;
        protected ExcitementRating ExcitementRating = null;
        protected IntensityRating IntensityRating = null;
        protected SatisfactionRating SatisfactionRating = null;
        protected GuestsRating GuestsRating = null;
        protected RevenueRating RevenueRating = null;
        protected ProfitRating ProfitRating = null;
        protected DecoRating DecoRating = null;
        protected PhotoRating Photo = null;
        protected VoucherRating Voucher = null;

        protected ParkGuest ParkGuest = null;
        protected ParkEmployee ParkEmployee = null;
        protected ParkMoney ParkMoney = null;

        protected string Attraction = null;
        protected string Shop = null;
        protected string Type = null;
        protected int Count = 1;

        public int LocationId;
        public string PanelId = null;
        public int Index;

        public Challenge (ParkitectController ParkitectController, int locationId)
        {
            this.ParkitectController = ParkitectController;
            this.LocationId = locationId;

            int id = 0;
            int index = 0;

            if (locationId < 0)
            {
                this.Index = -1;
                this.SerializedPanelId = $"Challenge {locationId * -1}";
                this.PanelId = "";
                return;
            }

            if (locationId < 3)
            {
                id = locationId + 1;
            } else
            {
                id = (locationId % 3) + 1;
                double i = locationId / 3;
                index = (int)Math.Floor(i);
            }

            this.Index = index;
            this.SerializedPanelId = $"Challenge {id}";
            this.PanelId = $"Challenge_{id}_{index}";
        }

        public string Text()
        {
            if (this.ParkGuest != null)
            {
                return this.ParkGuest.Text();
            }

            if (this.ParkEmployee != null)
            {
                return this.ParkEmployee.Text();
            }

            if (this.ParkMoney != null)
            {
                return this.ParkMoney.Text();
            }

            return $"Have {this.Count}x \"{this.GetShopOrAttractionName()}\"";
        }

        public string SubText()
        {
            List<string> ratings = new List<string>();

            if (this.NauseaRating != null)
            {
                ratings.Add(this.NauseaRating.SubText());
            }

            if (this.ExcitementRating != null)
            {
                ratings.Add(this.ExcitementRating.SubText());
            }

            if (this.IntensityRating != null)
            {
                ratings.Add(this.IntensityRating.SubText());
            }

            if (this.SatisfactionRating != null)
            {
                ratings.Add(this.SatisfactionRating.SubText());
            }

            if (this.GuestsRating != null)
            {
                ratings.Add(this.GuestsRating.SubText());
            }

            if (this.RevenueRating != null)
            {
                ratings.Add(this.RevenueRating.SubText());
            }

            if (this.ProfitRating != null)
            {
                ratings.Add(this.ProfitRating.SubText());
            }

            if (this.DecoRating != null)
            {
                ratings.Add(this.DecoRating.SubText());
            }

            if (this.Photo != null)
            {
                ratings.Add(this.Photo.SubText());
            }

            if (this.Voucher != null)
            {
                ratings.Add(this.Voucher.SubText());
            }

            if (this.ParkEmployee != null)
            {
                int level = this.ParkitectController.GetEmployeeExperienceLevel(this.ParkEmployee.GetEmployeePrefabs());
                return this.ColoredText(Colors.Grey, $"with Experience level: {level}");
            }

            return string.Join(" ", ratings);
        }

        private string GetShopOrAttractionName()
        {
            if (this.IsShopOrAttractionType())
            {
                if (this.Type == Constants.Stall.FacilityType)
                {
                    return Constants.Stall.FacilityTypeLabel;
                }
                return this.Type;
            }

            string thing = this.Attraction != null ? this.Attraction : this.Shop;
            return this.ParkitectController.GetSerializedFromPrefabs(thing);
        }

        private bool IsShopOrAttractionType()
        {
            return this.Attraction == null && this.Shop == null;
        }

        protected ChallengeHelper CheckNauseaRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.NauseaRating != null)
            {
                int count = attractions.Where(a => this.NauseaRating.Check(a.getNauseaRating())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.NauseaRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckExcitementRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.ExcitementRating != null)
            {
                int count = attractions.Where(a => this.ExcitementRating.Check(a.getExcitementRating())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.ExcitementRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckIntensityRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.IntensityRating != null)
            {
                int count = attractions.Where(a => this.IntensityRating.Check(a.getIntensityRating())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.IntensityRating.SubText());
            }

            return challengeHelper;
        }
        
        protected ChallengeHelper CheckSatisfactionRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.SatisfactionRating != null)
            {
                int count = attractions.Where(a => this.SatisfactionRating.Check(a.getSatisfactionRate())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.SatisfactionRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckGuestsRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.GuestsRating != null)
            {
                int count = attractions.Where(a => this.GuestsRating.Check(a.customersCount)).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.GuestsRating.SubText());
            }

            return challengeHelper;
        }
        protected ChallengeHelper CheckGuestsRating(ChallengeHelper challengeHelper, List<Shop> shops)
        {
            if (this.GuestsRating != null)
            {
                int count = shops.Where(s => this.GuestsRating.Check(s.customersCount)).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.GuestsRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckRevenueRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.RevenueRating != null)
            {
                int count = attractions.Where(a => this.RevenueRating.Check(a.getTotalRevenue())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.RevenueRating.SubText());
            }

            return challengeHelper;
        }
        protected ChallengeHelper CheckRevenueRating(ChallengeHelper challengeHelper, List<Shop> shops)
        {
            if (this.RevenueRating != null)
            {
                int count = shops.Where(s => this.RevenueRating.Check(s.getTotalRevenue())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.RevenueRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckProfitRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.ProfitRating != null)
            {
                int count = attractions.Where(a => this.ProfitRating.Check(a.getTotalProfit())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.ProfitRating.SubText());
            }

            return challengeHelper;
        }
        protected ChallengeHelper CheckProfitRating(ChallengeHelper challengeHelper, List<Shop> shops)
        {
            if (this.ProfitRating != null)
            {
                int count = shops.Where(s => this.ProfitRating.Check(s.getTotalProfit())).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.ProfitRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckDecoRating(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.DecoRating != null)
            {
                int decoCount = attractions.Where(a => this.DecoRating.Check(a.getDecoResultScore())).Count();
                challengeHelper.ValidateAndAddToCheckList(decoCount, this.DecoRating.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckPhoto(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.Photo != null)
            {
                List<Coaster> coasters = attractions
                    .OfType<Coaster>()
                        .Where(c => c.ridePhotosSold > 0)
                    .ToList();
                int count = coasters.Where(a => this.Photo.Check(a.ridePhotosSold)).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.Photo.SubText());
            }

            return challengeHelper;
        }

        protected ChallengeHelper CheckVoucher(ChallengeHelper challengeHelper, List<Attraction> attractions)
        {
            if (this.Voucher != null)
            {
                attractions = attractions.Where(c => c.vouchersRedeemed > 0).ToList();
                int count = attractions.Where(a => this.Voucher.Check(a.vouchersRedeemed)).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.Voucher.SubText());
            }

            return challengeHelper;
        }
        protected ChallengeHelper CheckVoucher(ChallengeHelper challengeHelper, List<Shop> shops)
        {
            if (this.Voucher != null)
            {
                List<ProductShop> productShops = shops
                    .OfType<ProductShop>()
                    .Where(c => c.vouchersRedeemed > 0)
                    .ToList();
                int count = productShops.Where(a => this.Voucher.Check(a.vouchersRedeemed)).Count();
                challengeHelper.ValidateAndAddToCheckList(count, this.Voucher.SubText());
            }

            return challengeHelper;
        }

        public bool Check()
        {
            Helper.Debug("[Challenge::Check]");
            Helper.Debug($"[Challenge::Check] - {this.PanelId}");

            ChallengeHelper challengeHelper = new ChallengeHelper(this.Count);

            // Must be the Type
            if (this.Type != null)
            {
                if (Constants.Attraction.Types.Contains(this.Type))
                {
                    List<Attraction> attractions = this.ParkitectController.GetAllCountableAttractionsTypeFromPark(this.Type, this.DecoRating);
                    challengeHelper.LowestCount = attractions.Count;

                    if (this.GuestsRating != null)
                    {
                        int customers = attractions.Sum(a => a.customersCount);

                        if (!this.GuestsRating.Check(customers))
                        {
                            challengeHelper.AddToChecklist(this.GuestsRating.SubText(customers));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(attractions.Count, this.GuestsRating.SubText());
                        }
                    }

                    if (this.RevenueRating != null)
                    {
                        float revenues = attractions.Sum(a => (float)a.getTotalRevenue());

                        if (!this.RevenueRating.Check(revenues))
                        {
                            challengeHelper.AddToChecklist(this.RevenueRating.SubText(revenues));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(attractions.Count, this.RevenueRating.SubText());
                        }
                    }

                    if (this.ProfitRating != null)
                    {
                        float profits = attractions.Sum(a => (float)a.getTotalProfit());

                        if (!this.ProfitRating.Check(profits))
                        {
                            challengeHelper.AddToChecklist(this.ProfitRating.SubText(profits));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(attractions.Count, this.ProfitRating.SubText());
                        }
                    }

                    if (this.Photo != null)
                    {
                        List<Coaster> coasters = attractions
                            .OfType<Coaster>()
                            .Where(c => c.ridePhotosSold > 0)
                            .ToList();
                        int photos = coasters.Sum(c => c.ridePhotosSold);

                        if (!this.Photo.Check(photos))
                        {
                            challengeHelper.AddToChecklist(this.Photo.SubText(photos));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(coasters.Count, this.Photo.SubText());
                        }
                    }

                    if (this.Voucher != null)
                    {
                        attractions = attractions.Where(c => c.vouchersRedeemed > 0).ToList();
                        int vouchers = attractions.Sum(a => a.vouchersRedeemed);

                        if (!this.Voucher.Check(vouchers))
                        {
                            challengeHelper.AddToChecklist(this.Voucher.SubText(vouchers));
                        } else 
                        {
                            challengeHelper.ValidateAndAddToCheckList(attractions.Count, this.Voucher.SubText());
                        }
                    }
                }
                else if (Constants.Stall.Types.Contains(this.Type))
                {
                    List<Shop> shops = this.ParkitectController.GetAllCountableShopsTypeFromPark(this.Type);
                    challengeHelper.LowestCount = shops.Count;

                    if (this.GuestsRating != null)
                    {
                        int customers = shops.Sum(a => a.customersCount);

                        if (!this.GuestsRating.Check(customers))
                        {
                            challengeHelper.AddToChecklist(this.GuestsRating.SubText(customers));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(shops.Count, this.GuestsRating.SubText());
                        }
                    }

                    if (this.RevenueRating != null)
                    {
                        float revenues = shops.Sum(s => (float)s.getTotalRevenue());

                        if (!this.RevenueRating.Check(revenues))
                        {
                            challengeHelper.AddToChecklist(this.RevenueRating.SubText(revenues));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(shops.Count, this.RevenueRating.SubText());
                        }
                    }

                    if (this.ProfitRating != null)
                    {
                        float profits = shops.Sum(s => (float)s.getTotalProfit());

                        if (!this.ProfitRating.Check(profits))
                        {
                            challengeHelper.AddToChecklist(this.ProfitRating.SubText(profits));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(shops.Count, this.ProfitRating.SubText());
                        }
                    }

                    if (this.Voucher != null)
                    {
                        List<ProductShop> productShops = shops
                            .OfType<ProductShop>()
                            .Where(c => c.vouchersRedeemed > 0)
                            .ToList();
                        int vouchers = productShops.Sum(a => a.vouchersRedeemed);

                        if (!this.Voucher.Check(vouchers))
                        {
                            challengeHelper.AddToChecklist(this.Voucher.SubText(vouchers));
                        }
                        else
                        {
                            challengeHelper.ValidateAndAddToCheckList(productShops.Count, this.Voucher.SubText());
                        }
                    }
                }

                if (challengeHelper.HasUnsolvedCheckList())
                {
                    challengeHelper.AddToChecklist($"Missing {challengeHelper.GetCountDifference()}x '{this.GetShopOrAttractionName()}'", true);
                }
            }
            else if (this.Attraction != null)
            {
                List<Attraction> attractions = this.ParkitectController.GetAllCountableAttractionsFromPark(this.Attraction);
                challengeHelper.LowestCount = attractions == null ? 0 : attractions.Count;

                challengeHelper = this.CheckNauseaRating(challengeHelper, attractions);
                challengeHelper = this.CheckExcitementRating(challengeHelper, attractions);
                challengeHelper = this.CheckIntensityRating(challengeHelper, attractions);
                challengeHelper = this.CheckSatisfactionRating(challengeHelper, attractions);
                challengeHelper = this.CheckGuestsRating(challengeHelper, attractions);
                challengeHelper = this.CheckRevenueRating(challengeHelper, attractions);
                challengeHelper = this.CheckProfitRating(challengeHelper, attractions);
                challengeHelper = this.CheckDecoRating(challengeHelper, attractions);
                challengeHelper = this.CheckPhoto(challengeHelper, attractions);
                challengeHelper = this.CheckVoucher(challengeHelper, attractions);

                if (attractions == null || challengeHelper.HasUnsolvedCheckList())
                {
                    string text = $"- {challengeHelper.Count - challengeHelper.LowestCount}x {this.ParkitectController.GetSerializedFromPrefabs(this.Attraction)}";
                    challengeHelper.AddToChecklist(text, true);
                }
            }
            else if (this.Shop != null)
            {
                List<Shop> shops = this.ParkitectController.GetAllCountableShopsFromPark(this.Shop);
                challengeHelper.LowestCount = shops == null ? 0 : shops.Count;

                challengeHelper = this.CheckGuestsRating(challengeHelper, shops);
                challengeHelper = this.CheckRevenueRating(challengeHelper, shops);
                challengeHelper = this.CheckProfitRating(challengeHelper, shops);
                challengeHelper = this.CheckVoucher(challengeHelper, shops);

                if (shops == null || challengeHelper.HasUnsolvedCheckList())
                {
                    string text = $"- {challengeHelper.Count - challengeHelper.LowestCount}x {this.ParkitectController.GetSerializedFromPrefabs(this.Shop)}";
                    challengeHelper.AddToChecklist(text, true);
                }
            }
            else if (this.ParkGuest != null)
            {
                int guests = this.ParkitectController.GetParkGuestCount();
                if (!this.ParkGuest.Check(guests))
                {
                    challengeHelper.AddToChecklist(this.ParkGuest.SubText(guests));
                }
            }
            else if (this.ParkEmployee != null)
            {
                int employeeCount = this.ParkitectController.GetAllCountableEmployeesFromPark(this.ParkEmployee.GetEmployeePrefabs()).Count();
                if (!this.ParkEmployee.Check(employeeCount))
                {
                    challengeHelper.AddToChecklist(this.ParkEmployee.SubText(employeeCount));
                }
            }
            else if (this.ParkMoney != null)
            {
                double playerMoney = this.ParkitectController.GetPlayerMoney();
                if (!this.ParkMoney.Check(playerMoney))
                {
                    challengeHelper.AddToChecklist(this.ParkMoney.SubText(this.ParkMoney.Amount));
                } else
                {
                    // Can pay, so do so :D
                    this.ParkitectController.PlayerRemoveMoney(this.ParkMoney.Amount);
                }
            }

            // Error :(
            if (challengeHelper.UnsolvedCheckList.Count > 0)
            {
                Helper.Debug($"[Challenge::Check] {this.Type} -> Unsolved {challengeHelper.UnsolvedCheckList.Count}");
                this.ParkitectController.SendMessage($"{this.SerializedPanelId} has missing requirements:", challengeHelper.Message());
                return false;
            }

            return true;
        }

        public void SetAttraction(string attraction, int count)
        {
            if (this.Shop != null || this.Type != null)
            {
                Helper.Debug("[Challenge::SetAttraction] -> Already defined a Shop or a Type for this challenge!");
            }

            this.Attraction = attraction;
            this.Count = count;
        }
     
        public void SetAttractionType(string type, int count)
        {
            if (this.Shop != null)
            {
                Helper.Debug("[Challenge::SetAttractionType] -> Already defined a Shop for this challenge!");
            }

            this.Attraction = null;
            this.Type = type;
            this.Count = count;
        }

        public void SetShop(string shop, int count)
        {
            if (this.Attraction != null || this.Type != null)
            {
                Helper.Debug("[Challenge::SetShop] -> Already defined an Attraction or a Type for this challenge!");
            }

            this.Shop = shop;
            this.Count = count;
        }
  
        public void SetShopType(string type, int count)
        {
            if (this.Attraction != null)
            {
                Helper.Debug("[Challenge::SetAttractionType] -> Already defined an Attraction for this challenge!");
            }

            this.Shop = null;
            this.Type = type;
            this.Count = count;
        }

        public void AddNausea (float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.NauseaRating = new NauseaRating(rating, this.Type);
        }

        public void AddExcitement (float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.ExcitementRating = new ExcitementRating(rating, this.Type);
        }

        public void AddIntensity(float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.IntensityRating = new IntensityRating(rating, this.Type);
        }

        public void AddSatisfaction(float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.SatisfactionRating = new SatisfactionRating(rating, this.Type);
        }

        public void AddGuestsRating(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.GuestsRating = new GuestsRating(amount, this.Type);
        }

        public void AddRevenueRating(float amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.RevenueRating = new RevenueRating(amount, this.Type);
        }

        public void AddProfitRating(float amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.ProfitRating = new ProfitRating(amount, this.Type);
        }

        public void AddDecoRating(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            this.DecoRating = new DecoRating(value);
        }

        public void AddPhoto(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.Photo = new PhotoRating(amount, this.Type);
        }

        public void AddVoucher(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.Voucher = new VoucherRating(amount, this.Type);
        }

        public void AddEmployee(int amount, string prefabName)
        {
            if (amount <= 0)
            {
                return;
            }

            this.ParkEmployee = new ParkEmployee(amount, prefabName);
        }
        public void AddParkGuests(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.ParkGuest = new ParkGuest(amount);
        }
        public void AddParkMoney(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.ParkMoney = new ParkMoney(amount);
        }

        public string ColoredText(string color, string text)
        {
            return $"<color={color}> {text}</color>";
        }
    }
}
