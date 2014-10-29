namespace SimpleOrderRouting.Journey1.TestHelpers
{
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Interfaces;

    public class MarketProvider : IProvideMarkets
    {
        private readonly Market[] markets;

        public MarketProvider(Market[] markets)
        {
            this.markets = markets;
        }

        public IEnumerable<IMarket> GetAvailableMarkets()
        {
            return this.markets.Select(m => m.ExternalMarket);
        }
    }
}