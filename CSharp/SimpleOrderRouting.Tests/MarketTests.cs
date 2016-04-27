// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketTests.cs" company="LunchBox corp">
//     Copyright 2014 The Lunch-Box mob: 
//           Ozgur DEVELIOGLU (@Zgurrr)
//           Cyrille  DUPUYDAUBY (@Cyrdup)
//           Tomasz JASKULA (@tjaskula)
//           Mendel MONTEIRO-BECKERMAN (@MendelMonteiro)
//           Thomas PIERRAIN (@tpierrain)
//     
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//         http://www.apache.org/licenses/LICENSE-2.0
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace SimpleOrderRouting.Tests
{
    using NFluent;

    using SimpleOrderRouting.Infra;

    using SimpleOrderRouting.Domain.SmartOrderRouting;

    using Xunit;

    public class MarketTests
    {
        [Fact]
        public void MarketOrderShouldDecreaseAvailableQuantity()
        {
            var market = new Market { SellPrice = 100M, SellQuantity = 50 };

            var order = market.CreateMarketOrder(Way.Buy, quantity: 10);
            market.Send(order);

            Check.That(market.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void LargeMarketOrderShouldFail()
        {
            var market = new Market { SellPrice = 100M, SellQuantity = 50 };

            var order = market.CreateMarketOrder(Way.Buy, quantity: 100);

            bool failed = false;
            string failureReason = null;

            market.OrderFailed += (s, failure) =>
            {
                failed = true;
                failureReason = failure;
            };
            
            market.Send(order);

            Check.That(failed).IsTrue();
            Check.That(failureReason).IsEqualTo("Excessive quantity!");
        }

        [Fact]
        public void MarketOrderShouldCaptureExec()
        {
            var market = new Market { SellPrice = 100M, SellQuantity = 50 };
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
            var market = new Market { SellPrice = 100M, SellQuantity = 50 };
            var executed = false;
            var order = market.CreateLimitOrder(Way.Buy, price: 100M, quantity: 10, allowPartialExecution: false);

            market.OrderExecuted += (s, a) => executed = true;
            market.Send(order);

            Check.That(executed).IsTrue();
            Check.That(market.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void LimitOrderShouldFailIfPriceTooHigh()
        {
            var market = new Market { SellPrice = 100M, SellQuantity = 50 };
            var executed = false;
            bool failed = false;
            string failureReason = null;
            var order = market.CreateLimitOrder(Way.Buy, price: 101M, quantity: 10, allowPartialExecution: false);

            market.OrderExecuted += (s, a) => executed = true;
            market.OrderFailed += (s, failure) =>
                                  {
                                      failed = true;
                                      failureReason = failure;
                                  };
            market.Send(order);

            Check.That(executed).IsFalse();
            Check.That(failed).IsTrue();
            Check.That(failureReason).IsEqualTo("Invalid price");
            Check.That(market.SellQuantity).IsEqualTo(50);
        }

        [Fact]
        public void LimitOrderShouldSupportPartialExecution()
        {
            var market = new Market { SellPrice = 100M, SellQuantity = 50 };
            var executed = false;
            var order = market.CreateLimitOrder(Way.Buy, price: 100M, quantity: 110, allowPartialExecution: true);

            var execQuantity = 0;
            market.OrderExecuted += (s, a) =>
            {
                executed = true;
                execQuantity = a.Quantity;
            };
            market.Send(order);

            Check.That(execQuantity).IsEqualTo(50);
            Check.That(executed).IsTrue();
            Check.That(market.SellQuantity).IsEqualTo(0);
        }
    }
}
