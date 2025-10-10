namespace ArchipelagoMod.Src.Challenges
{
    abstract class AbstractRating
    {
        public abstract string Color { get; set; }
        public abstract string Label { get; set; }

        public float Rating = 0f;
        public string Currency = "";

        public AbstractRating (float rating, string currency = "")
        {
            this.Rating = rating;
            this.Currency = currency;
        }

        public string Text ()
        {
            return $"<color={ this.Color }> > {this.Rating}{this.Currency} {this.Label}</color>";
        }

        public bool Check(float rating)
        {
            if (rating <= 1f)
            {
                rating *= 100f;
            }

            return rating >= this.Rating;
        }

        public bool Check(double rating)
        {
            if (rating <= 1f)
            {
                rating *= 100f;
            }

            return rating >= this.Rating;
        }

        public bool Check(int amount)
        {
            return amount >= this.Rating;
        }
    }
}
