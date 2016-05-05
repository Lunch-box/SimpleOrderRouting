namespace OtherTeam.StandardizedMarketGatewayAPI
{
    public class ApiLimitOrder : ApiOrder
    {
        public decimal Price { get; private set; }

        public ApiLimitOrder(ApiMarketWay way, int quantity, decimal price, bool allowPartialExecution)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.AllowPartialExecution = allowPartialExecution;
            this.Way = way;
        }
    }
}