// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvestorInstruction.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Investors
{
    using System;

    /// <summary>
    /// Trading instruction given to the SOR on the investor-side.
    /// </summary>
    public class InvestorInstruction
    {
        public InvestorInstruction(long investorInstructionIdentifier, Way way, int quantity, decimal price, bool allowPartialExecution = false, DateTime? goodTill = null)
        {
            this.Way = way;
            this.Quantity = quantity;
            this.Price = price;
            this.AllowPartialExecution = allowPartialExecution;
            this.GoodTill = goodTill;
        
            this.InvestorInstructionIdentifier = investorInstructionIdentifier;
        }

        public long InvestorInstructionIdentifier { get; private set; }

        public DateTime? GoodTill { get; private set; }

        public bool AllowPartialExecution { get; private set; }

        public decimal Price { get; private set; }

        public int Quantity { get; private set; }

        public Way Way { get; private set; }
        
        public static bool operator ==(InvestorInstruction left, InvestorInstruction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InvestorInstruction left, InvestorInstruction right)
        {
            return !Equals(left, right);
        }

        protected bool Equals(InvestorInstruction other)
        {
            return this.InvestorInstructionIdentifier == other.InvestorInstructionIdentifier && this.GoodTill.Equals(other.GoodTill) && this.AllowPartialExecution == other.AllowPartialExecution && this.Price == other.Price && this.Quantity == other.Quantity && this.Way == other.Way;
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
            return this.Equals((InvestorInstruction)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.InvestorInstructionIdentifier.GetHashCode();
                hashCode = (hashCode * 397) ^ this.GoodTill.GetHashCode();
                hashCode = (hashCode * 397) ^ this.AllowPartialExecution.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Price.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Quantity;
                hashCode = (hashCode * 397) ^ (int)this.Way;
                return hashCode;
            }
        }
    }
}