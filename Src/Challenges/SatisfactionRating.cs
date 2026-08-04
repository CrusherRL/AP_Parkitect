namespace ArchipelagoMod.Src.Challenges
{
    class SatisfactionRating : AbstractRating
    {
        public override string Label { get; set; } = "Satisfaction";
        public override string Color { get; set; } = Colors.Purple;

        public SatisfactionRating(float rating, string type = null, string currency = "") : base(rating, type, currency) {}
    }
}
