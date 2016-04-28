namespace SimpleOrderRouting.Domain
{
    using System.Collections.Generic;

    public interface IProvideMarkets
    {
        IEnumerable<IMarket> GetAvailableMarkets();
    }
}