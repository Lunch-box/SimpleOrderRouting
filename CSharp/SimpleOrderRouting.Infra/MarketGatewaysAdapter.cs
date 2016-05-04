namespace SimpleOrderRouting.Infra
{
    using System;
    using System.Collections.Generic;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets.Feeds;
    using SimpleOrderRouting.Markets.Orders;

    public class MarketGatewaysAdapter : ICanRouteOrders, ICanReceiveMarketData, IProvideMarkets
    {
        private Dictionary<string, ApiMarketGateway> gateways;

        // legacy
        // ICanRouteOrders canRouteOrders = null;//new OrderRoutingService(marketsInvolved);
        //ICanReceiveMarketData canReceiveMarketData = null; //new MarketDataProvider(marketsInvolved);
        //IProvideMarkets provideMarkets = null; //new MarketProvider(marketsInvolved);
            
        public MarketGatewaysAdapter(ApiMarketGateway[] apiMarketGateways)
        {
            foreach (var marketGateway in apiMarketGateways)
            {
                this.gateways[marketGateway.Name] = marketGateway;
            }
        }

        public event EventHandler<MarketDataUpdate> InstrumentMarketDataUpdated;

        public void Subscribe(IMarket market)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMarket> GetAvailableMarkets()
        {
            throw new NotImplementedException();
        }

        #region ICanRouteOrders

        public IOrder CreateMarketOrder(OrderDescription orderDescription)
        {
            // adapt from the SOR model to the external market gateway one
            var marketGateway = this.gateways[orderDescription.TargetMarketName];

            ApiMarketWay apiMarketWay = (orderDescription.OrderWay == Way.Sell) ? ApiMarketWay.Sell : ApiMarketWay.Buy;
            ApiMarketOrder apiMarketOrder = marketGateway.CreateMarketOrder(apiMarketWay, orderDescription.Quantity);

            return new MarketOrderAdapter(marketGateway, apiMarketOrder);
        }

        public IOrder CreateLimitOrder(OrderDescription orderDescription)
        {
            throw new NotImplementedException();
        }

        public void Route(IOrder order)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}