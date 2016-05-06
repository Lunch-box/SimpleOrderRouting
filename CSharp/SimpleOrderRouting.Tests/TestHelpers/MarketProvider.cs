namespace SimpleOrderRouting.Tests.TestHelpers
{
    using System;
    using System.Collections.Generic;

    [Obsolete("Replace it with the MarketGatewayAdapter.")]
    public class MarketProvider : IProvideMarkets
    {
        private readonly IEnumerable<IMarket> markets;
        private readonly IEnumerable<string> marketNames;

        public MarketProvider(IEnumerable<string> marketNames)
        {
            this.marketNames = marketNames;
        }

        [Obsolete("use the string version instead.")]
        public MarketProvider(IEnumerable<IMarket> markets)
        {
            this.markets = markets;
        }

        public IEnumerable<string> GetAvailableMarketNames()
        {
            return this.marketNames;
        }

        public IEnumerable<IMarket> GetAvailableMarkets()
        {
            // TODO : here we should have a translation between externa context and out MarketInfos domain object.
            return this.markets;
        }
    }
}