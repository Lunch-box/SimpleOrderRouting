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
namespace SimpleOrderRouting.Journey1
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using SimpleOrderRouting.Interfaces.SmartOrderRouting;

    public class SorTestHarness
    {
        private InvestorInstructionDto investorInstructionDto;

        private object synchro = new object();

        private bool done = false;

        private InvestorInstructionIdentifierDto instructionIdentifier;

        public void Run()
        {
            // initialize the context
            var sor = BuildSor();


            // instantiate the service
            var service = new SmartOrderRoutingService(sor);

            // initialize our engine
            service.InstructionUpdated += ServiceOnInstructionUpdated;
            this.instructionIdentifier = service.RequestUniqueIdentifier();

            // build demo order
            this.investorInstructionDto = new InvestorInstructionDto(Way.Buy, 10, 100M, true, null);

            var stopWatch = new Stopwatch();

            // sends the instruction
            stopWatch.Start();
            service.Send(this.instructionIdentifier, investorInstructionDto);

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
            if (investorInstructionUpdatedDto.IdentifierDto == instructionIdentifier)
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

        private static SmartOrderRoutingEngine BuildSor()
        {
            var marketA = new Market
                              {
                                  SellQuantity = 150,
                                  SellPrice = 100M
                              };

            var marketB = new Market
                              {
                                  SellQuantity = 55,
                                  SellPrice = 101M
                              };

            var sor = new SmartOrderRoutingEngine(new[] { marketA, marketB });
            return sor;
        }

        public double AverageLatency { get; set; }
    }
}