// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Market.cs" company="LunchBox corp">
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

    public class Market : ITestableMarket, IMarket
    {
        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public event EventHandler<string> OrderFailed;

        public int SellQuantity { get; set; }

        public decimal SellPrice { get; set; }

        public Predicate<IOrder> OrderPredicate { get; set; }

        public int TimesSent { get; private set; }

        public IOrder CreateMarketOrder(Way buy, int quantity)
        {
            return new MarketOrder(this, buy, quantity);
        }

        public void Send(IOrder order)
        {
            this.TimesSent++;

            if (this.OrderPredicate != null && this.OrderPredicate(order) == false)
            {
                this.RaiseOrderFailed(order, "Predicate failed.");
                return;
            }

            switch (order.Way)
            {
                case Way.Buy:
                    if (order is LimitOrder)
                    {
                        var limitOrder = order as LimitOrder;
                        if (limitOrder.Price > this.SellPrice)
                        {
                            this.RaiseOrderFailed(order, "Invalid price");
                            return;
                        }
                    }

                    if (order.Quantity > this.SellQuantity)
                    {
                        if (!order.AllowPartialExecution)
                        {
                            this.RaiseOrderFailed(order, "Excessive quantity!");
                            return;
                        }
                    }

                    var executedQuantity = Math.Min(order.Quantity, this.SellQuantity);
                    this.SellQuantity -= executedQuantity;

                    this.RaiseOrderExecuted(order, executedQuantity);

                    break;
            }
        }

        private void RaiseOrderExecuted(IOrder order, int executedQuantity)
        {
            var onOrderExecuted = this.OrderExecuted;
            if (onOrderExecuted != null)
            {
                onOrderExecuted(order, new DealExecutedEventArgs(this.SellPrice, executedQuantity));
            }
        }

        private void RaiseOrderFailed(IOrder order, string reason)
        {
            var onOrderFailed = this.OrderFailed;
            if (onOrderFailed != null)
            {
                onOrderFailed(order, reason);
            }
        }

        public IOrder CreateLimitOrder(Way way, decimal price, int quantity, bool allowPartialExecution)
        {
            return new LimitOrder(this, way, price, quantity, allowPartialExecution);
        }
    }
}