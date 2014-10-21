namespace SimpleOrderRouting.Interfaces.SmartOrderRouting
{
    using System.Threading;

    public class InvestorInstructionIdentifierDto
    {
        private static long nextValue;

        private long value;

        public InvestorInstructionIdentifierDto()
        {
            this.value =  Interlocked.Increment(ref nextValue);
        }
    }
}