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

    [<Fact>]
    let ``Apply returns Failure if value is not a Success``() =
        let failure = fail "First error"
        let intToString i = i.ToString()
        let successFunc = succeed intToString
        let result = applyR successFunc failure
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors.[0] |> should equal "First error"

    [<Fact>]
    let ``Apply returns Failure if function is not a Success``() =
        let success = succeed 10
        let intToString i = i.ToString()
        let failFunc = fail "Function in error"
        let result = applyR failFunc success
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors.[0] |> should equal "Function in error"

    [<Fact>]
    let ``Apply returns Failure if function and value are not a Success``() =
        let expectedErrors = ["First error"; "Function in error"]
        let failure = fail expectedErrors.[0]
        let intToString i = i.ToString()
        let failFunc = fail expectedErrors.[1]
        let result = applyR failFunc failure
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors |> List.rev
                                       |> List.iteri (fun i e -> e |> should equal expectedErrors.[i])

    [<Fact>]
    let ``Litf a function and applies it if the result is on the Success branch``() =
        let intToString i = i.ToString()
        let value = succeed 10
        let result = liftR intToString value
        match result with
            | Success r -> r |> should equal "10"
            | Failure _ -> failBranch()

    [<Fact>]
    let ``Litf a function and not applies it if the result is on the Failure branch``() =
        let intToString i = i.ToString()
        let value = fail "Failed value"
        let result = liftR intToString value
        match result with
            | Success r -> failBranch()
            | Failure errors -> errors.[0] |> should equal "Failed value"