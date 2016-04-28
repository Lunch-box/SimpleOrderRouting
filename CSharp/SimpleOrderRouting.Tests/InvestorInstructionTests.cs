using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOrderRouting.Tests
{
    using NFluent;

    using SimpleOrderRouting.Domain;
    using SimpleOrderRouting.Domain.SmartOrderRouting;

    using Xunit;

    public class InvestorInstructionTests
    {
        [Fact]
        public void Should_Compare_values_and_not_references()
        {
            var investorInstructionIdentifier = 42;
            const Way way = Way.Buy;
            var quantity = 1;
            var price = 23.3M;
            var allowPartialExecution = true;
            var goodTill = DateTime.Now;

            var firstInstruction = new InvestorInstruction(investorInstructionIdentifier, way, quantity, price, allowPartialExecution, goodTill);
            var secondIdenticalInstruction = new InvestorInstruction(investorInstructionIdentifier, way, quantity, price, allowPartialExecution, goodTill);
            
            Check.That(firstInstruction).IsEqualTo(secondIdenticalInstruction);

            allowPartialExecution = false;
            var slightlyDifferentInstruction = new InvestorInstruction(investorInstructionIdentifier, way, quantity, price, allowPartialExecution, goodTill);
            Check.That(slightlyDifferentInstruction).IsNotEqualTo(firstInstruction);
        }
    }
}
