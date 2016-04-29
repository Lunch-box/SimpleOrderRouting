namespace SimpleOrderRouting
{
    using System.Collections.Generic;

    public interface IProvideMarkets
    {
        IEnumerable<IMarket> GetAvailableMarkets();
    }
}