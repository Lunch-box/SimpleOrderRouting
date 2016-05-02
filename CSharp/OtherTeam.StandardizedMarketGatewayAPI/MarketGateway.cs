namespace OtherTeam.StandardizedMarketGatewayAPI
{
    public class MarketGateway
    {
        public MarketGateway(string name, int sellQuantity, decimal sellPrice)
        {
            this.Name = name;
            this.SellQuantity = sellQuantity;
            this.SellPrice = sellPrice;
        }

        public string Name { get; private set; }

        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }
    }
}