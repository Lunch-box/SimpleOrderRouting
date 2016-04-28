namespace SimpleOrderRouting.Infra
{
    using System;

    using SimpleOrderRouting.Domain;
    using SimpleOrderRouting.Domain.Order;
    using SimpleOrderRouting.Domain.SmartOrderRouting;

    /// <summary>
    /// External API for the Smart Order Routing service. Aggregates all instruction events.
    /// (Hexagonal) Adapter from the infrastructure code to the domain code and vice-versa.
    /// </summary>
    public class InvestorInstructionsAdapter
    {
        private readonly SmartOrderRoutingEngine sor;

        public InvestorInstructionsAdapter(SmartOrderRoutingEngine sor)
        {
            this.sor = sor;
        }

        public event EventHandler<InvestorInstructionUpdatedDto> InstructionUpdated;

        public void Route(InvestorInstructionDto investorInstructionDto)
        {
            // 1. Adapt from infra to domain
            var investorInstruction = this.CreateInvestorInstruction(investorInstructionDto.UniqueIdentifier, null, investorInstructionDto.Way, investorInstructionDto.Quantity, investorInstructionDto.Price, investorInstructionDto.AllowPartialExecution, investorInstructionDto.GoodTill);
            this.sor.Route(investorInstruction);
        }

        private InvestorInstruction CreateInvestorInstruction(InvestorInstructionIdentifierDto instructionIdentifierDto, InstrumentIdentifier instrumentIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null)
        {
            return new InvestorInstruction(instructionIdentifierDto.Value, way, quantity, price, allowPartialExecution, goodTill);
        }

        public void Subscribe(InvestorInstructionIdentifierDto investorInstructionIdentifierDto, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback)
        {
            // TODO: thread-safe it
            this.executionCallbacks[investorInstructionIdentifierDto] = executedCallback;
            this.failureCallbacks[investorInstructionIdentifierDto] = failureCallback;
            // TODO: regirst failure
        }

        private void internalInstruction_Failed(string e)
        {
            var onInstructionUpdated = this.InstructionUpdated;
            if (onInstructionUpdated != null)
            {
                // TODO: retrieve investor instruction
                InvestorInstruction instruction = null;
                //var instruction = (InvestorInstruction)sender;
                onInstructionUpdated(this, new InvestorInstructionUpdatedDto(instruction.InvestorInstructionIdentifier, InvestorInstructionStatus.Failed));
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
                onInstructionUpdated(this, new InvestorInstructionUpdatedDto(instruction.InvestorInstructionIdentifier, InvestorInstructionStatus.Executed));
            }
        }
    }
}