namespace SimpleOrderRouting.Journey1
{
    public struct OrderDescription
    {
        public Market TargetMarket;

        public Way OrderWay;

        public int Quantity;

        public decimal OrderPrice;

        public bool AllowPartial;

        public OrderDescription(Market targetMarket, Way orderWay, int quantity, decimal orderPrice, bool allowPartial)
        {
            this.TargetMarket = targetMarket;
            this.OrderWay = orderWay;
            this.Quantity = quantity;
            this.OrderPrice = orderPrice;
            this.AllowPartial = allowPartial;
        }
    }
}