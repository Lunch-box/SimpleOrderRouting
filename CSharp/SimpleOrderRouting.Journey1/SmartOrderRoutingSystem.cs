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

namespace SimpleOrderRouting.Journey1
{
    public class SmartOrderRoutingSystem : IDisposable
    {
        private readonly SmartOrderRoutingConfiguration _sorConfig;
        private OrderMessageHandler _orderMessageHandler;

        public SmartOrderRoutingSystem(SmartOrderRoutingConfiguration sorConfig)
        {
            _sorConfig = sorConfig;
        }

        public IDisposable Start()
        {
            _orderMessageHandler = _sorConfig.OrderMessageHandler;
            _orderMessageHandler.MessageHandled += OrderMessageHandlerOnMessageHandled;
            return this;
        }

        public void Dispose()
        {
            _orderMessageHandler.MessageHandled -= OrderMessageHandlerOnMessageHandled;
        }

        private void OrderMessageHandlerOnMessageHandled(object sender, OrderRequestEventArgs orderRequestEventArgs)
        {
        }
    }
}