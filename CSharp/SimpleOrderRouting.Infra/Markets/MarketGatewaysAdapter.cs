// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketGatewaysAdapter.cs" company="LunchBox corp">
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

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets.Feeds;
    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Adapter between the external Markets gateways model and the SOR one.
    /// </summary>
    public sealed class MarketGatewaysAdapter : ICanRouteOrders, ICanReceiveMarketData, IProvideMarkets
    {
        private readonly Dictionary<string, ApiMarketGateway> gateways;

        public MarketGatewaysAdapter(params ApiMarketGateway[] apiMarketGateways)
        {
            this.gateways = new Dictionary<string, ApiMarketGateway>();

            foreach (var marketGateway in apiMarketGateways)
            {
                this.gateways[marketGateway.MarketName] = marketGateway;
                marketGateway.MarketDataUpdated += this.MarketGatewayOnMarketDataUpdated;
                marketGateway.OrderExecuted += this.MarketGatewayOnOrderExecuted;
                marketGateway.OrderFailed += this.MarketGatewayOnOrderFailed;
            }
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        private void RaiseOrderExecuted(DealExecutedEventArgs args)
        {
            var orderExecuted = this.OrderExecuted;
            if (orderExecuted != null)
            {
                orderExecuted(this, args);
            }
        }

        private void RaiseOrderFailed(OrderFailedEventArgs args)
        {
            var orderFailed = this.OrderFailed;
            if (orderFailed != null)
            {
                orderFailed(this, args);
            }
        }

        private void MarketGatewayOnMarketDataUpdated(object sender, ApiMarketDataUpdateEventArgs apiMarketDataUpdateEventArgs)
        {
            // Adapts the external API feed format to the SOR domain format
            var marketDataUpdatedArgs = new MarketDataUpdatedArgs(apiMarketDataUpdateEventArgs.OriginMarketName, apiMarketDataUpdateEventArgs.MarketPrice, apiMarketDataUpdateEventArgs.QuantityOnTheMarket);
            this.RaiseMarketDataUpdate(marketDataUpdatedArgs);
        }

        private void MarketGatewayOnOrderExecuted(object sender, ApiDealExecutedEventArgs externalApiExecutionArgs)
        {
            // Adapts the external API format to the SOR domain format
            var dealExecutedEventArgs = new DealExecutedEventArgs(externalApiExecutionArgs.Price, externalApiExecutionArgs.Quantity);
            this.RaiseOrderExecuted(dealExecutedEventArgs);
        }

        private void MarketGatewayOnOrderFailed(object sender, ApiOrderFailedEventArgs apiArgs)
        {
            // Adapts the external API format to the SOR domain format
            var dealExecutedEventArgs = new OrderFailedEventArgs(apiArgs.MarketName, apiArgs.FailureCause);
            this.RaiseOrderFailed(dealExecutedEventArgs);
        }

        #region ICanReceiveMarketData

        public event EventHandler<MarketDataUpdatedArgs> InstrumentMarketDataUpdated;

        public void Subscribe(string marketName)
        {
            var internalMarket = this.gateways.First(m => m.Key == marketName).Value;

            // Raise the first event
            var marketDataUpdatedArgs = new MarketDataUpdatedArgs(internalMarket.MarketName, internalMarket.SellPrice, internalMarket.SellQuantity);

            this.RaiseMarketDataUpdate(marketDataUpdatedArgs);
        }

        private void RaiseMarketDataUpdate(MarketDataUpdatedArgs args)
        {
            var onInstrumentMarketDataUpdated = this.InstrumentMarketDataUpdated;
            if (onInstrumentMarketDataUpdated != null)
            {
                onInstrumentMarketDataUpdated(this, args);
            }
        }

        #endregion

        #region IProvideMarkets

        public IEnumerable<string> GetAvailableMarketNames()
        {
            return this.gateways.Keys;
        }

        #endregion

        #region ICanRouteOrders

        public IOrder CreateMarketOrder(OrderDescription orderDescription)
        {
            // Adapts from the SOR model to the external market gateway one
            var marketGateway = this.gateways[orderDescription.TargetMarketName];

            ApiMarketWay apiMarketWay = (orderDescription.OrderWay == Way.Sell) ? ApiMarketWay.Sell : ApiMarketWay.Buy;
            ApiMarketOrder apiMarketOrder = marketGateway.CreateMarketOrder(apiMarketWay, orderDescription.Quantity);

            return new MarketOrderAdapter(marketGateway, apiMarketOrder);
        }

        public IOrder CreateLimitOrder(OrderDescription orderDescription)
        {
            var marketGateway = this.gateways[orderDescription.TargetMarketName];
            ApiMarketWay apiMarketWay = (orderDescription.OrderWay == Way.Sell) ? ApiMarketWay.Sell : ApiMarketWay.Buy;
            ApiLimitOrder apiLimitOrder = marketGateway.CreateLimitOrder(apiMarketWay, orderDescription.Quantity, orderDescription.OrderPrice, orderDescription.AllowPartialExecution);

            return new LimitOrderAdapter(marketGateway, apiLimitOrder);
        }

        public void Route(OrderBasket basketOrder)
        {
            basketOrder.Send();
        }

        public void Route(IOrder order)
        {
            // Note: the previously created IOrder is already an adapter from the SOR model to the external market gateway format
            order.Send();
        }

        #endregion
    }
}