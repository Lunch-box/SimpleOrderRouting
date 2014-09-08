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
   
    /// <summary>
    /// Provides access to the various services offered by the external markets.
    /// Manages incomming InvestorInstructions and monitor their lifecycle.
    /// Is responsible for the consistency of the open positions (i.e. alive orders) that are present on every markets.
    /// </summary>
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
            var solver = new MarketSweepSolver(markets);

            //2.
            var orderBasket = solver.Solve(investorInstruction);

            //3.

            ExecuteOrderBasket(orderBasket, investorInstruction);
        }

        private static void ExecuteOrderBasket(IEnumerable<OrderDescription> orderInstructions, InvestorInstruction investorInstruction)
        {
            foreach (var orderInstruction in orderInstructions)
            {
                var market = orderInstruction.TargetMarket;
                var limitOrder = market.CreateLimitOrder(orderInstruction.OrderWay, orderInstruction.OrderPrice, orderInstruction.Quantity, orderInstruction.AllowPartial);
                EventHandler<DealExecutedEventArgs> handler = (executedOrder, args) =>
                {
                    if (limitOrder == executedOrder)
                    {
                        // we have been executed
                        investorInstruction.NotifyOrderExecution(args.Quantity, args.Price);
                        //remainingQuantityToBeExecuted -= args.Quantity;
                    }
                };
                market.OrderExecuted += handler;
                try
                {
                    limitOrder.Send();
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