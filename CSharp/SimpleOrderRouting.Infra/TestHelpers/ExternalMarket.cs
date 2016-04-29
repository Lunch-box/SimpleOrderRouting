namespace SimpleOrderRouting.Infra.TestHelpers
{
    public class ExternalMarket : IMarket
    {
        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }
    }
}