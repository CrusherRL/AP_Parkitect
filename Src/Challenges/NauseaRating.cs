namespace ArchipelagoMod.Src.Challenges
{
    class NauseaRating : AbstractRating
    {
        public override string Label { get; set; } = "Nausea";
        public override string Color { get; set; } = "#8C9327"; // Yellow

        public NauseaRating(float rating, string currency = "") : base(rating, currency) {}
    }
}
