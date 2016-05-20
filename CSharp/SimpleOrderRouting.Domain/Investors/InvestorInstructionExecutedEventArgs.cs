using System;

namespace SimpleOrderRouting.Investors
{
    /// <summary>
    /// Event data for InvestorInstructionExecuted event.
    /// </summary>
    public class InvestorInstructionExecutedEventArgs : EventArgs
    {
        public InvestorInstructionExecutedEventArgs(Way way, int quantity, decimal price)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.Way = way;
        }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        public Way Way { get; private set; }
    }
}
