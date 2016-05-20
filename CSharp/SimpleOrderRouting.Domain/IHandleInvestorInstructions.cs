namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Investors;

    /// <summary>
    /// Provides an integration point for all investor side use cases.
    /// </summary>
    public interface IHandleInvestorInstructions
    {
        void Route(InvestorInstruction investorInstruction, Action<InvestorInstructionExecutedEventArgs> executedCallback, Action<string> failureCallback);
    }
}