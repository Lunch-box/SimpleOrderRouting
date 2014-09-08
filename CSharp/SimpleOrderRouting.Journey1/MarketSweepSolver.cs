namespace SimpleOrderRouting.Journey1
{
    using System;
    using System.Collections.Generic;

    public class MarketSweepSolver : IInvestorInstructionSolver
    {
        private readonly Market[] markets;

        public MarketSweepSolver(Market[] markets)
        {
            this.markets = markets;
        }

        /// <summary>
        /// Build the description of the orders needed to fulfill the <see cref="investorInstruction"/>
        /// </summary>
        /// <param name="investorInstruction"></param>
        /// <returns>Order description</returns>
        public IEnumerable<OrderDescription> Solve(InvestorInstruction investorInstruction)
        {
            var description = new List<OrderDescription>();
            // Checks liquidities available to weighted average for execution
            int remainingQuantityToBeExecuted = investorInstruction.Quantity;

            var requestedPrice = investorInstruction.Price;
            
            var availableQuantityOnMarkets = this.ComputeAvailableQuantityForThisPrice(requestedPrice);

            var ratio = (remainingQuantityToBeExecuted / (decimal)availableQuantityOnMarkets);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var market in this.markets)
            {
                var convertedMarketQuantity = Math.Round((market.SellQuantity * ratio), 2, MidpointRounding.AwayFromZero);
                var quantityToExecute = Convert.ToInt32(convertedMarketQuantity);

                if (quantityToExecute > 0)
                {
                    description.Add(new OrderDescription(market, investorInstruction.Way, quantityToExecute, requestedPrice, true));
                }
            }

            return description;
        }

        private int ComputeAvailableQuantityForThisPrice(decimal requestedPrice)
        {
            var availableQuantityOnMarkets = 0;

            foreach (var market in this.markets)
            {
                if (requestedPrice >= market.SellPrice)
                {
                    availableQuantityOnMarkets += market.SellQuantity;
                }
            }
            return availableQuantityOnMarkets;
        }
    }
}