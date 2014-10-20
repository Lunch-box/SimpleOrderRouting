// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrder.cs" company="LunchBox corp">
//    Copyright 2014 The Lunch-Box mob: Ozgur DEVELIOGLU (@Zgurrr), Cyrille  DUPUYDAUBY 
//    (@Cyrdup), Tomasz JASKULA (@tjaskula), Thomas PIERRAIN (@tpierrain)
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace SimpleOrderRouting.Journey1
{
    using System;

    using SimpleOrderRouting.Interfaces;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

    public interface IOrder
    {
        event EventHandler<DealExecutedEventArgs> OrderExecuted;

        // TODO: Maybe not necessary if we change the Send() signature to inlcude the notification.
        event EventHandler<OrderFailedEventArgs> OrderFailed;

        Way Way { get; }

        int Quantity { get; }

        bool AllowPartialExecution { get; }

        Market Market { get; }

        void Send();
    }
}