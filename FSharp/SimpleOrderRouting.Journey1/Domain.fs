namespace SimpleOrderRouting.Journey1

module Domain =
    
    open System

    type Way =
        | Buy
        | Sell

    type Market = {SellQuantity : int; SellPrice : decimal; TimeSent : int}
    type InvestorInstruction = {Way : Way; Quantity : int; Price : decimal; AllowPartialExecution : bool; GoodTill : DateTime option}