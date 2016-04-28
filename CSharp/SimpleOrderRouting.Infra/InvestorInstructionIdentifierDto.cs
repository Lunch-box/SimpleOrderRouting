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
            return Equals(left, right);
        }

        public static bool operator !=(InvestorInstructionIdentifierDto left, InvestorInstructionIdentifierDto right)
        {
            return !Equals(left, right);
        }

        private static long nextValue;

        public long Value { get; private set; }

        public InvestorInstructionIdentifierDto()
        {
            this.Value =  Interlocked.Increment(ref nextValue);
        }
    }
}