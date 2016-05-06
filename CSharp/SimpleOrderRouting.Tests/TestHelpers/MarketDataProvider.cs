namespace SimpleOrderRouting.Tests.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Markets.Feeds;

    [Obsolete("Replace it with the MarketGatewayAdapter.")]
    public class MarketDataProvider : ICanReceiveMarketData
    {
        private readonly IEnumerable<IMarket> marketsInvolved;

        private readonly IEnumerable<string> involvedMarketNames;

        public MarketDataProvider(IEnumerable<string> involvedMarketNames)
        {
            this.involvedMarketNames = involvedMarketNames;
        }

        [Obsolete("Use the string version instead.")]
        public MarketDataProvider(IEnumerable<IMarket> marketsInvolved)
        {
            this.marketsInvolved = marketsInvolved;
        }

        public event EventHandler<MarketDataUpdatedArgs> InstrumentMarketDataUpdated;

        public void Subscribe(string marketName)
        {
            var internalMarket = this.involvedMarketNames.First(m => m == marketName);

            //var onInstrumentMarketDataUpdated = this.InstrumentMarketDataUpdated;
            //if (onInstrumentMarketDataUpdated != null)
            //{
            //    onInstrumentMarketDataUpdated(this, new MarketDataUpdatedArgs(marketName, internalMarket.SellPrice, internalMarket.SellQuantity));
            //}
        }
    }
}