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

        public static AP_Item Init (string name, string Playername, long locationId, string serializedName = "")
        {
            AP_Item self = new AP_Item ();

            self.Name = name;
            self.SerializedName = serializedName;
            self.Playername = Playername;
            self.LocationId = (int)locationId - Constants.ArchipelagoBaseId;
            self.IsTrap = Constants.Trap.All.Contains(self.Name);
            self.IsSkip = name == "Skip";

            if (!self.IsTrap && !self.IsSkip)
            {
                self.PrefabName = Helper.GetPrefabsFromString(self.Name);
            }

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
                return $"You Received a Skip{this.FromPlayerMessage()}. Use it wisely!";
            }

            return $"You Received \"{this.SerializedName}\"{this.FromPlayerMessage()}";
        }

        private string FromPlayerMessage()
        {
            if (this.Playername == Constants.Playername)
            {
                return "";
            }

            return $" from {this.Playername}";
        }
    }
}
