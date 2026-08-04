namespace ArchipelagoMod.Src.Challenges
{
    class ExcitementRating : AbstractRating
    {
        public override string Label { get; set; } = "Excitement";
        public override string Color { get; set; } = Colors.LightGreen;

        public ExcitementRating(float rating, string type = null, string currency = "") : base(rating, currency) {}
    }
}
