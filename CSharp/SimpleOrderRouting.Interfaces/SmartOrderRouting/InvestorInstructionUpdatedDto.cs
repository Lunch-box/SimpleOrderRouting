namespace SimpleOrderRouting.Interfaces.SmartOrderRouting
{
    using System;

    public class InvestorInstructionUpdatedDto : EventArgs
    {
        public InvestorInstructionIdentifierDto IdentifierDto { get; set; }

        public InvestorInstructionStatus Status { get; set; }

        public InvestorInstructionUpdatedDto(InvestorInstructionIdentifierDto identifierDto, InvestorInstructionStatus status)
        {
            this.IdentifierDto = identifierDto;
            this.Status = status;
        }
    }
}