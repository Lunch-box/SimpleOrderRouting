module Rop

    // ==============================================
    // This is a utility library for managing Success/Failure results
    //
    // See http://fsharpforfunandprofit.com/rop
    // See https://github.com/swlaschin/Railway-Oriented-Programming-Example
    // ==============================================

    /// A Result is a success or failure
    /// The Success case has a success value
    /// The Failure case has a list of messages

    type RopResult<'TSuccess> =
    | Success of 'TSuccess 
    | Failure of string list

    /// create a Success with no messages
    let succeed x =
        Success x

    /// create a Failure with a message
    let fail msg =
        Failure [msg]