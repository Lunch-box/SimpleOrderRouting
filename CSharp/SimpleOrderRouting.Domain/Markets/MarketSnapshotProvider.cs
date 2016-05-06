// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketSnapshotProvider.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Markets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Markets.Feeds;

    public class MarketSnapshotProvider
    {
        private readonly Dictionary<string, MarketInfo> lastMarketUpdates = new Dictionary<string, MarketInfo>();

        public MarketSnapshotProvider(IEnumerable<string> marketNames, ICanReceiveMarketData canReceiveMarketData)
        {
            canReceiveMarketData.InstrumentMarketDataUpdated += this.InstrumentMarketDataUpdated;
            foreach (var marketName in marketNames)
            {
                // TODO : Get rid of the hack (casting to concrete class)
                //this.lastMarketUpdates[marketName] = new MarketInfo(marketName, );
                canReceiveMarketData.Subscribe(marketName);
            }
        }

        private void InstrumentMarketDataUpdated(object sender, MarketDataUpdatedArgs marketDataUpdatedArgs)
        {
            this.lastMarketUpdates[marketDataUpdatedArgs.MarketName] = new MarketInfo(marketDataUpdatedArgs.MarketName, marketDataUpdatedArgs.Quantity, marketDataUpdatedArgs.Price);
        }

        public MarketSnapshot GetSnapshot()
        {
            return new MarketSnapshot(this.lastMarketUpdates.Values.ToList());
        }

        public void MarketFailed(string marketName)
        {
            this.lastMarketUpdates.First(m => m.Key == marketName).Value.OrdersFailureCount++;
        }
    }
}