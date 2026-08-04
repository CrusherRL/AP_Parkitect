namespace ArchipelagoMod.Src.Challenges
{
    class GuestsRating : AbstractRating
    {
        public override string Label { get; set; } = "Total Guests";
        public override string Color { get; set; } = Colors.Cyan;

        public GuestsRating(float rating, string type = null, string currency = "") : base(rating, type, currency) {}
    }
}
