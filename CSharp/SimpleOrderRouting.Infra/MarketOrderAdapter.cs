namespace SimpleOrderRouting.Infra
{
    using System;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.Markets.Orders;

    public class MarketOrderAdapter : IOrder
    {
        private readonly ApiMarketGateway marketGateway;

        private readonly ApiMarketOrder apiMarketOrder;

        public MarketOrderAdapter(ApiMarketGateway marketGateway, ApiMarketOrder apiMarketOrder)
        {
            this.marketGateway = marketGateway;
            this.apiMarketOrder = apiMarketOrder;
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        public Way Way
        {
            get
            {
                return (this.apiMarketOrder.Way == ApiMarketWay.Sell) ? Way.Sell : Way.Buy;
            }
        }

        public int Quantity { get { return this.apiMarketOrder.Quantity;} }

        public bool AllowPartialExecution
        {
            get
            {
                return false;
            }
        }

        public Market Market { get; private set; }

        public void Send()
        {
            this.marketGateway.Send(apiMarketOrder);
        }
    }
}