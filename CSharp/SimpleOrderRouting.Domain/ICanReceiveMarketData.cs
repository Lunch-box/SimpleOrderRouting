namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Feeds;

    /// <summary>
    /// Allows to receive MarketData events.
    /// </summary>
    public interface ICanReceiveMarketData
    {
        event EventHandler<MarketDataUpdatedArgs> InstrumentMarketDataUpdated;
        
        void Subscribe(string marketName);
    }
}