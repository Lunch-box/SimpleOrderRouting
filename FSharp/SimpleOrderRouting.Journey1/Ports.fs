namespace SimpleOrderRouting.Journey1.Infrastructure

// This is the simulation of the external port that invokes the internal adatpers of the domain.
// This part is not generaly implemented by us but represents an external framework.
module Ports =

    // In memory port that allows to invoke API from the RPC in memory call.
    module InMemory =

        // Entry point of the port where a raw data is received (queue, REST, SOAP, etc.)
        let send rawData =
            ()