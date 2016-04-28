namespace SimpleOrderRouting.Infra
{
    using System;

    using SimpleOrderRouting.Domain.SmartOrderRouting;

    public class InvestorInstructionDto
    {
        public InvestorInstructionDto(InvestorInstructionIdentifierDto uniqueIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null)
        {
            this.UniqueIdentifier = uniqueIdentifier;
            this.Way = way;
            this.Quantity = quantity;
            this.Price = price;
            this.AllowPartialExecution = allowPartialExecution;
            this.GoodTill = goodTill;
        }

        public InvestorInstructionIdentifierDto UniqueIdentifier { get; private set; }

        /// <summary>
        /// Gets the way to be used for the Instruction (Buy/Sell).
        /// </summary>
        /// <value>
        /// The way to be used for the Instruction (Buy/Sell).
        /// </value>
        public Way Way { get; private set; }

        /// <summary>
        /// Gets the quantity to be bought or sell.
        /// </summary>
        /// <value>
        /// The quantity to be bought or sell.
        /// </value>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the price we are looking for the execution.
        /// </summary>
        /// <value>
        /// The price we are looking for the execution.
        /// </value>
        public decimal Price { get; private set; }

        public bool AllowPartialExecution { get; private set; }

        public DateTime? GoodTill { get; private set; }

        public string InstrumentIdentifier { get; private set; }
    }
}