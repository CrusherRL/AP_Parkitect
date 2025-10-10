namespace ArchipelagoMod.Src.Challenges
{
    class ExcitementRating : AbstractRating
    {
        public override string Label { get; set; } = "Excitement";
        public override string Color { get; set; } = "#66B508"; // Green

        public ExcitementRating(float rating, string currency = "") : base(rating, currency) {}
    }
}
