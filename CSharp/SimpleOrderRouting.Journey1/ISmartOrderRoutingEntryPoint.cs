namespace SimpleOrderRouting.Journey1
{
    using System;

    using SimpleOrderRouting.Interfaces.Order;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

    /// <summary>
    /// 
    /// </summary>
    public interface ISmartOrderRoutingEntryPoint
    {
        InvestorInstructionIdentifierDto RequestUniqueIdentifier();

        void Route(InvestorInstruction investorInstruction);

        InvestorInstruction CreateInvestorInstruction(InvestorInstructionIdentifierDto instructionIdentifierDto, InstrumentIdentifier instrumentIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null);
    }
}