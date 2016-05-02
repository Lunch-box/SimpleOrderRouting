namespace SimpleOrderRouting.Infra
{
    using System;
    using System.Collections.Generic;

    using OtherTeam.StandardizedMarketGatewayAPI;
    using SimpleOrderRouting.Markets.Feeds;

    public class MarketGatewaysAdapter : ICanRouteOrders, ICanReceiveMarketData, IProvideMarkets
    {
        // legacy
        // ICanRouteOrders canRouteOrders = null;//new OrderRoutingService(marketsInvolved);
        //ICanReceiveMarketData canReceiveMarketData = null; //new MarketDataProvider(marketsInvolved);
        //IProvideMarkets provideMarkets = null; //new MarketProvider(marketsInvolved);
            
        public MarketGatewaysAdapter(MarketGateway[] marketGateways)
        {
            throw new NotImplementedException();
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
    }
}