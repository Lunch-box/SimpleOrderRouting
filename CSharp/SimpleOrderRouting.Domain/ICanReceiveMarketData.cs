namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Feeds;

    public interface ICanReceiveMarketData
    {
        event EventHandler<MarketDataUpdate> InstrumentMarketDataUpdated;
        void Subscribe(IMarket market);
    }
}