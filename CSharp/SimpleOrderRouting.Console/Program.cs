namespace SimpleOrderRouting.Console
{
    using System;

    using OtherTeam.StandardizedMarketGatewayAPI;
    using SimpleOrderRouting.Infra;

    class Program
    {
        static void Main(string[] args)
        {
            var marketA = new ApiMarketGateway("NYSE (New York)", sellQuantity: 150, sellPrice: 100M);
            var marketB = new ApiMarketGateway("CME (Chicago)", sellQuantity: 55, sellPrice: 101M);
            
            var investorAdapter = CompositionRootHelper.ComposeTheHexagon(marketA, marketB);

            var investorInstructionDto = new InvestorInstructionDto(Way.Buy, quantity: 125, price: 100M);

            System.Console.WriteLine("SOR connected to markets: {0} and {1}", marketA.MarketName, marketB.MarketName);
            System.Console.WriteLine();

            System.Console.WriteLine("Type 'Enter' to submit the following investor instruction: [{0}]\n\n", investorInstructionDto);

            System.Console.ReadLine();

            investorAdapter.Route(investorInstructionDto, arg => { System.Console.WriteLine("Instruction executed: [{0}]", arg); }, eventArgs => {});

            System.Console.WriteLine();
            System.Console.WriteLine("Type 'Enter' to exit");
            System.Console.ReadLine();
        }
    }
}
