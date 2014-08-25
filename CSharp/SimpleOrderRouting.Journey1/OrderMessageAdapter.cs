// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SmartOrderRoutingSystem.cs" company="">
// //   Copyright 2014 The Lunch-Box mob: Ozgur DEVELIOGLU (@Zgurrr), Cyrille  DUPUYDAUBY 
// //   (@Cyrdup), Tomasz JASKULA (@tjaskula), Thomas PIERRAIN (@tpierrain)
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using SimpleOrderRouting.Journey1.ExternalMessageContext;

namespace SimpleOrderRouting.Journey1
{
    /// <summary>
    /// It's an adapter between external world and the sor handling logic.
    /// </summary>
    public class OrderMessageAdapter : IMessageHandler
    {
        private readonly Func<Message, OrderRequest> _orderRequestFactory;

        public OrderMessageAdapter(Func<Message, OrderRequest> orderRequestFactory)
        {
            _orderRequestFactory = orderRequestFactory;
        }

        internal event EventHandler<OrderRequestEventArgs> MessageHandled;

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