namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Orders;

    public interface ICanRouteOrders
    {
        //IOrder CreateMarketOrder(Way buy, int quantity);
        //IOrder CreateLimitOrder(Way way, decimal price, int quantity, bool allowPartialExecution);

        IOrder CreateMarketOrder(OrderDescription orderDescription);

        IOrder CreateLimitOrder(OrderDescription orderDescription);

        void Route(IOrder order);

        void Route(OrderBasket basketOrder);

        event EventHandler<DealExecutedEventArgs> OrderExecuted;

        event EventHandler<OrderFailedEventArgs> OrderFailed;
    }
}