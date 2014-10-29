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
    using System.Linq;

    using SimpleOrderRouting.Interfaces;

    /// <summary>
    /// Transforms an <see cref="InvestorInstruction"/> into an <see cref="OrderBasket"/> that 
    /// will allow us to route <see cref="LimitOrder"/> following a weight average strategy on 
    /// the relevant markets.
    /// </summary>
    public class MarketSweepSolver : IInvestorInstructionSolver
    {
        private const int MaxSupportedFailuresPerMarket = 3;
        private readonly IEnumerable<IMarket> markets;

        private readonly MarketSnapshotProvider marketSnapshotProvider;

        private readonly ICanReceiveMarketData canReceiveMarketData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketSweepSolver"/> class.
        /// </summary>
        /// <param name="markets">The market information.</param>
        public MarketSweepSolver(IEnumerable<IMarket> markets, MarketSnapshotProvider marketSnapshotProvider)
        {
            this.markets = markets;
            this.marketSnapshotProvider = marketSnapshotProvider;
        }

        /// <summary>
        /// Build the description of the orders needed to fulfill an <see cref="InvestorInstruction" /> which
        /// is aggregated within an <see cref="InstructionExecutionContext" /> instance.
        /// </summary>
        /// <param name="instructionExecutionContext">The <see cref="InstructionExecutionContext" /> instance that aggregates the <see cref="InvestorInstruction" />.</param>
        /// <returns>
        /// An <see cref="OrderBasket" /> containing all the orders to be routed in order to fulfill the initial <see cref="InvestorInstruction" />.
        /// </returns>
        public OrderBasket Solve(InstructionExecutionContext instructionExecutionContext)
        {
            var ordersDescription = new List<OrderDescription>();

            // Checks liquidities available to weighted average for execution
            int remainingQuantityToBeExecuted = instructionExecutionContext.Quantity;

            var requestedPrice = instructionExecutionContext.Price;

            var validMarkets = this.GetValidMarkets(requestedPrice);
            var availableQuantityOnMarkets = this.ComputeAvailableQuantityForThisPrice(validMarkets);

            if (availableQuantityOnMarkets == 0)
            {
                return new OrderBasket(new List<OrderDescription>());    
            }

            var ratio = remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var marketInfo in validMarkets)
            {
                var convertedMarketQuantity = Math.Round(marketInfo.Market.SellQuantity * ratio, 2, MidpointRounding.AwayFromZero);
                var quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                if (quantityToExecute > 0)
                {
                    ordersDescription.Add(new OrderDescription(marketInfo.Market, instructionExecutionContext.Way, quantityToExecute, requestedPrice, instructionExecutionContext.AllowPartialExecution));
                }
            }

            return new OrderBasket(ordersDescription);
        }

        private IEnumerable<MarketInfo> GetValidMarkets(decimal requestedPrice)
        {
            var allMarkets = marketSnapshotProvider.GetSnapshot();
            return allMarkets.Markets.Where(m => m.OrdersFailureCount < MaxSupportedFailuresPerMarket && requestedPrice >= m.Market.SellPrice);
        }

        private int ComputeAvailableQuantityForThisPrice(IEnumerable<MarketInfo> validMarkets)
        {
            var availableQuantityOnMarkets = 0;

            foreach (var marketInfo in validMarkets)
            {
                availableQuantityOnMarkets += marketInfo.Market.SellQuantity;
            }

            return availableQuantityOnMarkets;
        }
    }

    public class MarketSnapshotProvider
    {
        private Dictionary<IMarket, MarketInfo> _lastMarketUpdates = new Dictionary<IMarket, MarketInfo>();

        public MarketSnapshotProvider(IEnumerable<IMarket> marketsToWatch, ICanReceiveMarketData canReceiveMarketData)
        {
            canReceiveMarketData.InstrumentMarketDataUpdated += InstrumentMarketDataUpdated;
            foreach (var market in marketsToWatch)
            {
                _lastMarketUpdates[market] = new MarketInfo(new Market());
                canReceiveMarketData.Subscribe(market);
            }
        }

        private void InstrumentMarketDataUpdated(object sender, MarketDataUpdateDto marketDataUpdateDto)
        {
            var marketInfo = _lastMarketUpdates[marketDataUpdateDto.Market];
            marketInfo.Market.SellPrice = marketDataUpdateDto.Price;
            marketInfo.Market.SellQuantity = marketDataUpdateDto.Quantity;
        }

        public MarketSnapshot GetSnapshot()
        {
            return new MarketSnapshot(_lastMarketUpdates.Values.ToList());
        }

        public void MarketFailed(Market market)
        {
            // TODO: refactor this so the method accepts IMarket instead
            _lastMarketUpdates.First(m => m.Value.Market == market).Value.OrdersFailureCount++;
        }
    }

    public class MarketSnapshot
    {
        public MarketSnapshot(IList<MarketInfo> markets)
        {
            this.Markets = markets;
        }

        public IList<MarketInfo> Markets { get; set; }

    }
}