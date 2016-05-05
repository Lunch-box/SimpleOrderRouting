namespace SimpleOrderRouting.Markets.Feeds
{
    using System;

    using SimpleOrderRouting.Markets.Orders;

    public class MarketDataUpdate
    {
        [Obsolete("Use the string version instead.")]
        public MarketDataUpdate(IMarket market, decimal price, int quantity)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.Market = market;
        }

        public MarketDataUpdate(string marketName, decimal price, int quantity)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.MarketName = marketName;
        }

        public string MarketName { get; private set; }

        public IMarket Market { get; private set; }

        public InstrumentIdentifier InstrumentIdentifier { get; private set; }

        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
    }
}