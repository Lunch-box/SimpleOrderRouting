namespace SimpleOrderRouting.Journey1
{
    using System;

    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

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