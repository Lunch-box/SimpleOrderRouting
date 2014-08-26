namespace SimpleOrderRouting.Tests
{
    using System;

    using NFluent;

    using SimpleOrderRouting.Journey1;

    using Xunit;

    public class MarketTests
    {
        [Fact]
        public void MarketOrderShouldDecreaseAvailableQuantity()
        {
            var market = new Market(){ SellPrice = 100M, SellQuantity = 50};

            var order = market.CreateMarketOrder(Way.Buy, quantity: 10);
            market.Send(order);

            Check.That(market.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void LargeMarketOrderShouldTriggerException()
        {
            var market = new Market() { SellPrice = 100M, SellQuantity = 50 };

            var order = market.CreateMarketOrder(Way.Buy, quantity: 100);
            Check.ThatCode(() => market.Send(order)).Throws<ApplicationException>();
        }

        [Fact]
        public void MarketOrderShouldCaptureExec()
        {
            var market = new Market() { SellPrice = 100M, SellQuantity = 50 };
            var executed = false;
            var order = market.CreateMarketOrder(Way.Buy, quantity: 10);

            market.OrderExecuted += (s, a) => executed = true;
            market.Send(order);

            Check.That(executed).IsTrue();
            Check.That(market.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void LimitOrderShouldCaptureExec()
        {
            var market = new Market() { SellPrice = 100M, SellQuantity = 50 };
            var executed = false;
            var order = market.CreateLimitOrder(Way.Buy, price : 100M, quantity: 10);

            market.OrderExecuted += (s, a) => executed = true;
            market.Send(order);

            Check.That(executed).IsTrue();
            Check.That(market.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void LimitOrderShouldFailIfPriceTooHigh()
        {
            var market = new Market() { SellPrice = 100M, SellQuantity = 50 };
            var executed = false;
            var order = market.CreateLimitOrder(Way.Buy, price : 101M, quantity: 10);

            market.OrderExecuted += (s, a) => executed = true;
            market.Send(order);

            Check.That(executed).IsFalse();
            Check.That(market.SellQuantity).IsEqualTo(50);
        }
    }
}
