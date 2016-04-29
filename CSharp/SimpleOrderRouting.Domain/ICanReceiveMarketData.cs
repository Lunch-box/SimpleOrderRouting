namespace SimpleOrderRouting
{
    using System;

    public interface ICanReceiveMarketData
    {
        event EventHandler<MarketDataUpdate> InstrumentMarketDataUpdated;
        void Subscribe(IMarket market);
    }
}