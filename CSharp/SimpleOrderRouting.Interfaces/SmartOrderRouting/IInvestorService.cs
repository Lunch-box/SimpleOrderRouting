namespace SimpleOrderRouting.Interfaces.SmartOrderRouting
{
    using System;

    /// <summary>
    /// Bridge API between the SOR and the investor
    /// </summary>
    public interface IInvestorService
    {
        InvestorInstructionIdentifier RequestUniqueIdentifier();

        event EventHandler<InvestorInstructionDto> InvestorInstructionReceived;

        void Update(InvestorInstructionUpdatedEventArgs update);
    }
}