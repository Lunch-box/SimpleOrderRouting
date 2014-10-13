namespace SimpleOrderRouting.Journey1
{
    public class OrderFailedEventArgs
    {
        public OrderFailedEventArgs(Market market, string reason)
        {
            this.Reason = reason;
            this.Market = market;
        }

        public Market Market { get; private set; }

        public string Reason { get; private set; }
    }
}