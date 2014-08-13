// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SorAcceptanceTests.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN, Tomasz JASKULA
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace SimpleOrderRouting.Tests
{
    using NSubstitute;

    using SimpleOrderRouting.Journey1;

    using Xunit;

    public class SorAcceptanceTests
    {
        #region Public Methods and Operators

        [Fact]
        public void ShouldSendOrderWhenExecutionStrategyIsRequested()
        {
            var market = Substitute.For<IMarketOrderRouting>();

            var sut = new SimpleOrderRoutingSystem(market);
            sut.Post(new OrderRequest());

            market.ReceivedWithAnyArgs(1).Send(null);
        }

        [Fact]
        public void ShouldRaiseDealExecutionEventWhenOrderIsExecuted()
        {
            var market = Substitute.For<IMarketOrderRouting>();
            GetReadyToRaiseOrderExecutedEventEverytimeSendIsCalled(market);

            var sut = new SimpleOrderRoutingSystem(market);

            var subscriber = Substitute.For<IOrderExecutedSubscriber>();
            sut.OrderExecuted += subscriber.OrderExecuted;

            sut.Post(new OrderRequest());
            
            // Checks the event has been raised
            subscriber.ReceivedWithAnyArgs(1).OrderExecuted(null, null);
        }

        private static void GetReadyToRaiseOrderExecutedEventEverytimeSendIsCalled(IMarketOrderRouting marketOrderRouting)
        {
            marketOrderRouting.WhenForAnyArgs(m => m.Send(null)).Do(_ => marketOrderRouting.DealExecuted += Raise.EventWith<DealExecutedEventArgs>(marketOrderRouting, new DealExecutedEventArgs()));
        }

        [Fact]
        public void ShouldCancelOrderIfNoMarketDataIsAvailableForRelatedSpotInstrument()
        {
            var market = Substitute.For<IMarketOrderRouting>();
            var marketDataProvider = Substitute.For<IMarketDataProvider>();

            var sut = new SimpleOrderRoutingSystem(market, marketDataProvider);

            sut.Post(new OrderRequest() { InstrumentName = "EUR/USD" });

            // Send order not called
            market.DidNotReceiveWithAnyArgs().Send(null);

            // OrderRequestCanceled event raised
        }

        #endregion
    }
}