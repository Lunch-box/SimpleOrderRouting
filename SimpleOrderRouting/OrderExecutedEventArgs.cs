// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="OrderExecutedEventArgs.cs" company="">
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
namespace SimpleOrderRouting
{
    using System;

    /// <summary>
    /// Event data for OrderExecuted event.
    /// </summary>
    public class OrderExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderExecutedEventArgs"/> class.
        /// </summary>
        /// <param name="marketIdentifier">The identifier of the market where the order has been executed.</param>
        /// <param name="orderIdentifier">The identifier of the executed order.</param>
        public OrderExecutedEventArgs(string marketIdentifier, string orderIdentifier)
        {
            this.MarketIdentifier = marketIdentifier;
            this.OrderIdentifier = orderIdentifier;
        }

        /// <summary>
        /// Gets the identifier of the market where the order has been executed.
        /// </summary>
        /// <value>
        /// The identifier of the market where the order has been executed.
        /// </value>
        public string MarketIdentifier { get; private set; }

        /// <summary>
        /// Gets the identifier of the executed order.
        /// </summary>
        /// <value>
        /// The identifier of the executed order.
        /// </value>
        public string OrderIdentifier { get; private set; }
    }
}