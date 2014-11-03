namespace SimpleOrderRouting.Journey1.TestHelpers
{
    using System.Collections.Generic;

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
            // TODO : here we should have a translation between externa context and out Markets domain object.
            return this.markets;
        }
    }
}