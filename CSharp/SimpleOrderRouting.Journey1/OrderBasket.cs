// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderBasket.cs" company="LunchBox corp">
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

    /// <summary>
    /// Aggregates multiple <see cref="IOrder"/> instances.
    /// <remarks>OrderBasket is a composite (pattern).</remarks>
    /// </summary>
    public class OrderBasket : IOrder
    {
        private readonly List<OrderDescription> ordersDescriptions;

        public OrderBasket(List<OrderDescription> ordersDescription)
        {
            this.ordersDescriptions = ordersDescription;

            this.Way = ordersDescription[0].OrderWay;

            foreach (var orderDescription in ordersDescription)
            {
                if (orderDescription.AllowPartial)
                {
                    this.AllowPartialExecution = true;
                }

                this.Quantity += orderDescription.Quantity;
            }
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;
        public event EventHandler<string> OrderFailed;

        public bool AllowPartialExecution { get; private set; }

        public int Quantity { get; private set; }

        public Way Way { get; private set; }

        // TODO: Introduce Virtual Market?
        public Market Market { get; private set; }

        // TODO: Change the IOrder interface to always include the notification.
        public void Send()
        {
            throw new NotImplementedException();
        }

        public void Send(Action<TerminalState> notification)
        {
            List<TerminalState> failures = new List<TerminalState>(this.ordersDescriptions.Count);
            foreach (var orderDescription in this.ordersDescriptions)
            {
                var market = orderDescription.TargetMarket;
                var limitOrder = market.CreateLimitOrder(orderDescription.OrderWay, orderDescription.OrderPrice, orderDescription.Quantity, orderDescription.AllowPartial);

                limitOrder.OrderExecuted += this.LimitOrderOrderExecuted;
                EventHandler<string> limitOrderOnOrderFailed = (sender, args) => failures.Add(TerminalState.Failed);
                limitOrder.OrderFailed += limitOrderOnOrderFailed;

                limitOrder.Send();

                limitOrder.OrderExecuted -= this.LimitOrderOrderExecuted;
                limitOrder.OrderFailed -= limitOrderOnOrderFailed;
            }

            if (notification != null)
            {
                if (failures.Count > 0)
                {
                    notification(TerminalState.Failed);
                }
                else
                {
                    notification(TerminalState.Executed);
                }
            }
        }

        private void LimitOrderOrderExecuted(object sender, DealExecutedEventArgs e)
        {
            if (this.OrderExecuted != null)
            {
                this.OrderExecuted(this, e);
            }
        }
    }
}