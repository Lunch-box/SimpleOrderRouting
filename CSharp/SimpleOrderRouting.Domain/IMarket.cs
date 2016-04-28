namespace SimpleOrderRouting.Domain
{
    public interface IMarket
    {
        int SellQuantity { get; }

        decimal SellPrice { get; }
    }
}