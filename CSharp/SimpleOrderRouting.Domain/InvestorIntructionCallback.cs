// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvestorIntructionCallback.cs" company="LunchBox corp">
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

namespace SimpleOrderRouting
{
    using System;

    using SimpleOrderRouting.Markets.Orders;

    /// <summary>
    /// Allows to raise Callbacks on a given investor instruction.
    /// </summary>
    public class InvestorIntructionCallback
    {
        private readonly Action<OrderExecutedEventArgs> executedCallback;

        private readonly Action<string> failureCallback;

        public InvestorIntructionCallback(Action<OrderExecutedEventArgs> executedCallback, Action<string> failureCallback)
        {
            this.executedCallback = executedCallback;
            this.failureCallback = failureCallback;
        }

        public void OnExecuted(object sender, OrderExecutedEventArgs e)
        {
            this.executedCallback(e);
        }
    }
}