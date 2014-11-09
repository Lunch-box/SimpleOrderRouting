namespace SimpleOrderRouting.Journey1

module Dtos =

    open System
    open Domain

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
        member val Id : int = 0 with get, set
        member val Way : WayDto = Uninitialized with get, set
        member val Quantity : int = 0 with get, set
        member val Price : decimal = 0m with get, set
        member val AllowPartialExecution : bool = false with get, set
        member val GoodTill : Nullable<DateTime> = Nullable() with get, set

    /// Convert a DTO into a domain contact.
    ///
    /// We MUST handle the possibility of one or more errors
    /// because the InvestorInstruction type has stricter constraints than InvestorInstructionDto
    /// and the conversion might fail.
    /// This is the adapter entry point for converting the external InvestorInstructionDto to the internal domain
    let dtoToInvestorInstruction (dto : InvestorInstructionDto) =
        if dto = null then
            Rop.fail "Investor instruction is required"
        else
            let idOrError = createInvestorInstructionId dto.Id
            Rop.fail "sdsd"
        //{Way = Buy; Quantity = 0; Price = 0m; AllowPartialExecution = false}

module SmartOrderRoutingEngine =
    
    let route inverstorInstruction =
        ()