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
        InvestorInstructionIdentifierDto RequestUniqueIdentifier();

        void Route(InvestorInstruction investorInstruction);

        InvestorInstruction CreateInvestorInstruction(InvestorInstructionIdentifierDto instructionIdentifierDto, InstrumentIdentifier instrumentIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null);
    }
}