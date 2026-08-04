namespace ArchipelagoMod.Src.Challenges
{
    abstract class AbstractRating
    {
        public abstract string Color { get; set; }
        public abstract string Label { get; set; }
        public string Type = null;

        public float Rating = 0f;
        public string Currency = "";

        public AbstractRating (float rating, string type = "", string currency = "")
        {
            this.Rating = rating;
            this.Type = type;
            this.Currency = currency;
        }

        public string SubText(float count = 0f)
        {
            string text = $">= {this.Rating}{this.Currency} {this.Label}";

            if (!string.IsNullOrEmpty(this.Type))
            {
                if (count > 0f)
                {
                    text = $"Missing {this.Rating - count}{this.Currency} {this.Label}";
                }
                else
                {
                    text = $"- All \"{this.Type}\" {this.Label}: {this.Rating}{this.Currency}";
                }
            }

            return $"<color={this.Color}> {text}</color>";
        }

        public bool Check(float rating)
        {
            if (rating < 2f)
            {
                rating *= 100f;
            }

            return rating >= this.Rating;
        }

        public bool Check(double rating)
        {
            if (rating < 2f)
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
