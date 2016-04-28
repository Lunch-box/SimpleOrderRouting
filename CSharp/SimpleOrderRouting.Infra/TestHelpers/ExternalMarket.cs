namespace SimpleOrderRouting.Infra.TestHelpers
{
    using SimpleOrderRouting.Domain;

    public class ExternalMarket : IMarket
    {
        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }
    }
}