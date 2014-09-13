// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SorAcceptanceTests.cs" company="LunchBox corp">
//    Copyright 2014 The Lunch-Box mob: Ozgur DEVELIOGLU (@Zgurrr), Cyrille  DUPUYDAUBY 
//    (@Cyrdup), Tomasz JASKULA (@tjaskula), Thomas PIERRAIN (@tpierrain)
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace SimpleOrderRouting.Tests
{
    using NFluent;

    using SimpleOrderRouting.Journey1;

    using Xunit;

    public class SorAcceptanceTests
    {
        #region Public Methods and Operators

        [Fact]
        public void ShouldExecuteInstructionWhenThereIsEnoughLiquidityOnOneMarket()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested price
            var marketA = new Market()
                              {
                                  SellQuantity = 150,
                                  SellPrice = 100M
                              };
            
            var marketB = new Market()
                              {
                                  SellQuantity = 55,
                                  SellPrice = 101M
                              };

            var sor = new SmartOrderRoutingEngine(new[] { marketA, marketB });

            var investorInstruction = sor.CreateInvestorInstruction(Way.Buy, quantity: 125, price: 100M);

            OrderExecutedEventArgs orderExecutedEventArgs = null;
            investorInstruction.Executed += (sender, args) => { orderExecutedEventArgs = args; };

            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            // TODO :introduce autoreset event instead
            Check.That(orderExecutedEventArgs).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 125, Price = 100M });
        }

        [Fact]
        public void ShouldExecuteInstructionWhenThereIsEnoughLiquidityOnTheMarkets()
        {
            // Given market A: 100 @ $100, market B: 55 @ $100 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested price
            var marketA = new Market()
                              {
                                  SellQuantity = 100,
                                  SellPrice = 100M
                              };
            
            var marketB = new Market()
                              {
                                  SellQuantity = 55,
                                  SellPrice = 100M
                              };

            var sor = new SmartOrderRoutingEngine(new[] { marketA, marketB });

            var investorInstruction = sor.CreateInvestorInstruction(Way.Buy, quantity: 125, price: 100M);

            OrderExecutedEventArgs orderExecutedEventArgs = null;
            investorInstruction.Executed += (sender, args) => { orderExecutedEventArgs = args; };

            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            // TODO :introduce autoreset event instead
            Check.That(orderExecutedEventArgs).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 125, Price = 100M });
        }

        [Fact]
        public void ShouldExecuteOrdersOnWeightedAverageOfAvailableQuantities()
        {
            // 25 premier ; 75 % sur le second marché
            // Given market A: 100 @ $100, market B: 50 @ $100 
            // When Investor wants to buy 75 stocks @ $100 Then SOR can execute at the requested price
            // And execution is: 50 stocks on MarketA and 25 stocks on MarketB
            var marketA = new Market()
            {
                SellQuantity = 100,
                SellPrice = 100M
            };

            var marketB = new Market()
            {
                SellQuantity = 50,
                SellPrice = 100M
            };

            var sor = new SmartOrderRoutingEngine(new[] { marketA, marketB });

            var investorInstruction = sor.CreateInvestorInstruction(Way.Buy, quantity: 75, price: 100M);

            OrderExecutedEventArgs orderExecutedEventArgs = null;
            investorInstruction.Executed += (sender, args) => { orderExecutedEventArgs = args; };

            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            // TODO :introduce autoreset event instead
            Check.That(orderExecutedEventArgs).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 75, Price = 100M });
            Check.That(marketA.SellQuantity).IsEqualTo(50);
            Check.That(marketB.SellQuantity).IsEqualTo(25);
        }

        #endregion
    }
}