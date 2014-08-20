// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SorAcceptanceTests.cs" company="">
// //   Copyright 2014 The Lunch-Box mob: Ozgur DEVELIOGLU (@Zgurrr), Cyrille  DUPUYDAUBY 
// //   (@Cyrdup), Tomasz JASKULA (@tjaskula), Thomas PIERRAIN (@tpierrain)
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
    using System.Threading;

    using NFluent;

    using SimpleOrderRouting.Journey1;

    using Xunit;

    public class SorAcceptanceTests
    {
        #region Public Methods and Operators

        [Fact]
        public void ShouldExecuteOrderWhenThereIsEnoughLiquidityOnTheMarkets()
        {
            // Given market A: 150 @ $100, market B: 55 @ $100 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested price
            var marketA = new Market()
                              {
                                  SellQuantity = 150,
                                  SellPrice = 100
                              };
            
            var marketB = new Market()
                              {
                                  SellQuantity = 55,
                                  SellPrice = 100
                              };

            var sor = new SmartOrderRoutingSystem(new[] { marketA, marketB });

            var orderRequest = sor.CreateRoutingRequest(Way.Buy, quantity: 125, price: 100);
            OrderExecutedEventArgs orderExecutedEventArgs = null;
            orderRequest.Executed += (sender, args) =>{ orderExecutedEventArgs = args; };
            
            sor.Route(orderRequest);

            // TODO :introduce autoreset event instead
            Thread.Sleep(10);

            Check.That(orderExecutedEventArgs.Price).IsEqualTo(100);
            Check.That(orderExecutedEventArgs.Quantity).IsEqualTo(125);
            Check.ThatEnum(orderExecutedEventArgs.Way).IsEqualTo(Way.Buy);
        }
        
        #endregion
    }
}