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

    using NUnit.Framework;

    using OtherTeam.StandardizedMarketGatewayAPI;
    using SimpleOrderRouting.Infra;

    public class SorAcceptanceTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Acts like a composition root for the SOR Hexagonal Architecture.
        /// </summary>
        /// <param name="marketGateways">The list of ApiMarketGateway the SOR must interact with.</param>
        /// <returns>The adapter we must use as Investors in order to give investment instructions.</returns>
        private static InvestorInstructionsAdapter ComposeTheHexagon(params ApiMarketGateway[] marketGateways)
        {
            // Step1: instantiates the adapter(s) the (SOR) domain will need to work with through the Dependency Inversion principle.
            var marketGatewaysAdapter = new MarketGatewaysAdapter(marketGateways);
            
            // Step2: instantiates the SOR domain entry point.
            var sor = new SmartOrderRoutingEngine(marketGatewaysAdapter, marketGatewaysAdapter, marketGatewaysAdapter);
            
            // Step3: instantiates the adapters we will use to interact with our domain.
            var investorInstructionAdapter = new InvestorInstructionsAdapter(sor);

            return investorInstructionAdapter;
        }

        [Test]
        public void Should_execute_instruction_when_there_is_enough_liquidity_on_one_Market()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested MarketPrice
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 150, sellPrice: 100M);
            var marketB = new ApiMarketGateway("CME (Chicago)", sellQuantity: 55, sellPrice: 101M);
            
            var investorInstructionAdapter = ComposeTheHexagon(marketA, marketB);

            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 125, price: 100M);

            InstructionExecutedEventArgs instructionExecuted = null;

            investorInstructionAdapter.Route(investorInstructionDto, args => { instructionExecuted = args; }, args => {});

            Check.That(instructionExecuted).IsNotNull();
            Check.That(instructionExecuted).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 125, Price = 100M });

            Check.That(marketA.SellQuantity).IsEqualTo(25);
            Check.That(marketB.SellQuantity).IsEqualTo(55);
        }

        [Test]
        public void Should_failed_when_Order_exceeds_all_Market_capacity_and_partial_execution_not_allowed()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested MarketPrice
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 15, sellPrice: 100M);
            var marketB = new ApiMarketGateway("CME (Chicago)", sellQuantity: 55, sellPrice: 101M);

            var investorInstructionAdapter = ComposeTheHexagon(marketA, marketB);

            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 125, price: 100M, allowPartialExecution: false);

            // Subscribes to the instruction's events
            InstructionExecutedEventArgs instructionExecuted = null;
            InstructionFailedEventArgs instructionFailed = null;

            investorInstructionAdapter.Route(investorInstructionDto, (args) => { instructionExecuted = args; }, (args) => { instructionFailed = args; });

            // Couldn't execute because order with excessive QuantityOnTheMarket
            Check.That(instructionFailed.Reason).IsNotNull().And.IsEqualIgnoringCase("Excessive quantity!");
            Check.That(instructionExecuted).IsNull();

            Check.That(marketA.SellQuantity).IsEqualTo(15);
            Check.That(marketB.SellQuantity).IsEqualTo(55);
        }

        [Test]
        public void Should_stop_sending_Orders_to_a_Market_after_3_rejects()
        {
            var rejectingMarket = new ApiMarketGateway("LSE (London)", sellQuantity: 100, sellPrice: 100M, orderPredicate : order => false);

            var investorInstructionAdapter = ComposeTheHexagon(rejectingMarket);
            
            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 50, price: 100M, goodTill: DateTime.Now.AddMinutes(5));

            investorInstructionAdapter.Route(investorInstructionDto, (args) => { }, (args) => { });

            Check.That(rejectingMarket.TimesSent).IsEqualTo(3);

            Check.That(rejectingMarket.SellQuantity).IsEqualTo(100);
        }

        [Test]
        public void Should_succeeded_when_liquidity_is_available_even_if_one_Market_rejects()
        {
            // Given market A: 150 @ $100, market B: 55 @ $101 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested MarketPrice
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 50, sellPrice: 100M);
            var rejectingMarket = new ApiMarketGateway("LSE (London)", sellQuantity: 50, sellPrice: 100M, orderPredicate: _ => false);

            var investorInstructionAdapter = ComposeTheHexagon(marketA, rejectingMarket);

            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 50, price: 100M, goodTill: DateTime.Now.AddMinutes(5));

            // Subscribes to the instruction's events
            InstructionExecutedEventArgs instructionExecuted = null;
            InstructionFailedEventArgs instructionFailed = null;

            investorInstructionAdapter.Route(investorInstructionDto, (args) => { instructionExecuted = args; }, (args) => { instructionFailed = args; });

            Check.That(instructionExecuted).IsNotNull();
            Check.That(instructionFailed).IsNull();

            Check.That(marketA.SellQuantity).IsEqualTo(0);
            Check.That(rejectingMarket.SellQuantity).IsEqualTo(50);
        }

        [Test]
        public void Should_execute_Instruction_when_there_is_enough_liquidity_on_the_Markets()
        {
            // Given market A: 100 @ $100, market B: 55 @ $100 
            // When Investor wants to buy 125 stocks @ $100 Then SOR can execute at the requested MarketPrice
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 100, sellPrice: 100M);
            var marketB = new ApiMarketGateway("CME (Chicago)", sellQuantity: 55, sellPrice: 100M);

            var investorInstructionAdapter = ComposeTheHexagon(marketA, marketB);

            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 125, price: 100M);

            InstructionExecutedEventArgs instructionExecuted = null;

            investorInstructionAdapter.Route(investorInstructionDto, (args) => { instructionExecuted = args; }, null);

            Check.That(instructionExecuted).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 125, Price = 100M });

            Check.That(marketA.SellQuantity).IsEqualTo(19);
            Check.That(marketB.SellQuantity).IsEqualTo(11);
        }

        [Test]
        public void Should_execute_Orders_on_weighted_average_of_available_quantities()
        {
            // 25 premier ; 75 % sur le second marché
            // Given market A: 100 @ $100, market B: 50 @ $100 
            // When Investor wants to buy 75 stocks @ $100 Then SOR can execute at the requested MarketPrice
            // And execution is: 50 stocks on MarketA and 25 stocks on MarketB
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 100, sellPrice: 100M);
            var marketB = new ApiMarketGateway("CME (Chicago)", sellQuantity: 50, sellPrice: 100M);

            var investorInstructionAdapter = ComposeTheHexagon(marketA, marketB);

            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 75, price: 100M);

            InstructionExecutedEventArgs instructionExecuted = null;

            investorInstructionAdapter.Route(investorInstructionDto, (args) => { instructionExecuted = args; }, null);

            Check.That(instructionExecuted).HasFieldsWithSameValues(new { Way = Way.Buy, Quantity = 75, Price = 100M });
            
            Check.That(marketA.SellQuantity).IsEqualTo(50);
            Check.That(marketB.SellQuantity).IsEqualTo(25);
        }

        #endregion
    }
}