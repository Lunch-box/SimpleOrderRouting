// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISolveInvestorInstructions.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting
{
    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Transforms an <see cref="SimpleOrderRouting.Infra.InvestorInstruction"/> into an <see cref="OrderBasket"/>.
    /// </summary>
    public interface ISolveInvestorInstructions
    {
        // TODO: remove the reference to ICanRouteOrder (currently requested for the OrderBasket creation;-(
        /// <summary>
        /// Build the description of the orders needed to fulfill an <see cref="SimpleOrderRouting.Infra.InvestorInstruction" /> which
        /// is aggregated within an <see cref="InstructionExecutionContext" /> instance.
        /// </summary>
        /// <param name="instructionExecutionContext">The <see cref="InstructionExecutionContext" /> instance that aggregates the <see cref="SimpleOrderRouting.Infra.InvestorInstruction" />.</param>
        /// <param name="canRouteOrders">The can route orders (temp hack that should be removed afterwards).</param>
        /// <returns>
        /// An <see cref="OrderBasket" /> containing all the orders to be routed in order to fulfill the initial <see cref="SimpleOrderRouting.Infra.InvestorInstruction" />.
        /// </returns>
        OrderBasket Solve(InstructionExecutionContext instructionExecutionContext, ICanRouteOrders canRouteOrders);
    }
}