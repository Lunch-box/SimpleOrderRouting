namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Provides an integration point for all investor side use cases.
    /// </summary>
    public interface IHandleInvestorInstructions
    {
        void Route(InvestorInstruction investorInstruction, Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback);
    }
}