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
        public void ShouldRaiseDealExecutionEventWhenOrderIsExecuted()
        {
            var market = Substitute.For<IMarket>();
            SetupToRaiseOrderExecutedEventEverytimeSendIsCalled(market);

            var sut = new SimpleOrderRoutingSystem(market);
            
            var subscriber = Substitute.For<IOrderExecutedSubscriber>();

            // Question: OrderExecuted, DealExecuted or DealDone?
            sut.OrderExecuted += subscriber.OrderExecuted;

            sut.Post(new OrderRequest());
            
            // Checks the event has been raised
            subscriber.ReceivedWithAnyArgs(1).OrderExecuted(null, null);
        }

        private static void SetupToRaiseOrderExecutedEventEverytimeSendIsCalled(IMarket market)
        {
            market.WhenForAnyArgs(m => m.Send(null)).Do(_ => market.OrderExecuted += Raise.EventWith<OrderExecutedEventArgs>(market, new OrderExecutedEventArgs("Bloomb", "orderId")));
        }

        #endregion
    }
}