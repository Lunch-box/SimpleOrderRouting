namespace OtherTeam.StandardizedMarketGatewayAPI
{
    public class ApiMarketOrder
    {
        public int Quantity { get; private set; }

        public ApiMarketWay Way { get; private set; }

        public bool AllowPartialExecution
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

        void Send()
        {
            
        }
    }
}