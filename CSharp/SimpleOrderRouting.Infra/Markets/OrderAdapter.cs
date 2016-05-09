// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderAdapter.cs" company="LunchBox corp">
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

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Base class for the <see cref="LimitOrderAdapter"/> and the <see cref="MarketOrderAdapter"/>.
    /// </summary>
    public abstract class OrderAdapter : IOrder
    {
        protected readonly ApiMarketGateway MarketGateway;

        private readonly ApiOrder apiOrder;

        protected OrderAdapter(ApiMarketGateway marketGateway, ApiOrder apiOrder)
        {
            this.apiOrder = apiOrder;
            this.MarketGateway = marketGateway;
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        public Way Way
        {
            get
            {
                return (this.apiOrder.Way == ApiMarketWay.Sell) ? Way.Sell : Way.Buy;
            }
        }

        public int Quantity
        {
            get
            {
                return this.apiOrder.Quantity;
            }
        }

        public bool AllowPartialExecution
        {
            get
            {
                return false;
            }
        }

        public abstract void Send();
    }
}