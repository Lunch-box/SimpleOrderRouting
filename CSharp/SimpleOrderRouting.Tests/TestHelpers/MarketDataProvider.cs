namespace SimpleOrderRouting.Tests.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Markets.Feeds;

    public class MarketDataProvider : ICanReceiveMarketData
    {
        private readonly IEnumerable<IMarket> marketsInvolved;

        public MarketDataProvider(IEnumerable<IMarket> marketsInvolved)
        {
            this.marketsInvolved = marketsInvolved;
        }

        public event EventHandler<MarketDataUpdate> InstrumentMarketDataUpdated;

        public void Subscribe(IMarket market)
        {
            var internalMarket = this.marketsInvolved.First(m => m == market);
            
            var onInstrumentMarketDataUpdated = this.InstrumentMarketDataUpdated;
            if (onInstrumentMarketDataUpdated != null)
            {
                onInstrumentMarketDataUpdated(this, new MarketDataUpdate(market, internalMarket.SellPrice, internalMarket.SellQuantity));
            }
        }
    }
}