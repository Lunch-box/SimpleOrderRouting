namespace SimpleOrderRouting
{
    using System.Collections.Generic;

    /// <summary>
    /// Lists the names of available markets.
    /// </summary>
    public interface IProvideMarkets
    {
        IEnumerable<string> GetAvailableMarketNames();
    }
}