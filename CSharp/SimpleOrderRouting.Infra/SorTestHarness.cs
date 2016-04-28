// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SorTestHarness.cs" company="LunchBox corp">
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
namespace SimpleOrderRouting.Infra
{
    using System.Diagnostics;
    using System.Threading;

    using SimpleOrderRouting.Domain;
    using SimpleOrderRouting.Domain.SmartOrderRouting;
    using SimpleOrderRouting.Infra.TestHelpers;

    public class SorTestHarness
    {
        private InvestorInstructionDto investorInstructionDto;

        private object synchro = new object();

        private bool done = false;

        private InvestorInstructionIdentifierDto instructionIdentifier;

        public void Run()
        {
            // Build our hexagon (3 steps)

            // 1. Builds the runtime dependencies needed for our domain to work with (through DIP)
            var markets = BuildMarketVenues();
            
            // 2. Instantiates our domain entry point with it
            var sor = new SmartOrderRoutingEngine(new MarketProvider(markets), null, new MarketDataProvider(markets));

            // 3. Instantiates the adapter(s) we need to interact with our domain
            var instructionsAdapter = new InvestorInstructionsAdapter(sor);

            instructionsAdapter.InstructionUpdated += ServiceOnInstructionUpdated;
            var identifier = InvestorInstructionIdentifierFactory.RequestUniqueIdentifier();
            this.instructionIdentifier = identifier; //adapter.RequestUniqueIdentifier();

            // build demo order
            this.investorInstructionDto = new InvestorInstructionDto(identifier, Way.Buy, 10, 100M, true, null);

            var stopWatch = new Stopwatch();

            // sends the instruction
            stopWatch.Start();
            // Subscribes to the instruction's events
            OrderExecutedEventArgs orderExecutedEventArgs = null;
            string failureReason = null;
            instructionsAdapter.Subscribe(investorInstructionDto.UniqueIdentifier, (args) => { orderExecutedEventArgs = args; }, (args) => { failureReason = args; });
            instructionsAdapter.Route(investorInstructionDto);

            // wait for the exit condition
            lock (this.synchro)
            {
                if (this.done == false)
                {
                    Monitor.Wait(this.synchro, 500);
                }
            }

            stopWatch.Stop();
            
            if (this.done)
            {
                this.AverageLatency = stopWatch.ElapsedMilliseconds;
            }

        }

        private void ServiceOnInstructionUpdated(object sender, InvestorInstructionUpdatedDto investorInstructionUpdatedDto)
        {
            if (investorInstructionUpdatedDto.IdentifierDto == this.instructionIdentifier)
            {
                if (investorInstructionUpdatedDto.Status == InvestorInstructionStatus.PartiallyExecuted)
                {
                    return;
                }
                lock (this.synchro)
                {
                    this.done = true;
                    Monitor.PulseAll(this.synchro);
                }
            }
        }

        private static Market[] BuildMarketVenues()
        {
            var marketA = new Market
                              {
                                  SellQuantity = 150,
                                  SellPrice = 100M,
                              };

            var marketB = new Market
                              {
                                  SellQuantity = 55,
                                  SellPrice = 101M,
                              };

            var markets = new[] { marketA, marketB };
            return markets;
        }

        public double AverageLatency { get; set; }
    }
}