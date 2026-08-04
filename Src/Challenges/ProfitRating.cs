namespace ArchipelagoMod.Src.Challenges
{
    class ProfitRating : AbstractRating
    {
        public override string Label { get; set; } = "Total Profit";
        public override string Color { get; set; } = Colors.LightRed;

        public ProfitRating(float rating, string type = null, string currency = "$") : base(rating, type, currency) {}
    }
}
