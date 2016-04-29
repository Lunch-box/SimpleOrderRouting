// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderExecutedEventArgs.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Markets.Orders
{
    /// <summary>
    /// Event data for OrderExecuted event.
    /// </summary>
    public class OrderExecutedEventArgs
    {
        public OrderExecutedEventArgs(Way way, int quantity, decimal price)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.Way = way;
        }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        public Way Way { get; private set; }
    }
}