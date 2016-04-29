// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketInfo.cs" company="LunchBox corp">
//     Copyright 2014 The Lunch-Box mob: 
//           Ozgur DEVELIOGLU (@Zgurrr)
//           Cyrille  DUPUYDAUBY (@Cyrdup)
//           Tomasz JASKULA (@tjaskula)
//           Mendel MONTEIRO-BECKERMAN (@MendelMonteiro)
//           Thomas PIERRAIN (@tpierrain)
//     
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//         http://www.apache.org/licenses/LICENSE-2.0
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace SimpleOrderRouting
{
    /// <summary>
    /// Keeps information about a given Market (e.g. # of failures, etc.).
    /// </summary>
    public class MarketInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketInfo"/> class.
        /// </summary>
        /// <param name="market">The market.</param>
        public MarketInfo(Market market)
        {
            this.Market = market;
        }

        /// <summary>
        /// Gets the corresponding market.
        /// </summary>
        /// <value>
        /// The corresponding market.
        /// </value>
        public Market Market { get; private set; }

        /// <summary>
        /// Gets or sets the number of failures for orders we received from this market.
        /// </summary>
        /// <value>
        /// The number of failures for orders we received from this market.
        /// </value>
        public int OrdersFailureCount { get; set; }
    }
}