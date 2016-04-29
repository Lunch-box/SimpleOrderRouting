// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionExecutionContext.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Investors
{
    using System;

    /// <summary>
    /// 1 to 1 relationship with an <see cref="SimpleOrderRouting.Infra.InvestorInstruction"/>.  Keeps the current state of the instruction execution.
    /// </summary>
    public class InstructionExecutionContext
    {
        private readonly InvestorInstruction investorInstruction;

        private readonly int initialQuantity;

        public InstructionExecutionContext(InvestorInstruction investorInstruction)
        {
            this.investorInstruction = investorInstruction;
            this.initialQuantity = investorInstruction.Quantity;
            this.Quantity = investorInstruction.Quantity;
            this.Price = investorInstruction.Price;
            this.Way = investorInstruction.Way;
            this.AllowPartialExecution = investorInstruction.AllowPartialExecution;
        }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        public Way Way { get; private set; }

        public bool AllowPartialExecution { get; private set; }

        /// <summary>
        /// Called when an order has been executed - called by the order basket.
        /// </summary>
        /// <param name="quantity">The executed quantity.</param>
        public void Executed(int quantity)
        {
            this.Quantity -= quantity;
            if (this.Quantity == 0)
            {
                this.investorInstruction.NotifyOrderExecution(this.initialQuantity, this.Price);
            }
            else if (this.Quantity < 0)
            {
                throw new ApplicationException("Executed more than specified in the investor instruction.");
            }
        }
    }
}