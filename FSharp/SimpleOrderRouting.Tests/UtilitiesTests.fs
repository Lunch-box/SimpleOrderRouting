namespace SimpleOrderRouting.Tests

open Xunit
open FsUnit.Xunit
open SimpleOrderRouting.Journey1.Rop

module ``Rop tests`` =

    let failBranch() =
        failwith "This branch should not have been called."

    [<Fact>]
    let ``Create a Success with no messages``() =
        let result = succeed 10
        match result with
            | Success v -> v |> should equal 10
            | Failure _ -> failBranch()

    [<Fact>]
    let ``create a Failure with a message``() =
        let result = fail "This is a failure"
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors.[0] |> should equal "This is a failure"