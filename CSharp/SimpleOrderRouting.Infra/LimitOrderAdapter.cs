namespace SimpleOrderRouting.Infra
{
    using OtherTeam.StandardizedMarketGatewayAPI;

    public class LimitOrderAdapter : OrderAdapter
    {
        private readonly ApiLimitOrder apiLimitOrder;

        public LimitOrderAdapter(ApiMarketGateway marketGateway, ApiLimitOrder apiLimitOrder) : base(marketGateway, apiLimitOrder)
        {
            this.apiLimitOrder = apiLimitOrder;
        }

        public override void Send()
        {
            this.marketGateway.Send(this.apiLimitOrder);
        }
    }
}