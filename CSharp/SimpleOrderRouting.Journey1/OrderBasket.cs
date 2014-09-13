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
        private readonly List<OrderDescription> ordersDescription;

        public OrderBasket(List<OrderDescription> ordersDescription)
        {
            this.ordersDescription = ordersDescription;

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
        
        public bool AllowPartialExecution { get; private set; }

        public int Quantity { get; private set; }

        public Way Way { get; private set; }

        // TODO: Introduce Virtual Market?
        public Market Market { get; private set; }

        public void Send()
        {
            foreach (var orderInstruction in this.ordersDescription)
            {
                var market = orderInstruction.TargetMarket;
                var limitOrder = market.CreateLimitOrder(orderInstruction.OrderWay, orderInstruction.OrderPrice, orderInstruction.Quantity, orderInstruction.AllowPartial);

                limitOrder.OrderExecuted += this.LimitOrderOrderExecuted;
                try
                {
                    limitOrder.Send();
                }
                catch (ApplicationException)
                {
                }

                limitOrder.OrderExecuted -= this.LimitOrderOrderExecuted;
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