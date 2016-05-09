namespace SimpleOrderRouting.Infra
{
    using System;

    using SimpleOrderRouting.Markets.Orders;

    public class InvestorInstructionDtoCallBacks
    {
        private readonly Action<OrderExecutedEventArgs> outsideExecutedCallback;
        private readonly Action<string> outsideFailureCallback;

        public InvestorInstructionDtoCallBacks(Action<OrderExecutedEventArgs> outsideExecutedCallback, Action<string> outsideFailureCallback)
        {
            this.outsideExecutedCallback = outsideExecutedCallback;
            this.outsideFailureCallback = outsideFailureCallback;
        }

        public void ExecutedCallback(OrderExecutedEventArgs args)
        {
            // Adapts to the inside (domain) model to the outside one
            // TODO: create dedicated signature/types for outside callback
            this.outsideExecutedCallback(args);
        }

        public void FailedCallbacks(string reason)
        {
            // Adapts to the inside (domain) model to the outside one
            // TODO: create dedicated signature/types for outside callback
            this.outsideFailureCallback(reason);
        }
    }
}