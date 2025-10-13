using System.Linq;

namespace ArchipelagoMod.Src.SlotData
{
    internal class AP_Item
    {
        public string Name { get; set; }
        public string SerializedName { get; set; }
        public Prefabs PrefabName { get; set; }
        public int LocationId{ get; set; }
        public long ItemId { get; set; }

        public bool IsTrap = false;

        public static AP_Item Init (string name, long itemId, long locationId, string serializedName = "")
        {
            AP_Item self = new AP_Item ();

            self.Name = name;
            self.SerializedName = serializedName;
            self.ItemId = itemId;
            self.LocationId = (int)locationId - Constants.ArchipelagoBaseId;
            self.IsTrap = Constants.Trap.All.Contains(self.Name);

            if (!self.IsTrap )
            {
                self.PrefabName = Helper.GetPrefabsFromString(self.Name);
            }

            return self;
        }
    }
}
