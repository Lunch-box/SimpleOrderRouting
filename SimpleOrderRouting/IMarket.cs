namespace SimpleOrderRouting
{
    using System;

    public interface IMarket
    {
        void Send(Order order);

        event EventHandler<OrderExecutedEventArgs> OrderExecuted;
    }
}