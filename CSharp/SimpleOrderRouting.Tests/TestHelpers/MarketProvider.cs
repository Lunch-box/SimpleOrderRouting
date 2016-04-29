namespace SimpleOrderRouting.Tests.TestHelpers
{
    using System.Collections.Generic;

    public class MarketProvider : IProvideMarkets
    {
        private readonly IEnumerable<IMarket> markets;

        public MarketProvider(IEnumerable<IMarket> markets)
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