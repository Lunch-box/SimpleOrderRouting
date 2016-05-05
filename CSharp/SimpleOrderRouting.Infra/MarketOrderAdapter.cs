namespace SimpleOrderRouting.Infra
{
    using OtherTeam.StandardizedMarketGatewayAPI;

    public class MarketOrderAdapter : OrderAdapter
    {
        private readonly ApiMarketOrder apiMarketOrder;

        public MarketOrderAdapter(ApiMarketGateway marketGateway, ApiMarketOrder apiMarketOrder) : base(marketGateway, apiMarketOrder)
        {
            this.apiMarketOrder = apiMarketOrder;
        }

        public override void Send()
        {
            this.marketGateway.Send(apiMarketOrder);
        }
    }
}