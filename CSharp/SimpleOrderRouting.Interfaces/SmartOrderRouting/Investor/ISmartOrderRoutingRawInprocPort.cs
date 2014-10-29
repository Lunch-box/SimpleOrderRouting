namespace SimpleOrderRouting.Interfaces.SmartOrderRouting.Investor
{
    using System;

    public interface ISmartOrderRoutingRawInprocPort
    {
        /// <summary>
        /// Get a new IdentifierDto for an investor instruction
        /// </summary>
        /// <returns></returns>
        InvestorInstructionIdentifierDto RequestUniqueIdentifier();

        /// <summary>
        /// Occurs when an <see cref="SimpleOrderRouting.Journey1.InvestorInstruction"/> has been updated.
        /// </summary>
        event EventHandler<InvestorInstructionUpdatedDto> InstructionUpdated;

        /// <summary>
        /// Send an instruction to the smart order routing service to be executed.
        /// </summary>
        /// <param name="instructionIdentifierDto"></param>
        /// <param name="investorInstructionDto"></param>
        void Send(InvestorInstructionIdentifierDto instructionIdentifierDto, InvestorInstructionDto investorInstructionDto);
    }
}