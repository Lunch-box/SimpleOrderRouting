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

    public class SmartOrderRoutingEngine
    {
        private readonly Market[] markets;

        public SmartOrderRoutingEngine(Market[] markets)
        {
            this.markets = markets;
        }

        public void Route(InvestorInstruction investorInstruction)
        {
            int remainingQuantityToBeExecuted = investorInstruction.Quantity;
            
            // Checks liquidities availabe to weighted average for execution
            var availableQuantityOnMarkets = 0;
            foreach (var market in this.markets)
            {
                if (investorInstruction.Price >= market.SellPrice)
                {
                    availableQuantityOnMarkets += market.SellQuantity;
                }
            }

            decimal ratio = ((decimal)remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets);
            decimal roundedRatio = Math.Round(ratio, 2, MidpointRounding.AwayFromZero);

            foreach (var market in this.markets)
            {
                decimal convertedMarketQuantity = Math.Round((market.SellQuantity * ratio), 2, MidpointRounding.AwayFromZero);
                int quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                var order = market.CreateLimitOrder(investorInstruction.Way, investorInstruction.Price, quantityToExecute, true);
                
                EventHandler<DealExecutedEventArgs> handler = (executedOrder, args) =>
                {
                    if (order == executedOrder)
                    {
                        // we have been executed
                        investorInstruction.NotifyOrderExecution(args.Quantity, args.Price);
                        remainingQuantityToBeExecuted -= args.Quantity;
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

        public InvestorInstruction CreateInvestorInstruction(Way way, int quantity, decimal price)
        {
            return new InvestorInstruction(way, quantity, price);
        }
    }
}