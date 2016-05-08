namespace SimpleOrderRouting.Infra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Markets.Feeds;

    public sealed class MarketDataAdapter
    {
        private readonly IReadOnlyDictionary<string, ApiMarketGateway> gateways;

        public MarketDataAdapter(IReadOnlyDictionary<string, ApiMarketGateway> gateways)
        {
            this.gateways = gateways;
            foreach (var marketGateway in this.gateways.Values)
            {
                marketGateway.MarketDataUpdated += marketGateway_MarketDataUpdated;
            }
        }

        void marketGateway_MarketDataUpdated(object sender, ApiMarketDataUpdateEventArgs args)
        {
            // Adapts the external API feed format to the SOR domain format
            var marketDataUpdatedArgs = new MarketDataUpdatedArgs(args.OriginMarketName, args.MarketPrice, args.QuantityOnTheMarket);
            this.RaiseMarketDataUpdate(marketDataUpdatedArgs);
        }

        public event EventHandler<MarketDataUpdatedArgs> InstrumentMarketDataUpdated;

        public void Subscribe(string marketName)
        {
            var internalMarket = this.gateways.First(m => m.Key == marketName).Value;
            
            // Raise the first event
            var marketDataUpdatedArgs = new MarketDataUpdatedArgs(internalMarket.MarketName, internalMarket.SellPrice, internalMarket.SellQuantity);

            this.RaiseMarketDataUpdate(marketDataUpdatedArgs);
        }

        private void RaiseMarketDataUpdate(MarketDataUpdatedArgs args)
        {
            var onInstrumentMarketDataUpdated = this.InstrumentMarketDataUpdated;
            if (onInstrumentMarketDataUpdated != null)
            {
                onInstrumentMarketDataUpdated(this, args);
            }
        }

    }
}