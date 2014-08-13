// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IMarket.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN, Tomasz JASKULA
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
namespace SimpleOrderRouting.Journey1
{
    using System;

    /// <summary>
    /// Order routing API.
    /// </summary>
    public interface IMarket
    {
        /// <summary>
        /// Occurs when an order is executed.
        /// </summary>
        event EventHandler<OrderExecutedEventArgs> OrderExecuted;

        /// <summary>
        /// Sends an order to a market for its execution.
        /// </summary>
        /// <param name="order">The order to be executed.</param>
        void Send(Order order);
    }
}