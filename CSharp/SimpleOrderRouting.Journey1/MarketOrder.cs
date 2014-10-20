// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketOrder.cs" company="LunchBox corp">
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

    using SimpleOrderRouting.Interfaces;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

    public class MarketOrder : IOrder
    {
        private readonly Way buy;

        private readonly int quantity;

        public MarketOrder(Market market, Way buy, int quantity)
        {
            this.Market = market;
            this.buy = buy;
            this.quantity = quantity;
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        public Market Market { get; private set; }

        public Way Way
        {
            get
            {
                return this.buy;
            }
        }

        public int Quantity
        {
            get
            {
                return this.quantity;
            }
        }

        public bool AllowPartialExecution
        {
            get
            {
                return false;
            }
        }

        public void Send()
        {
            this.Market.Send(this);
        }
    }
}