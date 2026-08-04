namespace Archipelago.Src.EnergyLink
{
    class EnergyLinkItem
    {
        public enum EnergyLinkType
        {
            Withdraw,
            Deposit
        }

        public EnergyLinkType Type = EnergyLinkType.Withdraw;
        public int Money;
        public int TaxMoney;
        public int TaxedMoney;

        public EnergyLinkItem(float money, float taxMoney, EnergyLinkType type = EnergyLinkType.Withdraw)
        {
            this.Money = (int)money;
            this.TaxMoney = (int)taxMoney;
            this.TaxedMoney = (int)(money - taxMoney);
            this.Type = type;
        }

        public string Message()
        {
            string type = this.Type == EnergyLinkType.Withdraw ? "Withdraw" : "Deposit";
            return $"{type} - Money: {this.Money} - Tax: {this.TaxMoney} = Taxed Money: {this.TaxedMoney}";
        }
    }
}
