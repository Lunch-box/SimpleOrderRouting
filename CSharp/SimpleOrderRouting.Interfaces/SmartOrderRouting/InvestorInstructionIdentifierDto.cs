namespace SimpleOrderRouting.Interfaces.SmartOrderRouting
{
    using System.Threading;

    public class InvestorInstructionIdentifierDto
    {
        protected bool Equals(InvestorInstructionIdentifierDto other)
        {
            return this.value == other.value;
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
            return Equals((InvestorInstructionIdentifierDto)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public static bool operator ==(InvestorInstructionIdentifierDto left, InvestorInstructionIdentifierDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InvestorInstructionIdentifierDto left, InvestorInstructionIdentifierDto right)
        {
            return !Equals(left, right);
        }

        private static long nextValue;

        private long value;

        public InvestorInstructionIdentifierDto()
        {
            this.value =  Interlocked.Increment(ref nextValue);
        }
    }
}