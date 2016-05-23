namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Feeds;
    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Allows to receive MarketData events.
    /// </summary>
    public interface ICanReceiveMarketData
    {

        /// <summary>
        /// Occurs when market data is updated for an Instrument.
        /// </summary>
        event EventHandler<MarketDataUpdatedArgs> InstrumentMarketDataUpdated;

        /// <summary>
        /// Occurs when one order failed on a Market.
        /// </summary>
        event EventHandler<OrderFailedEventArgs> OrderFailedOnAMarket;
        
        void Subscribe(string marketName);
    }
}