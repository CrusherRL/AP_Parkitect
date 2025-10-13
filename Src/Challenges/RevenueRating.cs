namespace ArchipelagoMod.Src.Challenges
{
    class RevenueRating : AbstractRating
    {
        public override string Label { get; set; } = "Total Revenue";
        public override string Color { get; set; } = "#D75031"; // Red

        public RevenueRating(float rating, string currency = "$") : base(rating, currency) {}
    }
}
