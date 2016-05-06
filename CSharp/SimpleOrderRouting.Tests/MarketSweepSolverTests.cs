namespace SimpleOrderRouting.Tests
{
    using System;

    using NFluent;

    using OtherTeam.StandardizedMarketGatewayAPI;

    using SimpleOrderRouting.Infra;
    using SimpleOrderRouting.Investors;
    using SimpleOrderRouting.Markets;
    using SimpleOrderRouting.SolvingStrategies;

    using Xunit;

    public class MarketSweepSolverTests
    {
        [Fact]
        public void Should_Solve_with_2_markets_when_asked_quantity_is_odd()
        {
            var marketA = new ApiMarketGateway("Euronext", sellQuantity: 50, sellPrice: 100M);
            var rejectingMarket = new ApiMarketGateway("CME", sellQuantity: 50, sellPrice: 100M, orderPredicate: _ => false);
            var marketsInvolved = new[] { marketA, rejectingMarket };
            var marketGatewayAdapter = new MarketGatewaysAdapter(marketsInvolved);

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 50, price: 100M, goodTill: DateTime.Now.AddMinutes(5));
            var instructionExecutionContext = new InstructionExecutionContext(investorInstruction);
            
            var marketSweepSolver = new MarketSweepSolver(new MarketSnapshotProvider(marketGatewayAdapter.GetAvailableMarketNames(), marketGatewayAdapter));
            var orderBasket = marketSweepSolver.Solve(instructionExecutionContext, marketGatewayAdapter);

            Check.That(orderBasket.OrdersDescriptions.Extracting("Quantity")).ContainsExactly(25, 25);
        }

        [Fact]
        public void Should_Solve_with_2_markets_when_asked_quantity_is_1()
        {
            var marketA = new ApiMarketGateway("Euronext", sellQuantity: 50, sellPrice: 100M);
            var rejectingMarket = new ApiMarketGateway("CME", sellQuantity: 50, sellPrice: 100M, orderPredicate: _ => false);
            var marketsInvolved = new[] { marketA, rejectingMarket };
            var marketGatewayAdapter = new MarketGatewaysAdapter(marketsInvolved);

            var investorInstruction = new InvestorInstruction(new InvestorInstructionIdentifierDto().Value, Way.Buy, quantity: 1, price: 100M, goodTill: DateTime.Now.AddMinutes(5));
            var instructionExecutionContext = new InstructionExecutionContext(investorInstruction);

            var marketSweepSolver = new MarketSweepSolver(new MarketSnapshotProvider(marketGatewayAdapter.GetAvailableMarketNames(), marketGatewayAdapter));
            var orderBasket = marketSweepSolver.Solve(instructionExecutionContext, marketGatewayAdapter);

            Check.That(orderBasket.OrdersDescriptions.Extracting("Quantity")).ContainsExactly(1);
        }
    }
}
