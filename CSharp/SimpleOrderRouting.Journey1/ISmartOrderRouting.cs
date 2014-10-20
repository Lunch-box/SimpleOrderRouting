namespace SimpleOrderRouting.Journey1
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface ISmartOrderRouting
    {
        InvestorInstructionIdentifier RequestUniqueIdentifier();

        void Route(InvestorInstruction investorInstruction);

        InvestorInstruction CreateInvestorInstruction(InvestorInstructionIdentifier instructionIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null);
    }

    public interface ISmartOrderRoutingService
    {
        InvestorInstructionIdentifier RequestUniqueIdentifier();

        /// <summary>
        /// Occurs when an <see cref="InvestorInstruction"/> has been updated.
        /// </summary>
        event EventHandler<InvestorInstructionUpdatedEventArgs> InstructionUpdated;

        void Send(InvestorInstructionIdentifier instructionIdentifier, InvestorInstructionDto investorInstructionDto);
    }
}