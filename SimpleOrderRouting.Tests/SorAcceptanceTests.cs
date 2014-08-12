namespace SimpleOrderRouting.Tests
{
    using System;

    using NSubstitute;

    using Xunit;

    public class SorAcceptanceTests
    {
        #region Public Methods and Operators

        [Fact]
        public void ShouldSendOrderWhenExecutionStrategyIsRequested()
        {
            var market = Substitute.For<IMarket>();

            var sut = new SimpleOrderRoutingSystem(market);
            sut.Post(new OrderRequest());

            market.ReceivedWithAnyArgs(1).Send(null);
        }

        //[Fact]
        public void ShouldReceiveDealExecutionWhenOrderIsSent()
        {
            var market = Substitute.For<IMarket>();

            var sut = new SimpleOrderRoutingSystem(market);
            
            sut.Post(new OrderRequest());

            market.ReceivedWithAnyArgs(1).Send(null);
        }

        #endregion
    }

    public class SimpleOrderRoutingSystem
    {
        private IMarket market;

        public SimpleOrderRoutingSystem(IMarket market)
        {
            this.market = market;
        }

        #region Public Methods and Operators

        public void Post(OrderRequest orderRequest)
        {
            var order = new Order();
            this.market.Send(order);
        }

        #endregion
    }
}