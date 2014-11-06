namespace SimpleOrderRouting.Journey1

module Domain =

    type Market = {SellQuantity : int; SellPrice : decimal; TimeSent : int}
    type InvestorInstruction = {Id : single}