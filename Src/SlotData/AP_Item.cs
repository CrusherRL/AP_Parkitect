using System.Linq;

namespace ArchipelagoMod.Src.SlotData
{
    internal class AP_Item
    {
        public string Name { get; set; }
        public string SerializedName { get; set; }
        public Prefabs PrefabName { get; set; }
        public int LocationId{ get; set; }
        public string Playername { get; set; }

        public bool IsTrap = false;

        public bool IsSkip = false;

        public bool IsSpeedup = false;

        public bool IsStatistic = false;

        public bool IsDeco = false;

        public bool IsMod = false;
        public string ModType = null;

        public static AP_Item Init(string name, string Playername, long locationId, string serializedName = "")
        {
            AP_Item self = new AP_Item
            {
                Name = name,
                SerializedName = serializedName,
                Playername = Playername,
                LocationId = (int)locationId - Constants.ArchipelagoBaseId
            };

            self.IsTrap = Constants.Trap.All.Contains(self.Name);
            self.IsSkip = Constants.Skips.Types.Contains(self.Name);
            self.IsSpeedup = Constants.ProgressiveSpeed.Types.Contains(self.Name);
            self.IsMod = Constants.Mods.All.Contains(self.Name);
            self.IsDeco = Constants.Decorations.All.Contains(self.Name);
            self.IsStatistic = Constants.Statistics.All.Contains(self.Name);

            Helper.Debug($"[AP_Item::Init] IsTrap - " + self.IsTrap);
            Helper.Debug($"[AP_Item::Init] IsSkip - " + self.IsSkip);
            Helper.Debug($"[AP_Item::Init] IsSpeedup - " + self.IsSpeedup);
            Helper.Debug($"[AP_Item::Init] IsMod - " + self.IsMod);
            Helper.Debug($"[AP_Item::Init] IsDecorationTheme - " + self.IsDeco);
            Helper.Debug($"[AP_Item::Init] isStatistic - " + self.IsStatistic);

            if (self.IsMod)
            {
                self.ModType = Constants.Mods.GetType(self.Name);
            }

            try
            {
                if (
                    !self.IsTrap
                    && !self.IsSkip
                    && !self.IsSpeedup
                    && !self.IsMod
                    && !self.IsDeco
                    && !self.IsStatistic
                    && !Constants.TrapLinks.Contains(self.Name)
                )
                {
                    self.PrefabName = Helper.GetPrefabsFromString(self.Name);
                }
            } catch { }

            return self;
        }

        public string Message()
        {
            if (this.IsTrap)
            {
                return "";
            }

            if (this.IsSkip)
            {
                return $"You Received a Skip {this.FromPlayerMessage()}. Use it wisely!";
            }

            if (this.IsSpeedup)
            {
                return $"Your Speedup increased {this.FromPlayerMessage()}";
            }

            string message = $"You Received \"{this.SerializedName}\" {this.FromPlayerMessage()}";

            if (this.IsDeco || this.IsStatistic)
            {
                string type = this.IsDeco ? "Decoration Theme:" : "Statistic:";
                return message.Replace("You Received", $"You Received {type}");
            }

            if (!this.IsMod)
            {
                return message;
            }

            return message + $"\nMod Items may to be researched!\nType: {this.ModType}";
        }

        private string FromPlayerMessage()
        {
            if (this.Playername == Constants.Playername)
            {
                return "";
            }

            return $"from {this.Playername}";
        }
    }
}
