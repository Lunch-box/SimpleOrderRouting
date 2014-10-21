namespace SimpleOrderRouting.Journey1
{
    using System;

    using SimpleOrderRouting.Interfaces;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting.Investor;

    /// <summary>
    /// External API for the Smart Order Routing service. Aggregates all instruction events.
    /// </summary>
    public class SmartOrderRoutingService : ISmartOrderRoutingService
    {
        private readonly ISmartOrderRouting smartOrderRouting;

        public SmartOrderRoutingService(ISmartOrderRouting smartOrderRouting)
        {
            this.smartOrderRouting = smartOrderRouting;
        }

        public InvestorInstructionIdentifierDto RequestUniqueIdentifier()
        {
            return this.smartOrderRouting.RequestUniqueIdentifier();
        }

        public event EventHandler<InvestorInstructionUpdatedDto> InstructionUpdated;

        public void Send(InvestorInstructionIdentifierDto instructionIdentifierDto, InvestorInstructionDto instruction)
        {
            var internalInstruction = this.smartOrderRouting.CreateInvestorInstruction(instructionIdentifierDto, instruction.Way, instruction.Quantity, instruction.Price, instruction.AllowPartialExecution, instruction.GoodTill);
            internalInstruction.Executed += internalInstruction_Executed;
            internalInstruction.Failed += internalInstruction_Failed;
            this.smartOrderRouting.Route(internalInstruction);

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