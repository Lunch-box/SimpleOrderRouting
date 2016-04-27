namespace SimpleOrderRouting.Infra
{
    using System;

    using SimpleOrderRouting.Domain;
    using SimpleOrderRouting.Domain.SmartOrderRouting;

    /// <summary>
    /// External API for the Smart Order Routing service. Aggregates all instruction events.
    /// </summary>
    public class SmartOrderRoutingRawInprocAdapter
    {
        private readonly ISmartOrderRoutingEntryPoint smartOrderRoutingEntryPoint;

        public SmartOrderRoutingRawInprocAdapter(ISmartOrderRoutingEntryPoint smartOrderRoutingEntryPoint)
        {
            this.smartOrderRoutingEntryPoint = smartOrderRoutingEntryPoint;
        }

        public InvestorInstructionIdentifierDto RequestUniqueIdentifier()
        {
            return this.smartOrderRoutingEntryPoint.RequestUniqueIdentifier();
        }

        public event EventHandler<InvestorInstructionUpdatedDto> InstructionUpdated;

        private void internalInstruction_Failed(string e)
        {
            var onInstructionUpdated = this.InstructionUpdated;
            if (onInstructionUpdated != null)
            {
                // TODO: retrieve investor instruction
                InvestorInstruction instruction = null;
                //var instruction = (InvestorInstruction)sender;
                onInstructionUpdated(this, new InvestorInstructionUpdatedDto(instruction.IdentifierDto, InvestorInstructionStatus.Failed));
            }
        }

        void internalInstruction_Executed(OrderExecutedEventArgs e)
        {
            var onInstructionUpdated = this.InstructionUpdated;
            if (onInstructionUpdated != null)
            {
                // TODO: retrieve investor instruction
                InvestorInstruction instruction = null;
                //var instruction = (InvestorInstruction)sender;
                onInstructionUpdated(this, new InvestorInstructionUpdatedDto(instruction.IdentifierDto, InvestorInstructionStatus.Executed));
            }
        }
    }
}