namespace ArchipelagoMod.Src.Challenges
{
    class SatisfactionRating : AbstractRating
    {
        public override string Label { get; set; } = "Profit Last Month";
        public override string Color { get; set; } = "#AC62C3"; // Purple

        public SatisfactionRating(float rating, string currency = "") : base(rating, currency) {}
    }
}
