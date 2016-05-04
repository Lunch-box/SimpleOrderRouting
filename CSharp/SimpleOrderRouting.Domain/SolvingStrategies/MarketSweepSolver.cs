// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketSweepSolver.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.SolvingStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Transforms an <see cref="InvestorInstruction"/> into an <see cref="OrderBasket"/> that 
    /// will allow us to route <see cref="LimitOrder"/> following a weight average strategy on 
    /// the relevant markets.
    /// </summary>
    public class MarketSweepSolver : ISolveInvestorInstructions
    {
        private const int MaxSupportedFailuresPerMarket = 3;

        private readonly MarketSnapshotProvider marketSnapshotProvider;

        private readonly ICanReceiveMarketData canReceiveMarketData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketSweepSolver"/> class.
        /// </summary>
        /// <param name="markets">The market information.</param>
        public MarketSweepSolver(MarketSnapshotProvider marketSnapshotProvider)
        {
            this.marketSnapshotProvider = marketSnapshotProvider;
        }

        /// <summary>
        /// Build the description of the orders needed to fulfill an <see cref="SimpleOrderRouting.Infra.InvestorInstruction" /> which
        /// is aggregated within an <see cref="InstructionExecutionContext" /> instance.
        /// </summary>
        /// <param name="instructionExecutionContext">The <see cref="InstructionExecutionContext" /> instance that aggregates the <see cref="SimpleOrderRouting.Infra.InvestorInstruction" />.</param>
        /// <returns>
        /// An <see cref="OrderBasket" /> containing all the orders to be routed in order to fulfill the initial <see cref="SimpleOrderRouting.Infra.InvestorInstruction" />.
        /// </returns>
        public OrderBasket Solve(InstructionExecutionContext instructionExecutionContext)
        {
            var ordersDescription = new List<OrderDescription>();

            // Checks liquidities available to weighted average for execution
            int remainingQuantityToBeExecuted = instructionExecutionContext.Quantity;

            var requestedPrice = instructionExecutionContext.Price;

            var validMarkets = this.GetValidMarkets(requestedPrice);
            var availableQuantityOnMarkets = this.ComputeAvailableQuantityForThisPrice(validMarkets);

            if (availableQuantityOnMarkets == 0)
            {
                return new OrderBasket(new List<OrderDescription>());    
            }

            var ratio = remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var marketInfo in validMarkets)
            {
                var convertedMarketQuantity = Math.Round(marketInfo.Market.SellQuantity * ratio, 2, MidpointRounding.AwayFromZero);
                var quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                if (quantityToExecute > 0)
                {
                    ordersDescription.Add(new OrderDescription(marketInfo.Market.Name, instructionExecutionContext.Way, quantityToExecute, requestedPrice, instructionExecutionContext.AllowPartialExecution));
                }
            }

            return new OrderBasket(ordersDescription);
        }

        private IEnumerable<MarketInfo> GetValidMarkets(decimal requestedPrice)
        {
            var allMarkets = this.marketSnapshotProvider.GetSnapshot();
            return allMarkets.Markets.Where(m => m.OrdersFailureCount < MaxSupportedFailuresPerMarket && requestedPrice >= m.Market.SellPrice);
        }

        private int ComputeAvailableQuantityForThisPrice(IEnumerable<MarketInfo> validMarkets)
        {
            var availableQuantityOnMarkets = 0;

            foreach (var marketInfo in validMarkets)
            {
                availableQuantityOnMarkets += marketInfo.Market.SellQuantity;
            }

            return availableQuantityOnMarkets;
        }
    }
}