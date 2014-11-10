using System;
using System.Collections.Generic;

namespace SimpleOrderRouting.Tests
{
    using NFluent;
    using SimpleOrderRouting.Interfaces.SmartOrderRouting;
    using SimpleOrderRouting.Journey1;
    using SimpleOrderRouting.Journey1.Infrastructure;
    using SimpleOrderRouting.Journey1.TestHelpers;

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

            var markets = new[] { marketA, marketB };
            var routingEngine = CreateSmartOrderRoutingEngine(markets);
            var sorRawInprocPort = new SmartOrderRoutingRawInprocAdapter(routingEngine);
            
            var uniqueIdentifier = sorRawInprocPort.RequestUniqueIdentifier();
            var updates = new List<InvestorInstructionUpdatedDto>();

            sorRawInprocPort.InstructionUpdated += (s, e) => updates.Add(e);
            sorRawInprocPort.Send(new InvestorInstructionDto(uniqueIdentifier, Way.Buy, quantity:125, price: 100M, allowPartialExecution:true, goodTill:DateTime.MaxValue));

            // NFluent: CountIs
            Check.That(updates).HasSize(1);
        }

        private static SmartOrderRoutingEngine CreateSmartOrderRoutingEngine(Market[] markets)
        {
            
            var routingEngine = new SmartOrderRoutingEngine(new MarketProvider(markets), null, new MarketDataProvider(markets));
            return routingEngine;
        }
    }
}
