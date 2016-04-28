// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvestorInstructionIdentifierDto.cs" company="LunchBox corp">
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
    using System.Threading;

    public class InvestorInstructionIdentifierDto
    {
        protected bool Equals(InvestorInstructionIdentifierDto other)
        {
            return this.Value == other.Value;
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
            return this.Equals((InvestorInstructionIdentifierDto)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(InvestorInstructionIdentifierDto left, InvestorInstructionIdentifierDto right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(InvestorInstructionIdentifierDto left, InvestorInstructionIdentifierDto right)
        {
            return !object.Equals(left, right);
        }

        private static long nextValue;

        public long Value { get; private set; }

        public InvestorInstructionIdentifierDto()
        {
            this.Value = Interlocked.Increment(ref nextValue);
        }
    }
}