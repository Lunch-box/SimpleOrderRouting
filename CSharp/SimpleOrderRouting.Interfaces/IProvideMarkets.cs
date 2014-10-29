namespace SimpleOrderRouting.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IProvideMarkets
    {
        IEnumerable<IMarket> GetAvailableMarkets();
    }

    public interface IMarket
    {
    }
}