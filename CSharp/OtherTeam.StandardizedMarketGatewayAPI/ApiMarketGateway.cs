namespace OtherTeam.StandardizedMarketGatewayAPI
{
    using System;

    public class ApiMarketGateway
    {
        public ApiMarketGateway(string marketName, int sellQuantity, decimal sellPrice)
        {
            this.MarketName = marketName;
            this.SellQuantity = sellQuantity;
            this.SellPrice = sellPrice;
        }

        public event EventHandler<ApiDealExecutedEventArgs> OrderExecuted;

        // TODO: set a proper event handler instead of this string
        public event EventHandler<string> OrderFailed;

        public string MarketName { get; private set; }

        public int SellQuantity { get; private set; }

        public decimal SellPrice { get; private set; }

        public int TimesSent { get; private set; }

        public Predicate<ApiOrder> OrderPredicate { get; set; }

        public ApiMarketOrder CreateMarketOrder(ApiMarketWay way, int quantity)
        {
            return new ApiMarketOrder(way, quantity);
        }

        public ApiLimitOrder CreateLimitOrder(ApiMarketWay apiMarketWay, int quantity, decimal price, bool allowPartial)
        {
            return new ApiLimitOrder(apiMarketWay, quantity, price, allowPartial);
        }

        public void Send(ApiMarketOrder marketOrder)
        {
            this.TimesSent++;

            if (this.OrderPredicate != null && this.OrderPredicate(marketOrder) == false)
            {
                this.RaiseOrderFailed(marketOrder, "Predicate failed.");
                return;
            }

            switch (marketOrder.Way)
            {
                case ApiMarketWay.Buy:
                    // TODO: restore that sh# and migrate Market tests to ApiMarketGatewatTests
                    if (marketOrder.Quantity > this.SellQuantity)
                    {
                        if (!marketOrder.AllowPartialExecution)
                        {
                            this.RaiseOrderFailed(marketOrder, "Excessive quantity!");
                            return;
                        }
                    }

                    var executedQuantity = Math.Min(marketOrder.Quantity, this.SellQuantity);
                    this.SellQuantity -= executedQuantity;

                    this.RaiseOrderExecuted(marketOrder, executedQuantity);

                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        public void Send(ApiLimitOrder limitOrder)
        {
            this.TimesSent++;

            if (this.OrderPredicate != null && this.OrderPredicate(limitOrder) == false)
            {
                this.RaiseOrderFailed(limitOrder, "Predicate failed.");
                return;
            }

            switch (limitOrder.Way)
            {
                case ApiMarketWay.Buy:
                    if (limitOrder.Price > this.SellPrice)
                    {
                        this.RaiseOrderFailed(limitOrder, "Invalid price");
                        return;
                    }

                    if (limitOrder.Quantity > this.SellQuantity)
                    {
                        if (!limitOrder.AllowPartialExecution)
                        {
                            this.RaiseOrderFailed(limitOrder, "Excessive quantity!");
                            return;
                        }
                    }

                    var executedQuantity = Math.Min(limitOrder.Quantity, this.SellQuantity);
                    this.SellQuantity -= executedQuantity;

                    this.RaiseOrderExecuted(limitOrder, executedQuantity);

                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }
        private void RaiseOrderExecuted(ApiOrder order, int executedQuantity)
        {
            var onOrderExecuted = this.OrderExecuted;
            if (onOrderExecuted != null)
            {
                onOrderExecuted(order, new ApiDealExecutedEventArgs(this.SellPrice, executedQuantity));
            }
        }

        private void RaiseOrderFailed(ApiOrder order, string reason)
        {
            var onOrderFailed = this.OrderFailed;
            if (onOrderFailed != null)
            {
                onOrderFailed(order, reason);
            }
        }

        
    }
}