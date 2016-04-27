namespace SimpleOrderRouting.Infra.TestHelpers
{
    using System;
    using System.Linq;

    using SimpleOrderRouting.Domain;

    public class MarketDataProvider : ICanReceiveMarketData
    {
        private readonly Market[] marketsInvolved;

        public MarketDataProvider(Market[] marketsInvolved)
        {
            this.marketsInvolved = marketsInvolved;
        }

        public event EventHandler<MarketDataUpdateDto> InstrumentMarketDataUpdated;

        public void Subscribe(IMarket market)
        {
            var internalMarket = this.marketsInvolved.First(m => m == market);
            
            var onInstrumentMarketDataUpdated = this.InstrumentMarketDataUpdated;
            if (onInstrumentMarketDataUpdated != null)
            {
                onInstrumentMarketDataUpdated(this, new MarketDataUpdateDto(market, internalMarket.SellPrice, internalMarket.SellQuantity));
            }
        }
    }
}