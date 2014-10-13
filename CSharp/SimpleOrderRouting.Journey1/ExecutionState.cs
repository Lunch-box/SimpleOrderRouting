namespace SimpleOrderRouting.Journey1
{
    public class ExecutionState
    {
        public ExecutionState(InvestorInstruction investorInstruction)
        {
            this.Quantity = investorInstruction.Quantity;
            this.Price = investorInstruction.Price;
            this.Way = investorInstruction.Way;
            this.AllowPartialExecution = investorInstruction.AllowPartialExecution;
        }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Way Way { get; set; }

        public bool AllowPartialExecution { get; set; }
        
        public void Executed(int quantity)
        {
            this.Quantity -= quantity;
        }
    }
}