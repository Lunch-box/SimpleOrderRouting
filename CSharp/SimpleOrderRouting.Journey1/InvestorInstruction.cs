// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="InvestorInstruction.cs" company="">
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

    public class InvestorInstruction
    {
        public event EventHandler<OrderExecutedEventArgs> Executed;

        /// <summary>
        /// Just a naive implementation to make the test pass.
        /// </summary>
        /// <param name="executedQuantity"></param>
        /// <param name="executedPrice"></param>
        public virtual void NotifyOrderExecution(int executedQuantity, decimal executedPrice)
        {
            if (executedPrice == Price && executedQuantity == this.Quantity)
            {
                // instruction fully executed, I notify
                if (this.Executed != null)
                {
                    this.Executed(this, new OrderExecutedEventArgs(this.Way, this.Quantity, this.Price));
                }
            }
        }


        public Way Way { get; private set; }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        public InvestorInstruction(Way way, int quantity, decimal price)
        {
            this.Way = way;
            this.Quantity = quantity;
            this.Price = price;
        }
    }
}