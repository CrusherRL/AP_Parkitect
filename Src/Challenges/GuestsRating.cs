namespace ArchipelagoMod.Src.Challenges
{
    class GuestsRating : AbstractRating
    {
        public override string Label { get; set; } = "Total Guests";
        public override string Color { get; set; } = "#07B0BA"; // Blue

        public GuestsRating(float rating, string currency = "") : base(rating, currency) {}
    }
}
