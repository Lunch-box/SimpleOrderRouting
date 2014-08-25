using System;

namespace SimpleOrderRouting.Journey1
{
    public class OrderMessageHandler : IMessageHandler
    {
        private readonly Func<Message, OrderRequest> _orderRequestFactory;

        public OrderMessageHandler(Func<Message, OrderRequest> orderRequestFactory)
        {
            _orderRequestFactory = orderRequestFactory;
        }

        public event EventHandler<OrderRequestEventArgs> MessageHandled;

        protected virtual void OnMessageHandled(OrderRequest e)
        {
            var handler = MessageHandled;
            if (handler != null) handler(this, new OrderRequestEventArgs(e));
        }

        public void HandleMessage(Message message)
        {
            var orderRequest = _orderRequestFactory(message);
            OnMessageHandled(orderRequest);
        }
    }
}