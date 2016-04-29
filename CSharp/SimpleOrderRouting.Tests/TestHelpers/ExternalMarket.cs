namespace SimpleOrderRouting.Tests.TestHelpers
{
    public class ExternalMarket : IMarket
    {
        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }
    }
}