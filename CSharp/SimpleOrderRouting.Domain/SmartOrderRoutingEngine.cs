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
    /// Provides access to the various services offered by the external market venues.
    /// Manages incoming InvestorInstructions and monitor their lifecycle.
    /// Is responsible for the consistency of the open positions (i.e. alive orders) that are present on every markets.
    /// </summary>
    public class SmartOrderRoutingEngine : IHandleInvestorInstructions
    {
        private readonly ICanRouteOrders orderRouting;
        private readonly MarketSnapshotProvider marketSnapshotProvider;
        private readonly IDictionary<InvestorInstruction, Action<OrderExecutedEventArgs>> executionCallbacks = new Dictionary<InvestorInstruction, Action<OrderExecutedEventArgs>>();
        private readonly IDictionary<InvestorInstruction, Action<string>> failureCallbacks = new Dictionary<InvestorInstruction, Action<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartOrderRoutingEngine"/> class.
        /// </summary>
        /// <param name="marketsProvider">The markets provider.</param>
        /// <param name="orderRouting">The order routing.</param>
        /// <param name="marketDataProvider">The market data provider.</param>
        public SmartOrderRoutingEngine(IProvideMarkets marketsProvider, ICanRouteOrders orderRouting, ICanReceiveMarketData marketDataProvider)
        {
            this.orderRouting = orderRouting;
            var availableMarkets = marketsProvider.GetAvailableMarketNames();
            this.marketSnapshotProvider = new MarketSnapshotProvider(availableMarkets, marketDataProvider);
        }

        public void Route(InvestorInstruction investorInstruction, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback)
        {
            // TODO: thread-safe it
            this.executionCallbacks[investorInstruction] = executedCallback;
            this.failureCallbacks[investorInstruction] = failureCallback;
            //// TODO: regirst failure

            // Prepares to feedback the investor
            investorInstruction.Executed += this.InvestorInstruction_Executed;
            var instructionExecutionContext = new InstructionExecutionContext(investorInstruction);

            // TODO: add symetry here (i.e. always go via the InstructionExecutionContext
            EventHandler<DealExecutedEventArgs> handler = (sender, args) => instructionExecutionContext.Executed(args.Quantity);
            EventHandler<OrderFailedEventArgs> failHandler = (sender, failure) => this.OnOrderFailed(investorInstruction, failure, instructionExecutionContext);

            this.orderRouting.OrderExecuted += handler;
            this.orderRouting.OrderFailed += failHandler;

            this.RouteImpl(instructionExecutionContext);

            this.orderRouting.OrderExecuted -= handler;
            this.orderRouting.OrderFailed -= failHandler;
        }

        private void RouteImpl(InstructionExecutionContext instructionExecutionContext)
        {
            // 1. Prepare the corresponding OrderBasket (via solver)
            var solver = new MarketSweepSolver(this.marketSnapshotProvider);
            var orderBasket = solver.Solve(instructionExecutionContext, this.orderRouting);

            // 2. Route the OrderBasket
            this.orderRouting.Route(orderBasket);
        }

        private void InvestorInstruction_Executed(object sender, OrderExecutedEventArgs e)
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
                this.RetryInvestorInstruction(instructionExecutionContext);
            }
            else
            {
                this.NotifyInvestorInstructionFailure(investorInstruction, reason);
            }
        }

        private void NotifyInvestorInstructionFailure(InvestorInstruction investorInstruction, OrderFailedEventArgs reason)
        {
            Action<string> failureCallback;
            if (this.failureCallbacks.TryGetValue(investorInstruction, out failureCallback))
            {
                failureCallback(reason.Reason);
            }
        }

        private void RetryInvestorInstruction(InstructionExecutionContext instructionExecutionContext)
        {
            this.RouteImpl(instructionExecutionContext);
        }    
    }
}