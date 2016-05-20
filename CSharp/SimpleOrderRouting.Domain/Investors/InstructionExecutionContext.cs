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

    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// 1 to 1 relationship with an <see cref="SimpleOrderRouting.Investors.InvestorInstruction"/>.  Keeps the current state of the instruction execution.
    /// <remarks>Entity</remarks>
    /// </summary>
    public class InstructionExecutionContext
    {
        private readonly InvestorInstruction investorInstruction;

        private readonly Action<OrderExecutedEventArgs> orderExecutedCallBack;

        private readonly int initialQuantity;

        public InstructionExecutionContext(InvestorInstruction investorInstruction, Action<OrderExecutedEventArgs> orderExecutedCallBack)
        {
            this.investorInstruction = investorInstruction;
            this.orderExecutedCallBack = orderExecutedCallBack;
            this.initialQuantity = investorInstruction.Quantity;
            this.RemainingQuantityToBeExecuted = investorInstruction.Quantity;
            this.Price = investorInstruction.Price;
            this.Way = investorInstruction.Way;
            this.AllowPartialExecution = investorInstruction.AllowPartialExecution;
        }

        public int RemainingQuantityToBeExecuted { get; private set; }

        public decimal Price { get; private set; }

        public Way Way { get; private set; }

        public bool AllowPartialExecution { get; private set; }

        /// <summary>
        /// Called when an order has been executed.
        /// </summary>
        /// <param name="quantity">The executed quantity.</param>
        public void DeclareOrderExecution(int quantity)
        {
            var previousRemainingQuantityToBeExecuted = this.RemainingQuantityToBeExecuted;
            
            this.RemainingQuantityToBeExecuted -= quantity;
            
            if (this.RemainingQuantityToBeExecuted == 0)
            {
                this.orderExecutedCallBack(new OrderExecutedEventArgs(this.Way, this.initialQuantity, this.Price));
            }
            else if (this.RemainingQuantityToBeExecuted < 0)
            {
                throw new ApplicationException(string.Format("Executed more than the investor instruction has requested. Previous remaining quantity to be executed: {0}, latest executed quantity: {1}. New remaining quantity to be executed: {2}.", previousRemainingQuantityToBeExecuted, quantity, this.RemainingQuantityToBeExecuted));
            }
        }
    }
}