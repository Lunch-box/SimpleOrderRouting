namespace SimpleOrderRouting.Infra
{
    using System;

    using SimpleOrderRouting.Domain;

    /// <summary>
    /// Provide an integration point for all investor side use cases.
    /// </summary>
    public interface ISmartOrderRoutingEntryPoint
    {
        void Subscribe(InvestorInstructionIdentifierDto investorInstructionIdentifierDto, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback);

        void Route(InvestorInstructionDto investorInstructionDto);
    }
}