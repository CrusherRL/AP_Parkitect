namespace ArchipelagoMod.Src.Challenges
{
    class RevenueRating : AbstractRating
    {
        public override string Label { get; set; } = "Total Revenue";
        public override string Color { get; set; } = Colors.BloodOrange;

        public RevenueRating(float rating, string type = null, string currency = "$") : base(rating, type, currency) {}
    }
}
