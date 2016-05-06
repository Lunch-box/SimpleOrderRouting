namespace SimpleOrderRouting.Infra
{
    using System;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Base class for the <see cref="LimitOrderAdapter"/> and the <see cref="MarketOrderAdapter"/>.
    /// </summary>
    public abstract class OrderAdapter : IOrder
    {
        protected readonly ApiMarketGateway MarketGateway;

        private readonly ApiOrder apiOrder;

        protected OrderAdapter(ApiMarketGateway marketGateway, ApiOrder apiOrder)
        {
            this.apiOrder = apiOrder;
            this.MarketGateway = marketGateway;
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
    }
}