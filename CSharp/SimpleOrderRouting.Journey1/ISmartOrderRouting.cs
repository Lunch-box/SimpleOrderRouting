namespace SimpleOrderRouting.Journey1
{
    using System;

    using SimpleOrderRouting.Interfaces;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

    /// <summary>
    /// 
    /// </summary>
    public interface ISmartOrderRouting
    {
        InvestorInstructionIdentifier RequestUniqueIdentifier();

        void Route(InvestorInstruction investorInstruction);

        InvestorInstruction CreateInvestorInstruction(InvestorInstructionIdentifier instructionIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null);
    }
}