namespace ArchipelagoMod.Src.Challenges
{
    class VoucherRating : AbstractRating
    {
        public override string Label { get; set; } = "Voucher redeemed";
        public override string Color { get; set; } = Colors.DarkGreen;

        public VoucherRating(float rating, string type = null, string currency = "") : base(rating, type, currency)
        {
            if (!string.IsNullOrEmpty(type))
            {
                this.Label = "redeemed Vouchers";
            }
        }
    }
}
