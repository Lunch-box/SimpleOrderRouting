// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvestorInstructionDto.cs" company="LunchBox corp">
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
    using System;

    /// <summary>
    /// Data Transfer Object for Investor Instruction.
    /// </summary>
    public class InvestorInstructionDto
    {
        public InvestorInstructionDto(Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null) : this(new InvestorInstructionIdentifierDto(), way, quantity, price, allowPartialExecution, goodTill)
        {
        }
        
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

        #region Unicity and Identity

        private bool Equals(InvestorInstructionDto other)
        {
            return object.Equals(this.UniqueIdentifier, other.UniqueIdentifier) && this.Way == other.Way && this.Quantity == other.Quantity && this.Price == other.Price && this.AllowPartialExecution == other.AllowPartialExecution && this.GoodTill.Equals(other.GoodTill);
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

            return this.Equals((InvestorInstructionDto)obj);
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
            return object.Equals(left, right);
        }

        public static bool operator !=(InvestorInstructionDto left, InvestorInstructionDto right)
        {
            return !object.Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Investor Instruction DTO: Way: {0} - Quantity: {1} - Price: {2} - AllowPartialExecution: {3} - GoodTill: {4}.", this.Way, this.Quantity, this.Price, this.AllowPartialExecution, this.GoodTill);
        }
    }
}