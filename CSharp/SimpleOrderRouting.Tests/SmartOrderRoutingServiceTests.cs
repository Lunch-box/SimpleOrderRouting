using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleOrderRouting.Tests
{
    using NFluent;

    using SimpleOrderRouting.Interfaces;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting.Investor;
    using SimpleOrderRouting.Journey1;
    using SimpleOrderRouting.Journey1.Infrastructure;

    using Xunit;

    public class SmartOrderRoutingServiceTests
    {
        [Fact]
        public void ShouldExecuteInstructionWhenThereIsEnoughLiquidityOnOneMarket()
        {
            var marketA = new Market()
            {
                SellQuantity = 150,
                SellPrice = 100M
            };

            var marketB = new Market()
            {
                SellQuantity = 55,
                SellPrice = 101M
            };

            ISmartOrderRoutingRawInprocPort sorRawInprocPort = new SmartOrderRoutingRawInprocPort(new SmartOrderRoutingEntryPointEngine(new[] { marketA, marketB }));
            
            var uniqueIdentifier = sorRawInprocPort.RequestUniqueIdentifier();
            var updates = new List<InvestorInstructionUpdatedDto>();

            sorRawInprocPort.InstructionUpdated += (s, e) => updates.Add(e);
            sorRawInprocPort.Send(uniqueIdentifier, new InvestorInstructionDto(Way.Buy, quantity:125, price: 100M, allowPartialExecution:true, goodTill:DateTime.MaxValue));

            // NFluent: CountIs
            Check.That(updates).HasSize(1);
        }
    }
}
