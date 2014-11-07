namespace SimpleOrderRouting.Journey1

module Markets =

    // TODO : This should be declared in an external assembly (we don't owe this code)
    type ExternalMarket = {SellQuantity : int; SellPrice : decimal; TimeSent : int}

module MarketAdapters =

    open System
    
    type WayDto =
        | Buy
        | Sell
        | Uninitialized
    
    /// Represents a DTO that is exposed to the outside world.
    /// This is a regular POCO class which can be null.
    ///
    /// you have to try hard to make an nullable object in F#
    [<AllowNullLiteralAttribute>]
    type InvestorInstructionDto() =
        member val Way : WayDto = Uninitialized with get, set
        member val Quantity : int = 0 with get, set
        member val Price : decimal = 0m with get, set
        member val AllowPartialExecution : bool = false with get, set
        member val GoodTill : Nullable<DateTime> = Nullable() with get, set

    
    /// This is the adapter entry point for converting the external InvestorInstructionDto to the internal domain
    let dtoToInvestorInstruction dto =
        ()