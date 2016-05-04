// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SorAcceptanceTests.cs" company="LunchBox corp">
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
    using System;

    using NFluent;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Infra;
    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.Markets.Orders;
    using SimpleOrderRouting.Tests.TestHelpers;

    using Xunit;

    public class SorAcceptanceTests
    {
        #region Public Methods and Operators

        [Fact]
        public void Should_execute_instruction_when_there_is_enough_liquidity_on_one_Market()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested price
            var marketA = new ApiMarketGateway("Euronext", 150, 100M);

            var marketB = new ApiMarketGateway("LSE", 55, 101M);

            var marketsInvolved = new[] { marketA, marketB };

            // TODO: plug the proper market adapter implementation
            var marketGatewaysAdapter = new MarketGatewaysAdapter(marketsInvolved);

            var sor = new SmartOrderRoutingEngine(marketGatewaysAdapter, marketGatewaysAdapter, marketGatewaysAdapter);

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 125, price: 100M);

            OrderExecutedEventArgs orderExecutedEventArgs = null;
            sor.Subscribe(investorInstruction, (args) => { orderExecutedEventArgs = args; }, null);
                //investorInstruction.Executed += (sender, args) => { orderExecutedEventArgs = args; };
            
            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            // TODO :introduce autoreset event instead
            Check.That(orderExecutedEventArgs).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 125, Price = 100M });
        }

        [Fact]
        public void Should_failed_when_Order_exceeds_all_Market_capacity_and_partial_execution_not_allowed()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested price
            var marketA = new Market
            {
                                  SellQuantity = 15,
                                  SellPrice = 100M
                              };
            
            var marketB = new Market
            {
                                  SellQuantity = 55,
                                  SellPrice = 101M
                              };

            var sor = CreateSmartOrderRoutingEngine(new[] { marketA, marketB });

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 125, price: 100M, allowPartialExecution: false);

            // Subscribes to the instruction's events
            OrderExecutedEventArgs orderExecutedEventArgs = null;
            string failureReason = null;

            sor.Subscribe(investorInstruction, (args) => { orderExecutedEventArgs = args; }, (args) => { failureReason = args; });

            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            // Couldn't execute because order with excessive quantity
            Check.That(failureReason).IsNotNull().And.IsEqualIgnoringCase("Excessive quantity!");
            Check.That(orderExecutedEventArgs).IsNull();
        }

        private static SmartOrderRoutingEngine CreateSmartOrderRoutingEngine(Market[] markets)
        {
            var routingEngine = new SmartOrderRoutingEngine(new MarketProvider(markets), null, new MarketDataProvider(markets));
            return routingEngine;
        }

        [Fact]
        public void Should_stop_sending_Orders_to_a_Market_after_3_rejects()
        {
            var rejectingMarket = new Market
                             {
                                 SellQuantity = 100,
                                 SellPrice = 100M,
                                 OrderPredicate = order => false
                             };

            var sor = CreateSmartOrderRoutingEngine(new[] { rejectingMarket });
            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 50, price: 100M, goodTill: DateTime.Now.AddMinutes(5));
            sor.Route(investorInstruction);

            Check.That(rejectingMarket.TimesSent).IsEqualTo(3);
        }

        [Fact]
        public void Should_succeeded_when_liquidity_is_available_even_if_one_Market_rejects()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested price
            var marketA = new Market()
            {
                SellQuantity = 50,
                SellPrice = 100M
            };

            var rejectMarket = new Market()
            {
                SellQuantity = 50,
                SellPrice = 100M,
                OrderPredicate = (o) => false
            };

            var sor = CreateSmartOrderRoutingEngine(new[] { marketA, rejectMarket });

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 50, price: 100M, goodTill: DateTime.Now.AddMinutes(5));

            // Subscribes to the instruction's events
            OrderExecutedEventArgs orderExecutedEventArgs = null;
            string failureReason = null;
            
            sor.Subscribe(investorInstruction, (args) => { orderExecutedEventArgs = args; }, (args) => { failureReason = args; });

            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            Check.That(orderExecutedEventArgs).IsNotNull();
            Check.That(failureReason).IsNull();
        }

        [Fact]
        public void Should_execute_Instruction_when_there_is_enough_liquidity_on_the_Markets()
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

            var sor = CreateSmartOrderRoutingEngine(new[] { marketA, marketB });

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, /*new InstrumentIdentifier("EURUSD"),*/ Way.Buy, quantity: 125, price: 100M);

            OrderExecutedEventArgs orderExecutedEventArgs = null;

            sor.Subscribe(investorInstruction, (args) => { orderExecutedEventArgs = args; }, null);

            // orderRequest.Route(); ?
            sor.Route(investorInstruction);

            Check.That(orderExecutedEventArgs).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 125, Price = 100M });
        }

        [Fact]
        public void Should_execute_Orders_on_weighted_average_of_available_quantities()
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

            var sor = CreateSmartOrderRoutingEngine(new[] { marketA, marketB });

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 75, price: 100M);

            OrderExecutedEventArgs orderExecutedEventArgs = null;

            sor.Subscribe(investorInstruction, (args) => { orderExecutedEventArgs = args; }, null);

            sor.Route(investorInstruction);

            Check.That(orderExecutedEventArgs).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 75, Price = 100M });
            Check.That(marketA.SellQuantity).IsEqualTo(50);
            Check.That(marketB.SellQuantity).IsEqualTo(25);
        }

        #endregion
    }
}