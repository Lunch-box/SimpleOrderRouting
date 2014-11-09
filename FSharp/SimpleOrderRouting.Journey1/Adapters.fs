namespace SimpleOrderRouting.Journey1

module MarketAdapters =

    open System
    open Domain
    open Dtos

    let send (investorInstructionDto : InvestorInstructionDto) =
        let investorInstruction = dtoToInvestorInstruction investorInstructionDto
        ()