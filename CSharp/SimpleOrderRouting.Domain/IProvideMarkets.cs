namespace SimpleOrderRouting
{
    using System;
    using System.Collections.Generic;

    public interface IProvideMarkets
    {
        [Obsolete("Should use the GetAvailableMarketNames() method instead.")]
        IEnumerable<IMarket> GetAvailableMarkets();
        
        IEnumerable<string> GetAvailableMarketNames();
    }
}