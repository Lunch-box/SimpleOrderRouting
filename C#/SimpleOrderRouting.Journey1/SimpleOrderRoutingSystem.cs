// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SimpleOrderRoutingSystem.cs" company="">
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
    /// Simple (vs. Smart) Order Routing System. Responsible to drive execution strategies for orders.
    /// </summary>
    public class SimpleOrderRoutingSystem
    {
        private readonly IMarketOrderRouting marketOrderRouting;
        private IMarketDataProvider marketDataProvider;

        public SimpleOrderRoutingSystem(IMarketOrderRouting marketOrderRouting, IMarketDataProvider marketDataProvider = null)
        {
            this.marketOrderRouting = marketOrderRouting;
            this.marketDataProvider = marketDataProvider;
            this.marketOrderRouting.DealExecuted += this.MarketDealExecuted;
        }

        public event EventHandler<OrderExecutedEventArgs> OrderExecuted;

        #region Public Methods and Operators

        private void MarketDealExecuted(object sender, DealExecutedEventArgs e)
        {
            var orderExecutedArgs = new OrderExecutedEventArgs();
            this.OnOrderExecuted(orderExecutedArgs);
        }

        protected virtual void OnOrderExecuted(OrderExecutedEventArgs e)
        {
            if (this.OrderExecuted != null)
            {
                this.OrderExecuted(this, e);
            }
        }

        public void Post(OrderRequest orderRequest)
        {
            var order = new Order();
            this.marketOrderRouting.Send(order);
        }

        #endregion
    }
}