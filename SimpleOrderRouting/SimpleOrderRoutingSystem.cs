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
namespace SimpleOrderRouting
{
    using System;

    /// <summary>
    /// Simple (vs. Smart) Order Routing System. Responsible to drive execution strategies for orders.
    /// </summary>
    public class SimpleOrderRoutingSystem
    {
        private readonly IMarket market;

        public SimpleOrderRoutingSystem(IMarket market)
        {
            this.market = market;
            this.market.OrderExecuted += this.Market_OrderExecuted;
        }

        public event EventHandler<OrderExecutedEventArgs> OrderExecuted;

        #region Public Methods and Operators

        private void Market_OrderExecuted(object sender, OrderExecutedEventArgs e)
        {
            this.OnOrderRequested(e);
        }

        protected virtual void OnOrderRequested(OrderExecutedEventArgs e)
        {
            if (this.OrderExecuted != null)
            {
                this.OrderExecuted(this, e);
            }
        }

        public void Post(OrderRequest orderRequest)
        {
            var order = new Order();
            this.market.Send(order);
        }

        #endregion
    }
}