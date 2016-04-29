namespace SimpleOrderRouting
{
    public interface IMarket
    {
        int SellQuantity { get; }

        decimal SellPrice { get; }
    }
}