namespace OtherTeam.StandardizedMarketGatewayAPI
{
    using System;

    public class ApiOrder
    {
        public int Quantity { get; protected set; }

        public ApiMarketWay Way { get; protected set; }

        public bool AllowPartialExecution { get; protected set; }

        private void Send()
        {
            throw new NotImplementedException();
        }
    }
}