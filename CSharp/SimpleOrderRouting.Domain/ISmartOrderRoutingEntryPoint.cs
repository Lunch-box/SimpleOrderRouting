namespace SimpleOrderRouting.Domain
{
    using System;

    using SimpleOrderRouting.Domain.SmartOrderRouting;

    /// <summary>
    /// Provide an integration point for all investor side use cases.
    /// </summary>
    public interface ISmartOrderRoutingEntryPoint
    {
        InvestorInstructionIdentifierDto RequestUniqueIdentifier();

        void Subscribe(InvestorInstructionIdentifierDto investorInstructionIdentifierDto, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback);

        void Route(InvestorInstructionDto investorInstructionDto);
    }
}