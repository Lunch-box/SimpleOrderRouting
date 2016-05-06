namespace OtherTeam.StandardizedMarketGatewayAPI
{
    using System;

    public class ApiMarketDataUpdateEventArgs : EventArgs
    {
        public ApiMarketDataUpdateEventArgs(string originMarketName, decimal marketPrice, int quantityOnTheMarket)
        {
            this.QuantityOnTheMarket = quantityOnTheMarket;
            this.MarketPrice = marketPrice;
            this.OriginMarketName = originMarketName;
        }

        // TODO : introduce the concept of various instruments
        
        public string OriginMarketName { get; private set; }
        public decimal MarketPrice { get; private set; }
        public int QuantityOnTheMarket { get; private set; }
    }
}