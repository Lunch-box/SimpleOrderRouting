namespace SimpleOrderRouting.Journey1

module Domain =

    open Rop
    
    type InvestorInstructionId

    val createInvestorInstructionId : id:int -> RopResult<InvestorInstructionId>