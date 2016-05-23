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
namespace SimpleOrderRouting.Markets.Orders
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
        public List<OrderDescription> OrdersDescriptions { get; private set; }

        private readonly ICanRouteOrders canRouteOrders;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBasket"/> class.
        /// </summary>
        /// <param name="ordersDescriptions">The orders descriptions.</param>
        /// <param name="canRouteOrders">The can route orders.</param>
        public OrderBasket(List<OrderDescription> ordersDescriptions, ICanRouteOrders canRouteOrders)
        {
            this.OrdersDescriptions = ordersDescriptions;
            this.canRouteOrders = canRouteOrders;

            if (ordersDescriptions.Count > 0)
            {
                this.Way = ordersDescriptions[0].OrderWay;

                foreach (var orderDescription in ordersDescriptions)
                {
                    if (orderDescription.AllowPartialExecution)
                    {
                        this.AllowPartialExecution = true;
                    }

                    this.Quantity += orderDescription.Quantity;
                }
            }
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<OrderFailedEventArgs> OrderFailed;

        public bool AllowPartialExecution { get; private set; }

        public int Quantity { get; private set; }

        public Way Way { get; private set; }

        // TODO: Change the IOrder interface to always include the notification.
        public void Send()
        {
            var failures = new List<OrderFailedEventArgs>(this.OrdersDescriptions.Count);

            foreach (var orderDescription in this.OrdersDescriptions)
            {
                // TODO: refactor this to prevent this domain object to rely too much on ICanRouterOrders things...
                var limitOrder = this.canRouteOrders.CreateLimitOrder(orderDescription);

                EventHandler<DealExecutedEventArgs> orderExecuted = (sender, executedEventArgs) => this.OnOrderExecuted(executedEventArgs);
                EventHandler<OrderFailedEventArgs> orderFailed = (sender, reason) => failures.Add(reason);
                
                limitOrder.OrderExecuted += orderExecuted;
                limitOrder.OrderFailed += orderFailed;

                limitOrder.Send();

                limitOrder.OrderExecuted -= orderExecuted;
                limitOrder.OrderFailed -= orderFailed;
            }

            if (failures.Count > 0)
            {
                this.RaiseOrderFailed(failures);
            }
        }

        private void OnOrderExecuted(DealExecutedEventArgs e)
        {
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