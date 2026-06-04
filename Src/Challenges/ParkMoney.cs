namespace ArchipelagoMod.Src.Challenges
{
    class ParkMoney : AbstractCount
    {
        public ParkMoney(int amount) : base(amount, "") { }

        public override string Text()
        {
            return $"Pay {this.Amount}$";
        }
        public override string SubText(int count)
        {
            return $"You don't own at least {this.Amount}$";
        }
    }
}
