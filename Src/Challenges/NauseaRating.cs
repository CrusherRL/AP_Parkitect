namespace ArchipelagoMod.Src.Challenges
{
    class NauseaRating : AbstractRating
    {
        public override string Label { get; set; } = "Nausea";
        public override string Color { get; set; } = Colors.Gold;

        public NauseaRating(float rating, string type = null, string currency = "") : base(rating, currency) {}
    }
}
