namespace ArchipelagoMod.Src.Challenges
{
    class IntensityRating : AbstractRating
    {
        public override string Label { get; set; } = "Intensity";
        public override string Color { get; set; } = "#B46A08"; // Orange

        public IntensityRating(float rating, string currency = "") : base(rating, currency) {}
    }
}
