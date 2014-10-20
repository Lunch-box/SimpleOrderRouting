namespace SimpleOrderRouting.Interfaces.SmartOrderRouting
{
    using System;

    public class InvestorInstructionUpdatedEventArgs : EventArgs
    {
        public InvestorInstructionIdentifier Identifier { get; set; }

        public InvestorInstructionStatus Status { get; set; }

        public InvestorInstructionUpdatedEventArgs(InvestorInstructionIdentifier identifier, InvestorInstructionStatus status)
        {
            this.Identifier = identifier;
            this.Status = status;
        }
    }
}