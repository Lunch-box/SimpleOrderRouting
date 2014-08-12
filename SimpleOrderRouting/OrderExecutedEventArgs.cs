namespace SimpleOrderRouting
{
    using System;

    public abstract class OrderExecutedEventArgs : EventArgs
    {
        public string OrderIdentifier { get; private set; }
    }
}