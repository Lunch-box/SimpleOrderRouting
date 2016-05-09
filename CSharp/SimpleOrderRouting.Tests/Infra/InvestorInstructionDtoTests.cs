// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvestorInstructionDtoTests.cs" company="LunchBox corp">
//     Copyright 2014 The Lunch-Box mob: 
//           Ozgur DEVELIOGLU (@Zgurrr)
//           Cyrille  DUPUYDAUBY (@Cyrdup)
//           Tomasz JASKULA (@tjaskula)
//           Mendel MONTEIRO-BECKERMAN (@MendelMonteiro)
//           Thomas PIERRAIN (@tpierrain)
//     
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//         http://www.apache.org/licenses/LICENSE-2.0
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleOrderRouting.Tests.Infra
{
    using System;
    using System.Collections.Generic;

    using NFluent;

    using NUnit.Framework;

    using SimpleOrderRouting.Infra;
    using SimpleOrderRouting.Investors;

    public class InvestorInstructionDtoTests
    {
        [Test]
        public void Should_rely_on_values_for_equality()
        {
            var investorInstructionIdentifier = 42;
            const Way Way = Way.Buy;
            var quantity = 1;
            var price = 23.3M;
            var allowPartialExecution = true;
            var goodTill = DateTime.Now;

            var dtoIdentifier = new InvestorInstructionIdentifierDto();
            var firstInstruction = new InvestorInstructionDto(dtoIdentifier, Way, quantity, price, allowPartialExecution, goodTill);
            var secondIdenticalInstruction = new InvestorInstructionDto(dtoIdentifier, Way, quantity, price, allowPartialExecution, goodTill);

            Check.That(firstInstruction).IsEqualTo(secondIdenticalInstruction);

            allowPartialExecution = false;
            var slightlyDifferentInstruction = new InvestorInstruction(investorInstructionIdentifier, Way, quantity, price, allowPartialExecution, goodTill);
            Check.That(slightlyDifferentInstruction).IsNotEqualTo(firstInstruction);
        }

        [Test]
        public void Should_rely_on_values_for_unicity()
        {
            var goodTill = DateTime.Now;
            var dtoIdentifier = new InvestorInstructionIdentifierDto();

            var instruction = new InvestorInstructionDto(dtoIdentifier, Way.Buy, 1, 1.1M, true, goodTill);
            var identicalInstruction = new InvestorInstructionDto(dtoIdentifier, Way.Buy, 1, 1.1M, true, goodTill);
            var dictionary = new Dictionary<InvestorInstructionDto, int>();
            dictionary[instruction] = 2;
            dictionary[identicalInstruction] = 3;

            Check.That(dictionary[instruction]).IsEqualTo(3);
        }
    }
}