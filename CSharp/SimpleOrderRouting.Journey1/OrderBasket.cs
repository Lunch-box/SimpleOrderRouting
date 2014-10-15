// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderBasket.cs" company="LunchBox corp">
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
    /// Aggregates multiple <see cref="IOrder"/> instances.
    /// <remarks>OrderBasket is a composite (pattern).</remarks>
    /// </summary>
    public class OrderBasket : IOrder
    {
        private readonly List<OrderDescription> ordersDescriptions;

        public OrderBasket(List<OrderDescription> ordersDescriptions)
        {
            this.ordersDescriptions = ordersDescriptions;

            this.Way = ordersDescriptions[0].OrderWay;

            foreach (var orderDescription in ordersDescriptions)
            {
                if (orderDescription.AllowPartial)
                {
                    this.AllowPartialExecution = true;
                }

                this.Quantity += orderDescription.Quantity;
            }
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        public bool AllowPartialExecution { get; private set; }

        public int Quantity { get; private set; }

        public Way Way { get; private set; }

        // TODO: Introduce Virtual Market?
        public Market Market { get; private set; }

        // TODO: Change the IOrder interface to always include the notification.
        public void Send()
        {
            var failures = new List<OrderFailedEventArgs>(this.ordersDescriptions.Count);
            foreach (var orderDescription in this.ordersDescriptions)
            {
                var market = orderDescription.TargetMarket;
                var limitOrder = market.CreateLimitOrder(orderDescription.OrderWay, orderDescription.OrderPrice, orderDescription.Quantity, orderDescription.AllowPartial);

                limitOrder.OrderExecuted += (o, e) => this.OnOrderExecuted(orderDescription, e);
                EventHandler<OrderFailedEventArgs> limitOrderOnOrderFailed = (sender, reason) => failures.Add(reason);
                limitOrder.OrderFailed += limitOrderOnOrderFailed;

                limitOrder.Send();

                limitOrder.OrderExecuted -= (sender, e) => this.OnOrderExecuted(orderDescription, e);
                limitOrder.OrderFailed -= limitOrderOnOrderFailed;
            }

            if (failures.Count > 0)
            {
                this.RaiseOrderFailed(failures);
            }
        }

        private void OnOrderExecuted(OrderDescription orderDescription, DealExecutedEventArgs e)
        {
            orderDescription.Executed(e.Quantity);
            this.RaiseOrderExecuted(e);
        }

        private void RaiseOrderFailed(List<OrderFailedEventArgs> failures)
        {
            var onOrderFailed = this.OrderFailed;
            if (onOrderFailed != null)
            {
                onOrderFailed(this, failures.First());
            }
        }

        private void RaiseOrderExecuted(DealExecutedEventArgs e)
        {
            var onOrderExecuted = this.OrderExecuted;
            if (onOrderExecuted != null)
            {
                onOrderExecuted(this, e);
            }
        }
    }
}