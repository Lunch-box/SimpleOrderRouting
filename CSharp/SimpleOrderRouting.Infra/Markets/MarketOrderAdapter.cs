namespace SimpleOrderRouting.Infra
{
    using OtherTeam.StandardizedMarketGatewayAPI;

    /// <summary>
    /// Adapts MarketOrder between the SOR and the external market gateway models. 
    /// </summary>
    public class MarketOrderAdapter : OrderAdapter
    {
        private readonly ApiMarketOrder apiMarketOrder;

        public MarketOrderAdapter(ApiMarketGateway marketGateway, ApiMarketOrder apiMarketOrder) : base(marketGateway, apiMarketOrder)
        {
            this.apiMarketOrder = apiMarketOrder;
        }

        public override void Send()
        {
            this.MarketGateway.Send(apiMarketOrder);
        }
    }
}