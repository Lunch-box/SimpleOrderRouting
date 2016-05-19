namespace OtherTeam.StandardizedMarketGatewayAPI
{
    using System;

    /// <summary>
    /// Gives access and mocks a Market given various initialization informations.
    /// </summary>
    public class ApiMarketGateway
    {
        public ApiMarketGateway(string marketName, int sellQuantity, decimal sellPrice, Predicate<ApiOrder> orderPredicate = null)
        {
            this.MarketName = marketName;
            this.SellQuantity = sellQuantity;
            this.SellPrice = sellPrice;
            this.OrderPredicate = orderPredicate;
        }

        public event EventHandler<ApiMarketDataUpdateEventArgs> MarketDataUpdated;

        public event EventHandler<ApiDealExecutedEventArgs> OrderExecuted;

        // TODO: set a proper event handler instead of this string
        public event EventHandler<ApiOrderFailedEventArgs> OrderFailed;

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

            if (this.PredicateFailed(marketOrder))
            {
                this.RaiseOrderFailed(marketOrder, new ApiOrderFailedEventArgs(this.MarketName, "Predicate failed."));
                return;
            }

            switch (marketOrder.Way)
            {
                case ApiMarketWay.Buy:
                    if (this.AskMoreThanAvailableQuantityAndDontSupportPartialExecution(marketOrder))
                    {
                        this.RaiseOrderFailed(marketOrder, new ApiOrderFailedEventArgs(this.MarketName, "Excessive quantity!"));
                        return;
                    }

                    this.ExecuteProperQuantity(marketOrder);

                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private void ExecuteProperQuantity(ApiOrder marketOrder)
        {
            var executedQuantity = Math.Min(marketOrder.Quantity, this.SellQuantity);
            this.SellQuantity -= executedQuantity;

            this.RaiseMarketDataUpdated(this.SellPrice, this.SellQuantity);
            this.RaiseOrderExecuted(marketOrder, executedQuantity);
        }

        private bool PredicateFailed(ApiOrder marketOrder)
        {
            return this.OrderPredicate != null && this.OrderPredicate(marketOrder) == false;
        }

        public void Send(ApiLimitOrder limitOrder)
        {
            this.TimesSent++;

            if (this.PredicateFailed(limitOrder))
            {
                this.RaiseOrderFailed(limitOrder, new ApiOrderFailedEventArgs(this.MarketName, "Predicate failed."));
                return;
            }

            switch (limitOrder.Way)
            {
                case ApiMarketWay.Buy:
                    if (limitOrder.Price > this.SellPrice)
                    {
                        this.RaiseOrderFailed(limitOrder, new ApiOrderFailedEventArgs(this.MarketName, "Invalid price"));
                        return;
                    }

                    if (this.AskMoreThanAvailableQuantityAndDontSupportPartialExecution(limitOrder))
                    {
                        this.RaiseOrderFailed(limitOrder, new ApiOrderFailedEventArgs(this.MarketName, "Excessive quantity!"));
                        return;
                    }

                    this.ExecuteProperQuantity(limitOrder);

                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private bool AskMoreThanAvailableQuantityAndDontSupportPartialExecution(ApiOrder limitOrder)
        {
            return (limitOrder.Quantity > this.SellQuantity) && (!limitOrder.AllowPartialExecution);
        }

        private void RaiseMarketDataUpdated(decimal newSellPrice, int newQuantityOnTheMarket)
        {
            var marketDataUpdated = this.MarketDataUpdated;
            if (marketDataUpdated != null)
            {
                marketDataUpdated(this, new ApiMarketDataUpdateEventArgs(this.MarketName, newSellPrice, newQuantityOnTheMarket));
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

        private void RaiseOrderFailed(ApiOrder order, ApiOrderFailedEventArgs args)
        {
            var onOrderFailed = this.OrderFailed;
            if (onOrderFailed != null)
            {
                onOrderFailed(order, args);
            }
        }

        
    }
}