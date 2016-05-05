namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Feeds;

    public interface ICanReceiveMarketData
    {
        event EventHandler<MarketDataUpdate> InstrumentMarketDataUpdated;
        
        [Obsolete("Use the overload with market name insteads")]
        void Subscribe(IMarket market);

        void Subscribe(string marketName);
    }
}