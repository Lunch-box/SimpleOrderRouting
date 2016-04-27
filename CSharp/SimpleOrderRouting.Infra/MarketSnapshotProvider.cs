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
namespace SimpleOrderRouting.Infra
{
    using System.Collections.Generic;
    using System.Linq;

    using SimpleOrderRouting.Domain;

    public class MarketSnapshotProvider
    {
        private readonly Dictionary<IMarket, MarketInfo> _lastMarketUpdates = new Dictionary<IMarket, MarketInfo>();

        public MarketSnapshotProvider(IEnumerable<IMarket> marketsToWatch, ICanReceiveMarketData canReceiveMarketData)
        {
            canReceiveMarketData.InstrumentMarketDataUpdated += this.InstrumentMarketDataUpdated;
            foreach (var market in marketsToWatch)
            {
                // TODO : Get rid of the hack (casting to concrete class)
                this._lastMarketUpdates[market] = new MarketInfo((Market)market);
                canReceiveMarketData.Subscribe(market);
            }
        }

        private void InstrumentMarketDataUpdated(object sender, MarketDataUpdateDto marketDataUpdateDto)
        {
            var marketInfo = this._lastMarketUpdates[marketDataUpdateDto.Market];
            marketInfo.Market.SellPrice = marketDataUpdateDto.Price;
            marketInfo.Market.SellQuantity = marketDataUpdateDto.Quantity;
        }

        public MarketSnapshot GetSnapshot()
        {
            return new MarketSnapshot(this._lastMarketUpdates.Values.ToList());
        }

        public void MarketFailed(Market market)
        {
            // TODO: refactor this so the method accepts IMarket instead
            this._lastMarketUpdates.First(m => m.Value.Market == market).Value.OrdersFailureCount++;
        }
    }
}