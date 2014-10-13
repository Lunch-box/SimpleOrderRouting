// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartOrderRoutingEngine.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Journey1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides access to the various services offered by the external markets.
    /// Manages incoming InvestorInstructions and monitor their lifecycle.
    /// Is responsible for the consistency of the open positions (i.e. alive orders) that are present on every markets.
    /// </summary>
    public class SmartOrderRoutingEngine
    {
//        private readonly Market[] markets;
        private readonly Dictionary<Market, MarketInfo> markets;

        public SmartOrderRoutingEngine(Market[] markets)
        {
            this.markets = markets.ToDictionary(market => market, market => new MarketInfo(market));
        }

        public void Route(InvestorInstruction investorInstruction)
        {
            // 1. Digest Investment instructions
            // 2. Prepare order book (solver)
            // 3. Send and monitor
            // 4. Feedback investor
            var executionState = new ExecutionState(investorInstruction);

            this.RouteImpl(investorInstruction, executionState);
        }

        // TODO: remove investor instruction as arg here?
        private void RouteImpl(InvestorInstruction investorInstruction, ExecutionState executionState)
        {
            var solver = new MarketSweepSolver(this.markets.Values);

            var orderBasket = solver.Solve(executionState);
            
            EventHandler<DealExecutedEventArgs> handler = (executedOrder, args) => { investorInstruction.NotifyOrderExecution(args.Quantity, args.Price); };

            EventHandler<OrderFailedEventArgs> failHandler = (s, failure) => SendOrderFailed(investorInstruction, failure, executionState);

            orderBasket.OrderExecuted += handler;
            orderBasket.OrderFailed += failHandler;

            orderBasket.Send();

            orderBasket.OrderExecuted -= handler;
            orderBasket.OrderFailed -= failHandler;
        }

        private void SendOrderFailed(InvestorInstruction investorInstruction, OrderFailedEventArgs reason, ExecutionState executionState)
        {
            this.markets[reason.Market].Failures++;

            if (investorInstruction.GoodTill != null && 
                investorInstruction.GoodTill > DateTime.Now && 
                executionState.Quantity > 0)
            {
                // retries
                this.RouteImpl(investorInstruction, executionState);
            }
            else
            {
                investorInstruction.NotifyOrderFailure(reason.Reason);
            }
        }

        public InvestorInstruction CreateInvestorInstruction(Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null)
        {
            return new InvestorInstruction(way, quantity, price, allowPartialExecution, goodTill);
        }
    }

    public class MarketInfo
    {
        public MarketInfo(Market market)
        {
            this.Market = market;
        }

        public Market Market { get; private set; }

        public int Failures { get; set; }
    }
}