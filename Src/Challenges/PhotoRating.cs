namespace ArchipelagoMod.Src.Challenges
{
    class PhotoRating : AbstractRating
    {
        public override string Label { get; set; } = "Photos sold";
        public override string Color { get; set; } = Colors.Pink;

        public PhotoRating(float rating, string type = null, string currency = "") : base(rating, type, currency)
        {
            if (!string.IsNullOrEmpty(type))
            {
                this.Label = "sold Photos";
            }
        }
    }
}
