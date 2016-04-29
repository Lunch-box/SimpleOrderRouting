namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Provide an integration point for all investor side use cases.
    /// </summary>
    public interface ISmartOrderRoutingEntryPoint
    {
        void Subscribe(InvestorInstruction investorInstruction, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback);

        void Route(InvestorInstruction investorInstruction);
    }
}