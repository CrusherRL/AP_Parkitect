namespace ArchipelagoMod.Src.Challenges
{
    class IntensityRating : AbstractRating
    {
        public override string Label { get; set; } = "Intensity";
        public override string Color { get; set; } = Colors.LightOrange;

        public IntensityRating(float rating, string type = null, string currency = "") : base(rating, type, currency) {}
    }
}
