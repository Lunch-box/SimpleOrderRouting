namespace SimpleOrderRouting.Journey1
{
    using System.Collections.Generic;

    public interface IInvestorInstructionSolver
    {
        /// <summary>
        /// Build the description of the orders needed to fulfill the <see cref="investorInstruction"/>
        /// </summary>
        /// <param name="investorInstruction"></param>
        /// <returns>Order description</returns>
        IEnumerable<OrderDescription> Solve(InvestorInstruction investorInstruction);
    }
}