namespace SimpleOrderRouting.Infra
{
    using System;
    using System.Collections.Generic;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets.Orders;

    public sealed class OrderRoutingAdapter : ICanRouteOrders
    {
        private IReadOnlyDictionary<string, ApiMarketGateway> gateways;

        public OrderRoutingAdapter(IReadOnlyDictionary<string, ApiMarketGateway> gateways)
        {
            this.gateways = gateways;
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        
        public IOrder CreateMarketOrder(OrderDescription orderDescription)
        {
            // Adapts from the SOR model to the external market gateway one
            var marketGateway = this.gateways[orderDescription.TargetMarketName];

            ApiMarketWay apiMarketWay = (orderDescription.OrderWay == Way.Sell) ? ApiMarketWay.Sell : ApiMarketWay.Buy;
            ApiMarketOrder apiMarketOrder = marketGateway.CreateMarketOrder(apiMarketWay, orderDescription.Quantity);

            return new MarketOrderAdapter(marketGateway, apiMarketOrder);
        }

        public IOrder CreateLimitOrder(OrderDescription orderDescription)
        {
            var marketGateway = this.gateways[orderDescription.TargetMarketName];
            ApiMarketWay apiMarketWay = (orderDescription.OrderWay == Way.Sell) ? ApiMarketWay.Sell : ApiMarketWay.Buy;
            ApiLimitOrder apiLimitOrder = marketGateway.CreateLimitOrder(apiMarketWay, orderDescription.Quantity, orderDescription.OrderPrice, orderDescription.AllowPartialExecution);

            return new LimitOrderAdapter(marketGateway, apiLimitOrder);
        }

        public void Route(OrderBasket basketOrder)
        {
            basketOrder.Send();
        }
    }
}