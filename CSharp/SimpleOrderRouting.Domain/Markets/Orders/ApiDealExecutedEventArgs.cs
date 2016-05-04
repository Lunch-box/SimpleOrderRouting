namespace SimpleOrderRouting.Markets.Orders
{
    using System;

    /// <summary>
    /// Event data for DealExecuted event.
    /// </summary>
    public class ApiDealExecutedEventArgs : EventArgs
    {
        public ApiDealExecutedEventArgs(decimal price, int quantity)
        {
            this.Price = price;
            this.Quantity = quantity;
        }

        public decimal Price { get; private set; }

        public int Quantity { get; private set; }
    }
}