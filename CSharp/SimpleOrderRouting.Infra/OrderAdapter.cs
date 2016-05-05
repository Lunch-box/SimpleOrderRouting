namespace SimpleOrderRouting.Infra
{
    using System;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.Markets.Orders;

    public abstract class OrderAdapter : IOrder
    {
        protected ApiMarketGateway marketGateway;

        protected readonly ApiOrder apiOrder;

        protected OrderAdapter(ApiMarketGateway marketGateway, ApiOrder apiOrder)
        {
            this.apiOrder = apiOrder;
            this.marketGateway = marketGateway;
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        public Way Way
        {
            get
            {
                return (this.apiOrder.Way == ApiMarketWay.Sell) ? Way.Sell : Way.Buy;
            }
        }

        public int Quantity { get { return this.apiOrder.Quantity; } }

        public abstract void Send();

        public bool AllowPartialExecution
        {
            get
            {
                return false;
            }
        }

        public Market Market { get; private set; }
    }
}