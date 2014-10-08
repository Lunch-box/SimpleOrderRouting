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

    public class MarketSweepSolver : IInvestorInstructionSolver
    {
        private readonly Market[] markets;

        public MarketSweepSolver(Market[] markets)
        {
            this.markets = markets;
        }

        /// <summary>
        /// Build the description of the orders needed to fulfill the <see cref="investorInstruction"/>
        /// </summary>
        /// <param name="investorInstruction">The Investor instruction.</param>
        /// <returns>Order description.</returns>
        public OrderBasket Solve(InvestorInstruction investorInstruction)
        {
            var ordersDescription = new List<OrderDescription>();

            // Checks liquidities available to weighted average for execution
            int remainingQuantityToBeExecuted = investorInstruction.Quantity;

            var requestedPrice = investorInstruction.Price;
            
            var availableQuantityOnMarkets = this.ComputeAvailableQuantityForThisPrice(requestedPrice);

            var ratio = remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var market in this.markets)
            {
                var convertedMarketQuantity = Math.Round(market.SellQuantity * ratio, 2, MidpointRounding.AwayFromZero);
                var quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                if (quantityToExecute > 0)
                {
                    ordersDescription.Add(new OrderDescription(market, investorInstruction.Way, quantityToExecute, 
                        requestedPrice, investorInstruction.AllowPartialExecution));
                }
            }

            return new OrderBasket(ordersDescription);
        }

        private int ComputeAvailableQuantityForThisPrice(decimal requestedPrice)
        {
            var availableQuantityOnMarkets = 0;

            foreach (var market in this.markets)
            {
                if (requestedPrice >= market.SellPrice)
                {
                    availableQuantityOnMarkets += market.SellQuantity;
                }
            }

            return availableQuantityOnMarkets;
        }
    }
}