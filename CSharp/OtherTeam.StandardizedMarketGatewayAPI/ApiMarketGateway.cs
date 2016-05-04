namespace OtherTeam.StandardizedMarketGatewayAPI
{
    public class ApiMarketGateway
    {
        public ApiMarketGateway(string name, int sellQuantity, decimal sellPrice)
        {
            this.Name = name;
            this.SellQuantity = sellQuantity;
            this.SellPrice = sellPrice;
        }

        public string Name { get; private set; }

        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }


        public ApiMarketOrder CreateMarketOrder(ApiMarketWay way, int quantity)
        {
            return new ApiMarketOrder(way, quantity);
        }

        public void Send(ApiMarketOrder apiMarketOrder)
        {
            throw new System.NotImplementedException();
        }
    }
}