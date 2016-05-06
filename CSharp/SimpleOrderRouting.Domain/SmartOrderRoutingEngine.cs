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
namespace SimpleOrderRouting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.Markets.Orders;
    using SimpleOrderRouting.SolvingStrategies;

    /// <summary>
    /// Provides access to the various services offered by the external markets.
    /// Manages incoming InvestorInstructions and monitor their lifecycle.
    /// Is responsible for the consistency of the open positions (i.e. alive orders) that are present on every markets.
    /// </summary>
    public class SmartOrderRoutingEngine : ISmartOrderRoutingEntryPoint
    {
        private readonly IProvideMarkets provideMarkets;
        private readonly ICanRouteOrders canRouteOrders;
        private readonly ICanReceiveMarketData canReceiveMarketData;

        private MarketSnapshotProvider marketSnapshotProvider;

        private IDictionary<InvestorInstruction, Action<OrderExecutedEventArgs>> executionCallbacks = new Dictionary<InvestorInstruction, Action<OrderExecutedEventArgs>>();
        private IDictionary<InvestorInstruction, Action<string>> failureCallbacks = new Dictionary<InvestorInstruction, Action<string>>();

        public SmartOrderRoutingEngine(IProvideMarkets provideMarkets, ICanRouteOrders canRouteOrders, ICanReceiveMarketData canReceiveMarketData)
        {
            this.provideMarkets = provideMarkets;
            this.canRouteOrders = canRouteOrders;
            
            this.canReceiveMarketData = canReceiveMarketData;
            var availableMarkets = provideMarkets.GetAvailableMarketNames();
            this.marketSnapshotProvider = new MarketSnapshotProvider(availableMarkets, canReceiveMarketData);
        }

        public void Route(InvestorInstruction investorInstruction)
        {
            var executionState = new InstructionExecutionContext(investorInstruction);

            // Prepare to Feedback the investor
            investorInstruction.Executed += this.InvestorInstruction_Executed;

            // TODO: add symetry here (i.e. always go via the InstructionExecutionContext
            EventHandler<DealExecutedEventArgs> handler = (sender, args) => executionState.Executed(args.Quantity);
            EventHandler<OrderFailedEventArgs> failHandler = (sender, failure) => this.OnOrderFailed(investorInstruction, failure, executionState);

            this.canRouteOrders.OrderExecuted += handler;
            this.canRouteOrders.OrderFailed += failHandler;

            this.RouteImpl(investorInstruction, executionState);

            // TODO: possible race condition between Solve and the event part?

            this.canRouteOrders.OrderExecuted -= handler;
            this.canRouteOrders.OrderFailed -= failHandler;
        }

        //// TODO: remove investor instruction as arg here?
        private void RouteImpl(InvestorInstruction investorInstruction, InstructionExecutionContext instructionExecutionContext)
        {
            // 1. Prepare order book (solver)
            var solver = new MarketSweepSolver(this.marketSnapshotProvider);
            var orderBasket = solver.Solve(instructionExecutionContext, this.canRouteOrders);
            
            // 2. Route the order book
            this.canRouteOrders.Route(orderBasket);
        }

        void InvestorInstruction_Executed(object sender, OrderExecutedEventArgs e)
        {
            var investorInstruction = sender as InvestorInstruction;
            Action<OrderExecutedEventArgs> successCallback;
            if (this.executionCallbacks.TryGetValue(investorInstruction, out successCallback))
            {
                successCallback(e);
            }
        }

        private void OnOrderFailed(InvestorInstruction investorInstruction, OrderFailedEventArgs reason, InstructionExecutionContext instructionExecutionContext)
        {
            this.marketSnapshotProvider.MarketFailed(reason.MarketName);

            if (investorInstruction.GoodTill != null && 
                investorInstruction.GoodTill > DateTime.Now && 
                instructionExecutionContext.RemainingQuantityToBeExecuted > 0)
            {
                // retries
                this.RouteImpl(investorInstruction, instructionExecutionContext);
            }
            else
            {
                Action<string> failureCallback;
                if (this.failureCallbacks.TryGetValue(investorInstruction, out failureCallback))
                {
                    failureCallback(reason.Reason);
                }
            }
        }
        
        public void Subscribe(InvestorInstruction investorInstruction, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback)
        {
            // TODO: thread-safe it
            this.executionCallbacks[investorInstruction] = executedCallback;
            this.failureCallbacks[investorInstruction] = failureCallback;
            //// TODO: regirst failure
        }
    }
}