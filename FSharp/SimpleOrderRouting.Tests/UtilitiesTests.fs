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
    let ``Create a Failure with a message``() =
        let result = fail "This is a failure"
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors.[0] |> should equal "This is a failure"

    [<Fact>]
    let ``Apply the function to the value only if both are Success``() =
        let success = succeed 10
        let intToString i = i.ToString()
        let successFunc = succeed intToString
        let result = applyR successFunc success
        match result with
            | Success r -> r |> should equal "10"
            | Failure _ -> failBranch()