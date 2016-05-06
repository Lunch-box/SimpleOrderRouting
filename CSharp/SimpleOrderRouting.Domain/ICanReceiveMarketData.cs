namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Feeds;

    public interface ICanReceiveMarketData
    {
        event EventHandler<MarketDataUpdatedArgs> InstrumentMarketDataUpdated;
        
        void Subscribe(string marketName);
    }
}