namespace SimpleOrderRouting
{
    using System;

    public class SimpleOrderRoutingSystem
    {
        private IMarket market;

        public SimpleOrderRoutingSystem(IMarket market)
        {
            this.market = market;
        }

        public event EventHandler<OrderExecutedEventArgs> OrderExecuted;

        #region Public Methods and Operators

        public void Post(OrderRequest orderRequest)
        {
            var order = new Order();
            this.market.Send(order);
        }

        #endregion
    }
}