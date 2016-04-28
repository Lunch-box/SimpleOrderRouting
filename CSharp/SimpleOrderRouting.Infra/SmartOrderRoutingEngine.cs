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
namespace SimpleOrderRouting.Infra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Domain;
    using SimpleOrderRouting.Domain.Order;
    using SimpleOrderRouting.Domain.SmartOrderRouting;

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

        private Dictionary<IMarket, IMarket> markets;

        private IDictionary<InvestorInstructionIdentifierDto, Action<OrderExecutedEventArgs>> executionCallbacks = new Dictionary<InvestorInstructionIdentifierDto, Action<OrderExecutedEventArgs>>();
        private IDictionary<InvestorInstructionIdentifierDto, Action<string>> failureCallbacks = new Dictionary<InvestorInstructionIdentifierDto, Action<string>>();

        public SmartOrderRoutingEngine(IProvideMarkets provideMarkets, ICanRouteOrders canRouteOrders, ICanReceiveMarketData canReceiveMarketData)
        {
            this.provideMarkets = provideMarkets;
            this.canRouteOrders = canRouteOrders;
            this.canReceiveMarketData = canReceiveMarketData;
            var availableMarkets = provideMarkets.GetAvailableMarkets();
            this.markets = availableMarkets.ToDictionary(market => market, market => market);
            this.marketSnapshotProvider = new MarketSnapshotProvider(availableMarkets, canReceiveMarketData);
        }


        public void Route(InvestorInstruction investorInstruction)
        {
            // 2. Prepare order book (solver)
            // 3. Send and monitor
            // 4. Feedback investor

            var executionState = new InstructionExecutionContext(investorInstruction);

            this.RouteImpl(investorInstruction, executionState);
        }
        
        [Obsolete("Engine should not know infra code like DTO objects!")]
        public void Route(InvestorInstructionDto investorInstructionDto)
        {
            // 1. Digest Investment instructions
            
            var investorInstruction = this.CreateInvestorInstruction(investorInstructionDto.UniqueIdentifier, null, investorInstructionDto.Way, investorInstructionDto.Quantity, investorInstructionDto.Price, investorInstructionDto.AllowPartialExecution, investorInstructionDto.GoodTill);
            this.Route(investorInstruction);
        }

        //// TODO: remove investor instruction as arg here?
        private void RouteImpl(InvestorInstruction investorInstruction, InstructionExecutionContext instructionExecutionContext)
        {
            var solver = new MarketSweepSolver(this.marketSnapshotProvider);

            var orderBasket = solver.Solve(instructionExecutionContext);
            
            EventHandler<DealExecutedEventArgs> handler = (executedOrder, args) => instructionExecutionContext.Executed(args.Quantity);
            EventHandler<OrderFailedEventArgs> failHandler = (s, failure) => this.SendOrderFailed(investorInstruction, failure, instructionExecutionContext);

            investorInstruction.Executed += (sender, e) => this.investorInstruction_Executed(sender, e);

            orderBasket.OrderExecuted += handler;
            orderBasket.OrderFailed += failHandler;

            orderBasket.Send();

            orderBasket.OrderExecuted -= handler;
            orderBasket.OrderFailed -= failHandler;
        }

        void investorInstruction_Executed(object sender, OrderExecutedEventArgs e)
        {
            var investorInstruction = sender as InvestorInstruction;
            Action<OrderExecutedEventArgs> successCallback;
            if (this.executionCallbacks.TryGetValue(investorInstruction.InvestorInstructionIdentifier, out successCallback))
            {
                successCallback(e);
            }
        }

        private void SendOrderFailed(InvestorInstruction investorInstruction, OrderFailedEventArgs reason, InstructionExecutionContext instructionExecutionContext)
        {
            this.marketSnapshotProvider.MarketFailed(reason.Market);

            if (investorInstruction.GoodTill != null && 
                investorInstruction.GoodTill > DateTime.Now && 
                instructionExecutionContext.Quantity > 0)
            {
                // retries
                this.RouteImpl(investorInstruction, instructionExecutionContext);
            }
            else
            {
                Action<string> failureCallback;
                if (this.failureCallbacks.TryGetValue(investorInstruction.InvestorInstructionIdentifier, out failureCallback))
                {
                    failureCallback(reason.Reason);
                }
            }
        }

        [Obsolete("To be removed")]
        private InvestorInstruction CreateInvestorInstruction(InvestorInstructionIdentifierDto instructionIdentifierDto, InstrumentIdentifier instrumentIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null)
        {
            return new InvestorInstruction(instructionIdentifierDto.Value, way, quantity, price, allowPartialExecution, goodTill);
        }

        public void Subscribe(InvestorInstructionIdentifierDto investorInstructionIdentifierDto, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback)
        {
            // TODO: thread-safe it
            this.executionCallbacks[investorInstructionIdentifierDto] = executedCallback;
            this.failureCallbacks[investorInstructionIdentifierDto] = failureCallback;
            // TODO: regirst failure
        }
    }
}