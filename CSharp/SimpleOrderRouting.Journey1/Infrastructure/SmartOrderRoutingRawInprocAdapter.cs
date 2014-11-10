namespace SimpleOrderRouting.Journey1.Infrastructure
{
    using System;

    using SimpleOrderRouting.Interfaces.Order;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

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

        public void Send(InvestorInstructionDto instruction)
        {
            var internalInstruction = this.smartOrderRoutingEntryPoint.CreateInvestorInstruction(instruction.UniqueIdentifier, new InstrumentIdentifier(instruction.InstrumentIdentifier), instruction.Way, instruction.Quantity, instruction.Price, instruction.AllowPartialExecution, instruction.GoodTill);
            internalInstruction.Executed += this.internalInstruction_Executed;
            internalInstruction.Failed += this.internalInstruction_Failed;
            this.smartOrderRoutingEntryPoint.Route(internalInstruction);

            // TODO: unsubscribe
        }

        private void internalInstruction_Failed(object sender, string e)
        {
            var onInstructionUpdated = this.InstructionUpdated;
            if (onInstructionUpdated != null)
            {
                var instruction = (InvestorInstruction)sender;
                onInstructionUpdated(this, new InvestorInstructionUpdatedDto(instruction.IdentifierDto, InvestorInstructionStatus.Failed));
            }
        }

        void internalInstruction_Executed(object sender, OrderExecutedEventArgs e)
        {
            var onInstructionUpdated = this.InstructionUpdated;
            if (onInstructionUpdated != null)
            {
                var instruction = (InvestorInstruction)sender;
                onInstructionUpdated(this, new InvestorInstructionUpdatedDto(instruction.IdentifierDto, InvestorInstructionStatus.Executed));
            }
        }
    }
}