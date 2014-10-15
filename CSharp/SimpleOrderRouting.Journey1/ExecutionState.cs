// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionState.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Journey1
{
    public class ExecutionState
    {
        public ExecutionState(InvestorInstruction investorInstruction)
        {
            this.Quantity = investorInstruction.Quantity;
            this.Price = investorInstruction.Price;
            this.Way = investorInstruction.Way;
            this.AllowPartialExecution = investorInstruction.AllowPartialExecution;
        }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Way Way { get; set; }

        public bool AllowPartialExecution { get; set; }
        
        public void Executed(int quantity)
        {
            this.Quantity -= quantity;
        }
    }
}