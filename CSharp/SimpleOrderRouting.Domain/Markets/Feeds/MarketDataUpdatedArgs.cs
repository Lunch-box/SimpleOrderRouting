namespace SimpleOrderRouting.Markets.Feeds
{
    using System;
    using SimpleOrderRouting.Markets.Orders;

    public class MarketDataUpdatedArgs : EventArgs
    {
        public MarketDataUpdatedArgs(string marketName, decimal price, int quantity)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.MarketName = marketName;
        }

        public string MarketName { get; private set; }

        // TODO : introduce the concept of various instruments
        public InstrumentIdentifier InstrumentIdentifier { get; private set; }

        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
    }
}