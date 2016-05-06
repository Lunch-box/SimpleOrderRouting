// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDescription.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Markets.Orders
{
    /// <summary>
    /// Caracteristics of an Order to be passed on a market.
    /// </summary>
    public struct OrderDescription
    {
        public string TargetMarketName;

        public Way OrderWay;

        public int Quantity { get; private set; }

        public decimal OrderPrice;

        public bool AllowPartialExecution;

        public OrderDescription(string targetMarketName, Way orderWay, int quantity, decimal orderPrice, bool allowPartialExecution)
            : this()
        {
            this.TargetMarketName = targetMarketName;
            this.OrderWay = orderWay;
            this.Quantity = quantity;
            this.OrderPrice = orderPrice;
            this.AllowPartialExecution = allowPartialExecution;
        }

        public override string ToString()
        {
            return string.Format("Order description: TargetMarketName={0} - quantity={1} - Way={2} - Price={3} - AllowPartialExecution={4}.", this.TargetMarketName, this.Quantity, this.OrderWay, this.OrderPrice, this.AllowPartialExecution);
        }
    }
}