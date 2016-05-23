// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketSweepSolverTests.cs" company="LunchBox corp">
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
    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.SolvingStrategies;

    public class MarketSweepSolverTests
    {
        [Test]
        public void Should_Solve_with_2_markets_when_asked_quantity_is_odd()
        {
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 50, sellPrice: 100M);
            var rejectingMarket = new ApiMarketGateway("CME (Chicago)", sellQuantity: 50, sellPrice: 100M, orderPredicate: _ => false);
            var marketsInvolved = new[] { marketA, rejectingMarket };
            var marketGatewayAdapter = new MarketsAdapter(marketsInvolved);

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 50, price: 100M, goodTill: DateTime.Now.AddMinutes(5));
            var instructionExecutionContext = new InstructionExecutionContext(investorInstruction, args => {}, failure => {});

            var marketSweepSolver = new MarketSweepSolver(new MarketSnapshotProvider(marketGatewayAdapter.GetAvailableMarketNames(), marketGatewayAdapter));
            var orderBasket = marketSweepSolver.Solve(instructionExecutionContext, marketGatewayAdapter);

            Check.That(orderBasket.OrdersDescriptions.Extracting("Quantity")).ContainsExactly(25, 25);
        }

        [Test]
        public void Should_Solve_with_2_markets_when_asked_quantity_is_1()
        {
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 50, sellPrice: 100M);
            var rejectingMarket = new ApiMarketGateway("CME (Chicago)", sellQuantity: 50, sellPrice: 100M, orderPredicate: _ => false);
            var marketsInvolved = new[] { marketA, rejectingMarket };
            var marketGatewayAdapter = new MarketsAdapter(marketsInvolved);

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 1, price: 100M, goodTill: DateTime.Now.AddMinutes(5));
            var instructionExecutionContext = new InstructionExecutionContext(investorInstruction, args => { }, failure => { });

            var marketSweepSolver = new MarketSweepSolver(new MarketSnapshotProvider(marketGatewayAdapter.GetAvailableMarketNames(), marketGatewayAdapter));
            var orderBasket = marketSweepSolver.Solve(instructionExecutionContext, marketGatewayAdapter);

            Check.That(orderBasket.OrdersDescriptions.Extracting("Quantity")).ContainsExactly(1);
        }
    }
}