namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Allows to create and route Orders on Market venues.
    /// </summary>
    public interface ICanRouteOrders
    {
        IOrder CreateMarketOrder(OrderDescription orderDescription);

        IOrder CreateLimitOrder(OrderDescription orderDescription);

        void Route(OrderBasket basketOrder);

        /// <summary>
        /// Occurs when one order is executed.
        /// </summary>
        event EventHandler<DealExecutedEventArgs> OrderExecuted;

        /// <summary>
        /// Occurs when one order failed.
        /// </summary>
        event EventHandler<OrderFailedEventArgs> OrderFailed;
    }
}