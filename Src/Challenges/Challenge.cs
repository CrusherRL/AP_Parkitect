using ArchipelagoMod.Src.Controller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchipelagoMod.Src.Challenges
{
    class Challenge
    {
        private ParkitectController ParkitectController = null;

        [JsonProperty]
        public string SerializedPanelId = null;

        [JsonProperty]
        protected RevenueRating RevenueRating = null;
        [JsonProperty]
        protected NauseaRating NauseaRating = null;
        [JsonProperty]
        protected ExcitementRating ExcitementRating = null;
        [JsonProperty]
        protected IntensityRating IntensityRating = null;
        [JsonProperty]
        protected SatisfactionRating SatisfactionRating = null;
        [JsonProperty]
        protected GuestsRating GuestsRating = null;

        [JsonProperty]
        protected string Attraction = null;
        [JsonProperty]
        protected string Shop = null;
        [JsonProperty]
        protected string Type = null;
        [JsonProperty]
        protected int Count = 1;

        [JsonProperty]
        public int LocationId;
        [JsonProperty]
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
            return $"Have {this.Count}x \"{this.GetShopOrAttractionName()}\"";
        }

        public string SubText()
        {
            List<string> ratings = new List<string>();

            if (this.NauseaRating != null)
            {
                ratings.Add(this.NauseaRating.Text());
            }

            if (this.ExcitementRating != null)
            {
                ratings.Add(this.ExcitementRating.Text());
            }

            if (this.IntensityRating != null)
            {
                ratings.Add(this.IntensityRating.Text());
            }

            if (this.SatisfactionRating != null)
            {
                ratings.Add(this.SatisfactionRating.Text());
            }

            if (this.GuestsRating != null)
            {
                ratings.Add(this.GuestsRating.Text());
            }

            if (this.RevenueRating != null)
            {
                ratings.Add(this.RevenueRating.Text());
            }

            return string.Join(" ", ratings);
        }

        private string GetShopOrAttractionName()
        {
            string thing = this.Attraction != null ? this.Attraction : this.Shop;

            // its a type!
            if (thing == null)
            {
                thing = this.Type;
            }

           return this.ParkitectController.GetSerializedFromPrefabs(thing);
        }
       
        public bool Check()
        {
            Helper.Debug("[Challenge::Check]");
            Helper.Debug($"[Challenge::Check] - {this.PanelId}");
            int lowestCount = this.Count;
            List<string> unsolvedList = new List<string>();

            string thing = null;

            // Must be the Type
            if (this.Type != null)
            {
                thing = this.Type.ToString();
                if (Constants.Attraction.Types.Contains(this.Type))
                {
                    lowestCount = this.ParkitectController.GetAllCountableAttractionsTypeFromPark(this.Type).Count();
                }
                else if (Constants.Stall.Types.Contains(this.Type))
                {
                    lowestCount = this.ParkitectController.GetAllCountableShopsTypeFromPark(this.Type).Count();
                }

                if (lowestCount < this.Count)
                {
                    unsolvedList.Add($"Missing {this.Count - lowestCount}x '{ this.GetShopOrAttractionName() }'");
                }
            }
            else if (this.Attraction != null)
            {
                thing = this.Attraction.ToString();
                List <Attraction> attractions = this.ParkitectController.GetAllCountableAttractionsFromPark(this.Attraction);
                lowestCount = attractions == null ? 0 : attractions.Count;

                if (this.NauseaRating != null)
                {
                    int nauseaCount = attractions.Where(a => this.NauseaRating.Check(a.getNauseaRating())).Count();
                    if (lowestCount == 0 || nauseaCount < this.Count)
                    {
                        lowestCount = lowestCount > nauseaCount ? nauseaCount : lowestCount;
                        unsolvedList.Add(this.NauseaRating.Text());
                    }
                }
                    
                if (this.ExcitementRating != null)
                {
                    int excitementCount = attractions.Where(a => this.ExcitementRating.Check(a.getExcitementRating())).Count();
                    if (lowestCount == 0 || excitementCount < this.Count)
                    {
                        lowestCount = lowestCount > excitementCount ? excitementCount : lowestCount;
                        unsolvedList.Add(this.ExcitementRating.Text());
                    }
                }
                
                if (this.IntensityRating != null)
                {
                    int intensityCount = attractions.Where(a => this.IntensityRating.Check(a.getIntensityRating())).Count();
                    if (lowestCount == 0 || intensityCount < this.Count)
                    {
                        lowestCount = lowestCount > intensityCount ? intensityCount : lowestCount;
                        unsolvedList.Add(this.IntensityRating.Text());
                    }
                }
                
                if (this.SatisfactionRating != null)
                {
                    int satisfactionCount = attractions.Where(a => this.SatisfactionRating.Check(a.getSatisfactionRate())).Count();
                    if (lowestCount == 0 || satisfactionCount < this.Count)
                    {
                        lowestCount = lowestCount > satisfactionCount ? satisfactionCount : lowestCount;
                        unsolvedList.Add(this.SatisfactionRating.Text());
                    }
                }

                if (this.GuestsRating != null)
                {
                    int guestCount = attractions.Where(a => this.GuestsRating.Check(a.customersCount)).Count();
                    if (lowestCount == 0 || guestCount < this.Count)
                    {
                        lowestCount = lowestCount > guestCount ? guestCount : lowestCount;
                        unsolvedList.Add(this.GuestsRating.Text());
                    }
                }
                
                if (this.RevenueRating != null)
                {
                    int revenue = attractions.Where(s => this.RevenueRating.Check(s.getTotalRevenue())).Count();
                    if (lowestCount == 0 || revenue < this.Count)
                    {
                        lowestCount = lowestCount > revenue ? revenue : lowestCount;
                        unsolvedList.Add(this.RevenueRating.Text());
                    }
                }

                if (attractions == null || lowestCount < this.Count)
                {
                    Helper.Debug(this.ParkitectController.GetSerializedFromPrefabs(this.Attraction));
                    unsolvedList.Insert(0, $"- {this.Count - lowestCount}x {this.ParkitectController.GetSerializedFromPrefabs(this.Attraction)}");
                }
            }
            else if (this.Shop != null)
            {
                thing = this.Shop.ToString();
                List<Shop> shops = this.ParkitectController.GetAllCountableShopsFromPark(this.Shop);
                lowestCount = shops.Count;

                if (this.GuestsRating != null)
                {
                    int guestCount = shops.Where(s => this.GuestsRating.Check(s.customersCount)).Count();
                    if (lowestCount == 0 || guestCount < this.Count)
                    {
                        lowestCount = lowestCount > guestCount ? guestCount : lowestCount;
                        unsolvedList.Add(this.GuestsRating.Text());
                    }
                }

                if (this.RevenueRating != null)
                {
                    int revenue = shops.Where(s => this.RevenueRating.Check(s.getTotalRevenue())).Count();
                    if (lowestCount == 0 || revenue < this.Count)
                    {
                        lowestCount = lowestCount > revenue ? revenue : lowestCount;
                        unsolvedList.Add(this.RevenueRating.Text());
                    }
                }

                if (shops == null || lowestCount < this.Count)
                {
                    unsolvedList.Insert(0, $"- {this.Count - lowestCount}x {this.ParkitectController.GetSerializedFromPrefabs(this.Shop)}");
                }
            }

            // Error :(
            if (unsolvedList.Count > 0)
            {
                Helper.Debug($"[Challenge::Check] {this.Type} -> Unsolved {unsolvedList.Count}");
                string list = string.Join("\n", unsolvedList);
                this.ParkitectController.SendMessage($"{this.SerializedPanelId} has missing requirements:", list);
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

            this.NauseaRating = new NauseaRating(rating);
        }

        public void AddExcitement (float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.ExcitementRating = new ExcitementRating(rating);
        }

        public void AddIntensity(float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.IntensityRating = new IntensityRating(rating);
        }

        public void AddSatisfaction(float rating)
        {
            if (rating <= 0f)
            {
                return;
            }

            this.SatisfactionRating = new SatisfactionRating(rating);
        }

        public void AddGuestsRating(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.GuestsRating = new GuestsRating(amount);
        }
     
        public void AddRevenueRating(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            this.RevenueRating = new RevenueRating(amount);
        }

        // Typed DTO for export
        public class ChallengeExport
        {
            public string SerializedPanelId { get; set; }
            public float? RevenueRating { get; set; }
            public float? NauseaRating { get; set; }
            public float? ExcitementRating { get; set; }
            public float? IntensityRating { get; set; }
            public float? SatisfactionRating { get; set; }
            public float? GuestsRating { get; set; }
            public string Attraction { get; set; }
            public string Shop { get; set; }
            public string Type { get; set; }
            public int? Count { get; set; }
            public int LocationId { get; set; }
            public string PanelId { get; set; }
        }
    }
}
