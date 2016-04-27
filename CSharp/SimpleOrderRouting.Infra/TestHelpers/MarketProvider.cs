namespace SimpleOrderRouting.Infra.TestHelpers
{
    using System.Collections.Generic;

    using SimpleOrderRouting.Domain;

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