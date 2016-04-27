namespace SimpleOrderRouting.Domain
{
    using System;

    using SimpleOrderRouting.Domain.Order;

    public interface ICanReceiveMarketData
    {
        event EventHandler<MarketDataUpdateDto> InstrumentMarketDataUpdated;
        void Subscribe(IMarket market);
    }

    public class MarketDataUpdateDto
    {
        public MarketDataUpdateDto(IMarket market, decimal price, int quantity)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.Market = market;
        }

        public IMarket Market { get; private set; }

        public InstrumentIdentifier InstrumentIdentifier { get; private set; }

        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
    }
}