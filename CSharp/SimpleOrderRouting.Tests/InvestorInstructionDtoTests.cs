namespace SimpleOrderRouting.Tests
{
    using System;
    using System.Collections.Generic;

    using NFluent;

    using SimpleOrderRouting.Domain;
    using SimpleOrderRouting.Domain.SmartOrderRouting;
    using SimpleOrderRouting.Infra;

    using Xunit;

    public class InvestorInstructionDtoTests
    {
        [Fact]
        public void Should_rely_on_values_for_equality()
        {
            var investorInstructionIdentifier = 42;
            const Way way = Way.Buy;
            var quantity = 1;
            var price = 23.3M;
            var allowPartialExecution = true;
            var goodTill = DateTime.Now;

            var dtoIdentifier = InvestorInstructionIdentifierFactory.RequestUniqueIdentifier();
            var firstInstruction = new InvestorInstructionDto(dtoIdentifier, way, quantity, price, allowPartialExecution, goodTill);
            var secondIdenticalInstruction = new InvestorInstructionDto(dtoIdentifier, way, quantity, price, allowPartialExecution, goodTill);

            Check.That(firstInstruction).IsEqualTo(secondIdenticalInstruction);

            allowPartialExecution = false;
            var slightlyDifferentInstruction = new InvestorInstruction(investorInstructionIdentifier, way, quantity, price, allowPartialExecution, goodTill);
            Check.That(slightlyDifferentInstruction).IsNotEqualTo(firstInstruction);
        }

        [Fact]
        public void Should_rely_on_values_for_unicity()
        {
            var goodTill = DateTime.Now;
            var dtoIdentifier = InvestorInstructionIdentifierFactory.RequestUniqueIdentifier();

            var instruction = new InvestorInstructionDto(dtoIdentifier, Way.Buy, 1, 1.1M, true, goodTill);
            var identicalInstruction = new InvestorInstructionDto(dtoIdentifier, Way.Buy, 1, 1.1M, true, goodTill);
            var dictionary = new Dictionary<InvestorInstructionDto, int>();
            dictionary[instruction] = 2;
            dictionary[identicalInstruction] = 3;

            Check.That(dictionary[instruction]).IsEqualTo(3);
        }
    }
}