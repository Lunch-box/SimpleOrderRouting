// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvestorInstruction.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Domain
{
    using System;

    using SimpleOrderRouting.Domain.SmartOrderRouting;

    /// <summary>
    /// Trading instruction given to the SOR on the investor-side.
    /// </summary>
    public class InvestorInstruction
    {
        public InvestorInstruction(long investorInstructionIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution, DateTime? goodTill)
        {
            this.Way = way;
            this.Quantity = quantity;
            this.Price = price;
            this.AllowPartialExecution = allowPartialExecution;
            this.GoodTill = goodTill;
        
            this.InvestorInstructionIdentifier = investorInstructionIdentifier;
        }

        public DateTime? GoodTill { get; private set; }

        public bool AllowPartialExecution { get; private set; }

        public decimal Price { get; private set; }

        public int Quantity { get; private set; }

        public Way Way { get; private set; }

        /// <summary>
        /// Occurs when the <see cref="InvestorInstruction"/> is fully executed.
        /// </summary>
        public event EventHandler<OrderExecutedEventArgs> Executed;

        /// <summary>
        /// Occurs when the <see cref="InvestorInstruction"/> has failed.
        /// </summary>
        public event EventHandler<string> Failed;

        public long InvestorInstructionIdentifier { get; private set; }

        /// <summary>
        /// Just a naive implementation to make the test pass. 
        /// Code smell here: with the Executed event raised from outside the InvestorInstruction.
        /// </summary>
        /// <param name="executedQuantity">The executed Quantity.</param>
        /// <param name="executedPrice">The executed Price.</param>
        internal virtual void NotifyOrderExecution(int executedQuantity, decimal executedPrice)
        {
            // instruction fully executed, I notify
            var onExecuted = this.Executed;
            if (onExecuted != null)
            {
                onExecuted(this, new OrderExecutedEventArgs(this.Way, executedQuantity, executedPrice));
            }
        }

        internal virtual void NotifyOrderFailure(string reason)
        {
            // instruction fully executed, I notify
            EventHandler<string> onFailed = this.Failed;
            if (onFailed != null)
            {
                onFailed(this, reason);
            }
        }
    }
}