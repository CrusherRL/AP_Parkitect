namespace ArchipelagoMod.Src.Challenges
{
    abstract class AbstractCount
    {
        public string Label { get; set; }
        public int Amount = 0;

        public AbstractCount(int amount, string label) 
        {
            this.Amount = amount;
            this.Label = label;
        }

        public virtual string Text()
        {
            string addition = "";
            if (this.Label == "Handyman")
            {
                addition += "/Hauler";
            }
            return $"Have at least {this.Amount} {this.Label}{addition} in your Park";
        }

        public virtual string SubText(int count)
        {
            return $"Missing {this.Amount - count} {this.Label}";
        }

        public bool Check(int amount)
        {
            return amount >= this.Amount;
        }

        public bool Check(double amount)
        {
            return amount >= this.Amount;
        }
    }
}
