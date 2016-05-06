namespace OtherTeam.StandardizedMarketGatewayAPI
{
    using System;

    /// <summary>
    /// Event data for DealExecuted event.
    /// </summary>
    public class ApiOrderFailedEventArgs : EventArgs
    {
        public string MarketName { get; private set; }

        public string FailureCause { get; private set; }

        public ApiOrderFailedEventArgs(string marketName, string failureCause)
        {
            this.MarketName = marketName;
            this.FailureCause = failureCause;
        }
    }
}