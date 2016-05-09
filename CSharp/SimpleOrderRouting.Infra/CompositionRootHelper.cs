// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositionRootHelper.cs" company="LunchBox corp">
//     Copyright 2014 The Lunch-Box mob: 
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
    using OtherTeam.StandardizedMarketGatewayAPI;

    public static class CompositionRootHelper
    {
        /// <summary>
        /// Acts like a composition root for the SOR Hexagonal Architecture.
        /// </summary>
        /// <param name="marketGateways">The list of ApiMarketGateway the SOR must interact with.</param>
        /// <returns>The adapter we must use as Investors in order to give investment instructions.</returns>
        public static InvestorInstructionsAdapter ComposeTheHexagon(params ApiMarketGateway[] marketGateways)
        {
            // Step1: instantiates the adapter(s) the (SOR) domain will need to work with through the Dependency Inversion principle.
            var marketGatewaysAdapter = new MarketGatewaysAdapter(marketGateways);

            // Step2: instantiates the SOR domain entry point.
            var sor = new SmartOrderRoutingEngine(marketGatewaysAdapter, marketGatewaysAdapter, marketGatewaysAdapter);

            // Step3: instantiates the adapters we will use to interact with our domain.
            var investorInstructionAdapter = new InvestorInstructionsAdapter(sor);

            return investorInstructionAdapter;
        }
    }
}