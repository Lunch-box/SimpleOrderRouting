// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="Market.cs" company="">
// //   Copyright 2014 The Lunch-Box mob: Ozgur DEVELIOGLU (@Zgurrr), Cyrille  DUPUYDAUBY 
// //   (@Cyrdup), Tomasz JASKULA (@tjaskula), Thomas PIERRAIN (@tpierrain)
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace SimpleOrderRouting.Journey1
{
    using System;

    public class Market
    {
        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public int SellQuantity { get; set; }

        public decimal SellPrice { get; set; }

        public IOrder CreateMarketOrder(Way buy, int quantity)
        {
            return new MarketOrder(this, buy, quantity);
        }

        public void Send(IOrder order)
        {
            switch (order.Way)
            {
                case Way.Buy:
                    if (order is LimitOrder)
                    {
                        var limitOrder = order as LimitOrder;
                        if (limitOrder.Price > this.SellPrice)
                        {
                            return;
                        }
                    }

                    if (order.Quantity > this.SellQuantity)
                    {
                        if (!order.AllowPartialExecution)
                        {
                            throw new ApplicationException("Excessive quantity!");
                        }
                    }

                    var executedQuantity = Math.Min(order.Quantity, this.SellQuantity);
                    this.SellQuantity -= executedQuantity;

                    if (this.OrderExecuted != null)
                    {
                        this.OrderExecuted(order, new DealExecutedEventArgs(this.SellPrice, executedQuantity));
                    }

                    break;
            }
        }

        public IOrder CreateLimitOrder(Way way, decimal price, int quantity, bool allowPartialExecution)
        {
            return new LimitOrder(this, way, price, quantity, allowPartialExecution);
        }
    }
}