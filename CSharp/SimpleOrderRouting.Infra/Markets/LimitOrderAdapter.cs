namespace SimpleOrderRouting.Infra
{
    using OtherTeam.StandardizedMarketGatewayAPI;

    /// <summary>
    /// Adapts LimitOrder between the SOR and the external market gateway models. 
    /// </summary>
    public class LimitOrderAdapter : OrderAdapter
    {
        private readonly ApiLimitOrder apiLimitOrder;

        public LimitOrderAdapter(ApiMarketGateway marketGateway, ApiLimitOrder apiLimitOrder) : base(marketGateway, apiLimitOrder)
        {
            this.apiLimitOrder = apiLimitOrder;
        }

        public override void Send()
        {
            this.MarketGateway.Send(this.apiLimitOrder);
        }
    }
}