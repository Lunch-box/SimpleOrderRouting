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

using System.Collections.Generic;

namespace SimpleOrderRouting.Journey1
{
    /// <summary>
    /// This is the external component that publishes messages to the SOR system.
    /// In a real life it could be a MOM component like RabbitMq, Tibco EMS or whatever.
    /// </summary>
    public class OrderMessageHub
    {
        private readonly List<IMessageHandler> _handlers = new List<IMessageHandler>();

        public void Publish(Message message)
        {
            foreach (var messageHandler in _handlers)
            {
                messageHandler.HandleMessage(message);
            }
        }

        public void Subscribe(IMessageHandler messageHandler)
        {
            _handlers.Add(messageHandler);
        }
    }

    public interface IMessageHandler
    {
        void HandleMessage(Message message);
    }
}