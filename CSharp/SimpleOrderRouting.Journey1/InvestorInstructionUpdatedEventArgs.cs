namespace SimpleOrderRouting.Journey1
{
    using System;

    public class InvestorInstructionUpdatedEventArgs : EventArgs
    {
        public InvestorInstructionStatus Status { get; set; }

        public InvestorInstructionUpdatedEventArgs(InvestorInstructionStatus status)
        {
            this.Status = status;
        }
    }

    public enum InvestorInstructionStatus
    {
        Executed,
        PartiallyExecuted,
        Failed
    }
}