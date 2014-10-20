namespace SimpleOrderRouting.Journey1
{
    using System.Threading;

    public class InvestorInstructionIdentifier
    {
        private static long nextValue;

        private long value;

        public InvestorInstructionIdentifier()
        {
            this.value =  Interlocked.Increment(ref nextValue);
        }
    }
}