namespace SimpleOrderRouting.Interfaces.SmartOrderRouting.Investor
{
    using System;

    public interface ISmartOrderRoutingService
    {
        /// <summary>
        /// Get a new identifier for an investor instruction
        /// </summary>
        /// <returns></returns>
        InvestorInstructionIdentifier RequestUniqueIdentifier();

        /// <summary>
        /// Occurs when an <see cref="SimpleOrderRouting.Journey1.InvestorInstruction"/> has been updated.
        /// </summary>
        event EventHandler<InvestorInstructionUpdatedEventArgs> InstructionUpdated;

        /// <summary>
        /// Send an instruction to the smart order routing service to be executed.
        /// </summary>
        /// <param name="instructionIdentifier"></param>
        /// <param name="investorInstructionDto"></param>
        void Send(InvestorInstructionIdentifier instructionIdentifier, InvestorInstructionDto investorInstructionDto);
    }
}