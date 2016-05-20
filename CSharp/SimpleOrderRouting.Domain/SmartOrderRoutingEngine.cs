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
        private readonly ICanRouteOrders routeOrders;
        private readonly MarketSnapshotProvider marketSnapshotProvider;
        
        private readonly IDictionary<InvestorInstruction, Action<string>> failureCallbacks = new Dictionary<InvestorInstruction, Action<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartOrderRoutingEngine"/> class.
        /// </summary>
        /// <param name="marketsProvider">The markets provider.</param>
        /// <param name="routeOrders">The order routing.</param>
        /// <param name="marketDataProvider">The market data provider.</param>
        public SmartOrderRoutingEngine(IProvideMarkets marketsProvider, ICanRouteOrders routeOrders, ICanReceiveMarketData marketDataProvider)
        {
            this.routeOrders = routeOrders;
            var availableMarkets = marketsProvider.GetAvailableMarketNames();
            this.marketSnapshotProvider = new MarketSnapshotProvider(availableMarkets, marketDataProvider);
        }

        public void Route(InvestorInstruction investorInstruction, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback)
        {
            // TODO: thread-safe it
            this.failureCallbacks[investorInstruction] = failureCallback;

            // Prepares to feedback the investor
            var instructionExecutionContext = new InstructionExecutionContext(investorInstruction, executedCallback);

            this.routeOrders.OrderExecuted += this.WhenOneOrderIsExecuted(instructionExecutionContext);
            this.routeOrders.OrderFailed += this.WhenOneOrderFailed(instructionExecutionContext);

            this.RouteImpl(instructionExecutionContext);

            this.routeOrders.OrderExecuted -= this.WhenOneOrderIsExecuted(instructionExecutionContext);
            this.routeOrders.OrderFailed -= this.WhenOneOrderFailed(instructionExecutionContext);
        }

        private EventHandler<OrderFailedEventArgs> WhenOneOrderFailed(InstructionExecutionContext instructionExecutionContext)
        {
            return (sender, orderFailed) =>
            {
                this.marketSnapshotProvider.DeclareFailure(orderFailed.MarketName);

                this.OnOrderFailed(orderFailed, instructionExecutionContext);
            };
        }

        private EventHandler<DealExecutedEventArgs> WhenOneOrderIsExecuted(InstructionExecutionContext instructionExecutionContext)
        {
            return (sender, dealExecuted) => instructionExecutionContext.DeclareOrderExecution(dealExecuted.Quantity);
        }

        private void RouteImpl(InstructionExecutionContext instructionExecutionContext)
        {
            // 1. Prepare the corresponding OrderBasket (via solver)
            var solver = new MarketSweepSolver(this.marketSnapshotProvider);
            var orderBasket = solver.Solve(instructionExecutionContext, this.routeOrders);

            // 2. Route the OrderBasket
            this.routeOrders.Route(orderBasket);
        }

        //private void InvestorInstruction_Executed(object sender, OrderExecutedEventArgs e)
        //{
        //    var investorInstruction = sender as InvestorInstruction;
        //    Action<OrderExecutedEventArgs> successCallback;
        //    if (this.executionCallbacks.TryGetValue(investorInstruction, out successCallback))
        //    {
        //        successCallback(e);
        //    }
        //}

        private void OnOrderFailed(OrderFailedEventArgs reason, InstructionExecutionContext instructionExecutionContext)
        {
            if (instructionExecutionContext.InstructionHasToBeContinued())
            {
                this.RetryInvestorInstruction(instructionExecutionContext);
            }
            else
            {
                this.NotifyInvestorInstructionFailure(instructionExecutionContext.Instruction, reason);
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