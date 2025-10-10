namespace ArchipelagoMod.Src.Challenges
{
    class ProfitRating : AbstractRating
    {
        public override string Label { get; set; } = "Total Profit";
        public override string Color { get; set; } = "#D75031"; // Red

        public ProfitRating(float rating, string currency = "$") : base(rating, currency) {}
    }
}
