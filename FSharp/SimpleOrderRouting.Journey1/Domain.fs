namespace SimpleOrderRouting.Journey1

module Domain =
    
    open System
    open Rop

    type Way =
        | Buy
        | Sell

    type Market = {SellQuantity : int; SellPrice : decimal; TimeSent : int}

    // ------------------------------
    // InvestorInstruction

    type InvestorInstructionId = InvestorInstructionId of int
    type InvestorInstruction = {Way : Way; Quantity : int; Price : decimal; AllowPartialExecution : bool; GoodTill : DateTime option}

    // Create a InvestorInstructionId from an int
    let createInvestorInstructionId id =
        if id < 1 then
            fail "InvestorInstructionId must be positive integer"
        else 
            succeed (InvestorInstructionId id)