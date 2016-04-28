namespace SimpleOrderRouting.Domain
{
    using System.Collections.Generic;

    public interface IProvideMarkets
    {
        IEnumerable<IMarket> GetAvailableMarkets();
    }

    public interface IMarket
    {
        int SellQuantity { get; }

        decimal SellPrice { get; }
    }
}