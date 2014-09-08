// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SmartOrderRoutingEngine.cs" company="">
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
namespace SimpleOrderRouting.Journey1
{
    using System;
    using System.Collections.Generic;

    public class SmartOrderRoutingEngine
    {
        private readonly Market[] markets;

        public SmartOrderRoutingEngine(Market[] markets)
        {
            this.markets = markets;
        }

        public void Route(InvestorInstruction investorInstruction)
        {
            // 1. Digest Investment instructions
            // 2. Prepare order book (solver)
            // 3. Send and monitor
            // 4. Feedback investor

            //2.
            var orderBasket = this.SolveMarketSweep(investorInstruction);

            //3.

            ExecuteOrderBasket(orderBasket, investorInstruction);
        }

        private static void ExecuteOrderBasket(IEnumerable<OrderDescription> orderBasket, InvestorInstruction investorInstruction)
        {
            foreach (var orderDescription in orderBasket)
            {
                var market = orderDescription.TargetMarket;
                var order = market.CreateLimitOrder(orderDescription.OrderWay, orderDescription.OrderPrice, orderDescription.Quantity, orderDescription.AllowPartial);
                EventHandler<DealExecutedEventArgs> handler = (executedOrder, args) =>
                {
                    if (order == executedOrder)
                    {
                        // we have been executed
                        investorInstruction.NotifyOrderExecution(args.Quantity, args.Price);
                        //remainingQuantityToBeExecuted -= args.Quantity;
                    }
                };
                market.OrderExecuted += handler;
                try
                {
                    market.Send(order);
                }
                catch (ApplicationException)
                {
                }

                market.OrderExecuted -= handler;
            }
        }

        /// <summary>
        /// Build the description of the orders needed to fulfill the <see cref="investorInstruction"/>
        /// </summary>
        /// <param name="investorInstruction"></param>
        /// <returns>Order description</returns>
        private IList<OrderDescription> SolveMarketSweep(InvestorInstruction investorInstruction)
        {
            var description = new List<OrderDescription>();
            // Checks liquidities available to weighted average for execution
            int remainingQuantityToBeExecuted = investorInstruction.Quantity;

            var requestedPrice = investorInstruction.Price;
            
            var availableQuantityOnMarkets = this.ComputeAvailableQuantityForThisPrice(requestedPrice);

            var ratio = (remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets);

            foreach (var market in this.markets)
            {
                var convertedMarketQuantity = Math.Round((market.SellQuantity * ratio), 2, MidpointRounding.AwayFromZero);
                var quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                if (quantityToExecute > 0)
                {
                    description.Add(new OrderDescription(market, investorInstruction.Way, quantityToExecute, requestedPrice, true));
                }
            }

            return description;
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

        public InvestorInstruction CreateInvestorInstruction(Way way, int quantity, decimal price)
        {
            return new InvestorInstruction(way, quantity, price);
        }

        private struct OrderDescription
        {
            public Market TargetMarket;

            public Way OrderWay;

            public int Quantity;

            public decimal OrderPrice;

            public bool AllowPartial;

            public OrderDescription(Market targetMarket, Way orderWay, int quantity, decimal orderPrice, bool allowPartial)
            {
                this.TargetMarket = targetMarket;
                this.OrderWay = orderWay;
                this.Quantity = quantity;
                this.OrderPrice = orderPrice;
                this.AllowPartial = allowPartial;
            }
        }
    }
}