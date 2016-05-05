namespace OtherTeam.StandardizedMarketGatewayAPI
{
    public class ApiMarketOrder : ApiOrder
    {
        public new bool AllowPartialExecution
        {
            get
            {
                return false;
            }
        }

        public ApiMarketOrder(ApiMarketWay way, int quantity)
        {
            this.Quantity = quantity;
            this.Way = way;
        }
    }
}