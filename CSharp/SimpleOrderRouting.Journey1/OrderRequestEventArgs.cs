namespace SimpleOrderRouting.Journey1
{
    public class OrderRequestEventArgs
    {
        public OrderRequestEventArgs(OrderRequest orderRequest)
        {
            OrderRequest = orderRequest;
        }

        public OrderRequest OrderRequest { get; private set; }
    }
}