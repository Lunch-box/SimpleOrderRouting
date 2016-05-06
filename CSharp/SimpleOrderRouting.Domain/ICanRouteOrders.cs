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

        event EventHandler<DealExecutedEventArgs> OrderExecuted;

        event EventHandler<OrderFailedEventArgs> OrderFailed;
    }
}