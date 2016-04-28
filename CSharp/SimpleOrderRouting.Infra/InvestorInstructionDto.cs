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

        protected bool Equals(InvestorInstructionDto other)
        {
            return Equals(this.UniqueIdentifier, other.UniqueIdentifier) && this.Way == other.Way && this.Quantity == other.Quantity && this.Price == other.Price && this.AllowPartialExecution == other.AllowPartialExecution && this.GoodTill.Equals(other.GoodTill);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((InvestorInstructionDto)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.UniqueIdentifier != null ? this.UniqueIdentifier.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)this.Way;
                hashCode = (hashCode * 397) ^ this.Quantity;
                hashCode = (hashCode * 397) ^ this.Price.GetHashCode();
                hashCode = (hashCode * 397) ^ this.AllowPartialExecution.GetHashCode();
                hashCode = (hashCode * 397) ^ this.GoodTill.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(InvestorInstructionDto left, InvestorInstructionDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InvestorInstructionDto left, InvestorInstructionDto right)
        {
            return !Equals(left, right);
        }
    }
}