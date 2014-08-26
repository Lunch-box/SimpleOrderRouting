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
        public int SellQuantity { get; set; }

        public decimal SellPrice { get; set; }

        
        public IOrder CreateMarketOrder(Way buy, int quantity)
        {
            return new MarketOrder(buy, quantity);
        }

        public void Send(IOrder order)
        {
            switch (order.Way)
            {
                case Way.Buy:
                    if (order is LimitOrder)
                    {
                        var limitOrder = order as LimitOrder;
                        if (limitOrder.Price > SellPrice) return;
                    }

                    if (order.Quantity > SellQuantity)
                    {
                        if (!order.AllowPartialExecution)
                        {
                            throw new ApplicationException("Excessive quantity!");
                        }


                    }

                    var executedQuantity = Math.Min(order.Quantity, this.SellQuantity);

                    SellQuantity -=  executedQuantity;

                    if (this.OrderExecuted != null)
                    {
                        this.OrderExecuted(order, new DealExecutedEventArgs(this.SellPrice, executedQuantity));
                    }

                    break;
            }
        }

        public event EventHandler<DealExecutedEventArgs> OrderExecuted;

        public IOrder CreateLimitOrder(Way way, decimal price, int quantity, bool allowPartialExecution)
        {
            return new LimitOrder(way, price, quantity, allowPartialExecution);
        }
    }

    public class LimitOrder : IOrder
    {
        public bool AllowPartialExecution { get; private set; }

        public LimitOrder(Way way, decimal price, int quantity, bool allowPartialExecution)
        {
            this.AllowPartialExecution = allowPartialExecution;
            this.Way = way;
            this.Price = price;
            this.Quantity = quantity;
        }

        public Way Way { get; private set; }

        public decimal Price { get; set; }

        public int Quantity { get; private set; }
    }

    public interface IOrder
    {
        Way Way { get; }

        int Quantity { get; }

        bool AllowPartialExecution { get; }
    }

    public class MarketOrder : IOrder
    {
        private readonly Way buy;

        private readonly int quantity;

        public MarketOrder(Way buy, int quantity)
        {
            this.buy = buy;
            this.quantity = quantity;
        }

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
    }
}