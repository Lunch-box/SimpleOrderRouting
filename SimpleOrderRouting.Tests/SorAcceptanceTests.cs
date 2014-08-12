namespace SimpleOrderRouting.Tests
{
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

        [Fact]
        public void ShouldReceiveDealExecutionWhenOrderIsSent()
        {
            var market = Substitute.For<IMarket>();

            var sut = new SimpleOrderRoutingSystem(market);
            
            var subscriber = Substitute.For<IOrderExecutedSubscriber>();

            // Question: OrderExecuted, DealExecuted or DealDone?
            sut.OrderExecuted += subscriber.OrderExecuted;

            sut.Post(new OrderRequest());
            
            // Checks the event has been raised
            subscriber.ReceivedWithAnyArgs(1).OrderExecuted(null, null);
        }

        public interface IOrderExecutedSubscriber
        {
            void OrderExecuted(object sender, OrderExecutedEventArgs e);
        }

        #endregion
    }
}