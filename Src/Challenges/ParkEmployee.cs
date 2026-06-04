namespace ArchipelagoMod.Src.Challenges
{
    class ParkEmployee : AbstractCount
    {
        public ParkEmployee(int amount, string prefabName) : base(amount, prefabName) { }

        public Prefabs GetEmployeePrefabs()
        {
            return Helper.GetPrefabsFromString(this.Label);
        }
    }
}
