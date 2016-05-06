// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiMarketGatewayTests.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Tests.Infra
{
    using NFluent;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using Xunit;

    public class ApiMarketGatewayTests
    {
        [Fact]
        public void Should_decrease_available_quantity_for_the_market_when_an_order_is_sent()
        {
            var marketGateway = new ApiMarketGateway(marketName: "euronext", sellQuantity:50, sellPrice: 100M);

            var order = marketGateway.CreateMarketOrder(ApiMarketWay.Buy, quantity: 10);
            marketGateway.Send(order);

            Check.That(marketGateway.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void Should_failed_to_execute_order_when_quantity_is_excessive()
        {
            var marketGateway = new ApiMarketGateway(marketName: "euronext", sellQuantity: 50, sellPrice: 100M);

            var order = marketGateway.CreateMarketOrder(ApiMarketWay.Buy, quantity: 100);

            bool failed = false;
            string failureReason = null;

            marketGateway.OrderFailed += (s, failedEventArgs) =>
            {
                failed = true;
                failureReason = failedEventArgs.FailureCause;
            };
            
            marketGateway.Send(order);

            Check.That(failed).IsTrue();
            Check.That(failureReason).IsEqualTo("Excessive quantity!");
        }

        [Fact]
        public void Should_Notify_MarketOrder_execution()
        {
            var marketGateway = new ApiMarketGateway(marketName: "euronext", sellQuantity: 50, sellPrice: 100M);

            var executed = false;
            var order = marketGateway.CreateMarketOrder(ApiMarketWay.Buy, quantity: 10);

            marketGateway.OrderExecuted += (s, a) => executed = true;
            marketGateway.Send(order);

            Check.That(executed).IsTrue();
            Check.That(marketGateway.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void Should_Notify_LimitOrder_execution()
        {
            var marketGateway = new ApiMarketGateway(marketName: "euronext", sellQuantity: 50, sellPrice: 100M);

            var executed = false;
            var order = marketGateway.CreateLimitOrder(ApiMarketWay.Buy, price: 100M, quantity: 10, allowPartial: false);

            marketGateway.OrderExecuted += (s, a) => executed = true;
            marketGateway.Send(order);

            Check.That(executed).IsTrue();
            Check.That(marketGateway.SellQuantity).IsEqualTo(40);
        }

        [Fact]
        public void Should_not_execute_LimitOrder_when_price_is_too_high()
        {
            var marketGateway = new ApiMarketGateway(marketName: "euronext", sellQuantity: 50, sellPrice: 100M);

            var executed = false;
            bool failed = false;
            string failureReason = null;
            var order = marketGateway.CreateLimitOrder(ApiMarketWay.Buy, price: 101M, quantity: 10, allowPartial: false);

            marketGateway.OrderExecuted += (s, a) => executed = true;
            marketGateway.OrderFailed += (s, failedEventArgs) =>
                                  {
                                      failed = true;
                                      failureReason = failedEventArgs.FailureCause;
                                  };
            marketGateway.Send(order);

            Check.That(executed).IsFalse();
            Check.That(failed).IsTrue();
            Check.That(failureReason).IsEqualTo("Invalid price");
            Check.That(marketGateway.SellQuantity).IsEqualTo(50);
        }

        [Fact]
        public void Should_support_partial_execution_for_LimitOrder()
        {
            var marketGateway = new ApiMarketGateway(marketName: "euronext", sellQuantity: 50, sellPrice: 100M);

            var executed = false;
            var order = marketGateway.CreateLimitOrder(ApiMarketWay.Buy, price: 100M, quantity: 110, allowPartial: true);

            var execQuantity = 0;
            marketGateway.OrderExecuted += (s, a) =>
            {
                executed = true;
                execQuantity = a.Quantity;
            };
            marketGateway.Send(order);

            Check.That(execQuantity).IsEqualTo(50);
            Check.That(executed).IsTrue();
            Check.That(marketGateway.SellQuantity).IsEqualTo(0);
        }
    }
}
