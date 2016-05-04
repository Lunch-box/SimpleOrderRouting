namespace OtherTeam.StandardizedMarketGatewayAPI
{
    using System;

    public class ApiMarketGateway
    {
        public ApiMarketGateway(string name, int sellQuantity, decimal sellPrice)
        {
            this.Name = name;
            this.SellQuantity = sellQuantity;
            this.SellPrice = sellPrice;
        }

        public event EventHandler<ApiDealExecutedEventArgs> OrderExecuted;

        public event EventHandler<string> OrderFailed;

        public string Name { get; private set; }

        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }

        public int TimesSent { get; private set; }

        public Predicate<ApiMarketOrder> OrderPredicate { get; set; }

        public ApiMarketOrder CreateMarketOrder(ApiMarketWay way, int quantity)
        {
            return new ApiMarketOrder(way, quantity);
        }

        public void Send(ApiMarketOrder order)
        {
            this.TimesSent++;

            if (this.OrderPredicate != null && this.OrderPredicate(order) == false)
            {
                this.RaiseOrderFailed(order, "Predicate failed.");
                return;
            }

            switch (order.Way)
            {
                case ApiMarketWay.Buy:
                    // TODO: restore that sh# and migrate Market tests to ApiMarketGatewatTests
                    //if (order is LimitOrder)
                    //{
                    // TODO: use polymorphism here instead
                    //    var limitOrder = order as LimitOrder;
                    //    if (limitOrder.Price > this.SellPrice)
                    //    {
                    //        this.RaiseOrderFailed(order, "Invalid price");
                    //        return;
                    //    }
                    //}

                    if (order.Quantity > this.SellQuantity)
                    {
                        if (!order.AllowPartialExecution)
                        {
                            this.RaiseOrderFailed(order, "Excessive quantity!");
                            return;
                        }
                    }

                    var executedQuantity = Math.Min(order.Quantity, this.SellQuantity);
                    this.SellQuantity -= executedQuantity;

                    this.RaiseOrderExecuted(order, executedQuantity);

                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private void RaiseOrderExecuted(ApiMarketOrder order, int executedQuantity)
        {
            var onOrderExecuted = this.OrderExecuted;
            if (onOrderExecuted != null)
            {
                onOrderExecuted(order, new ApiDealExecutedEventArgs(this.SellPrice, executedQuantity));
            }
        }

        private void RaiseOrderFailed(ApiMarketOrder order, string reason)
        {
            var onOrderFailed = this.OrderFailed;
            if (onOrderFailed != null)
            {
                onOrderFailed(order, reason);
            }
        }

    }
}