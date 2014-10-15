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
namespace SimpleOrderRouting.Journey1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Transforms an <see cref="InvestorInstruction"/> into an <see cref="OrderBasket"/> that 
    /// will allow us to route <see cref="LimitOrder"/> following a weight average strategy on 
    /// the relevant markets.
    /// </summary>
    public class MarketSweepSolver : IInvestorInstructionSolver
    {
        private const int MaxSupportedFailuresPerMarket = 3;
        private readonly IEnumerable<MarketInfo> marketInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketSweepSolver"/> class.
        /// </summary>
        /// <param name="marketInfos">The market information.</param>
        public MarketSweepSolver(IEnumerable<MarketInfo> marketInfos)
        {
            this.marketInfos = marketInfos;
        }

        /// <summary>
        /// Build the description of the orders needed to fulfill an <see cref="InvestorInstruction" /> which
        /// is aggregated within an <see cref="ExecutionState" /> instance.
        /// </summary>
        /// <param name="executionState">The <see cref="ExecutionState" /> instance that aggregates the <see cref="InvestorInstruction" />.</param>
        /// <returns>
        /// An <see cref="OrderBasket" /> containing all the orders to be routed in order to fulfill the initial <see cref="InvestorInstruction" />.
        /// </returns>
        public OrderBasket Solve(ExecutionState executionState)
        {
            var ordersDescription = new List<OrderDescription>();

            // Checks liquidities available to weighted average for execution
            int remainingQuantityToBeExecuted = executionState.Quantity;

            var requestedPrice = executionState.Price;
            
            var availableQuantityOnMarkets = this.ComputeAvailableQuantityForThisPrice(requestedPrice);

            var ratio = remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var marketInfo in this.marketInfos.Where(m => m.OrdersFailureCount < MaxSupportedFailuresPerMarket))
            {
                var convertedMarketQuantity = Math.Round(marketInfo.Market.SellQuantity * ratio, 2, MidpointRounding.AwayFromZero);
                var quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                if (quantityToExecute > 0)
                {
                    ordersDescription.Add(new OrderDescription(marketInfo.Market, executionState.Way, quantityToExecute, requestedPrice, executionState.AllowPartialExecution, executionState));
                }
            }

            return new OrderBasket(ordersDescription);
        }

        private int ComputeAvailableQuantityForThisPrice(decimal requestedPrice)
        {
            var availableQuantityOnMarkets = 0;

            foreach (var marketInfo in this.marketInfos)
            {
                if (requestedPrice >= marketInfo.Market.SellPrice)
                {
                    availableQuantityOnMarkets += marketInfo.Market.SellQuantity;
                }
            }

            return availableQuantityOnMarkets;
        }
    }
}