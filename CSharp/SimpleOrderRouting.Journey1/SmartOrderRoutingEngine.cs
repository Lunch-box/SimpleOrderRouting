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
            foreach (var market in markets)
            {
                var order=market.CreateLimitOrder(investorInstruction.Way, investorInstruction.Price, investorInstruction.Quantity,true);
                
                EventHandler<DealExecutedEventArgs> handler = (executedOrder, args) =>
                {
                    if (order == executedOrder)
                    {
                        // we have been executed
                        investorInstruction.NotifyOrderExecution(args.Quantity, args.Price);
                    }
                };
                market.OrderExecuted += handler;
                try
                {
                    market.Send(order);
                }
                catch(ApplicationException)
                {}
                market.OrderExecuted -= handler;
            }
        }


        public InvestorInstruction CreateInvestorInstruction(Way way, int quantity, decimal price)
        {
            return new InvestorInstruction(way, quantity, price);
        }
    }
}