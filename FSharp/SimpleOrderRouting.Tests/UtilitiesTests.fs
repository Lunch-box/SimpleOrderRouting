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

    [<Fact>]
    let ``Litf two functions and applies it if the results are on the Success branch``() =
        let intToString i i' = (i * i').ToString()
        let value = succeed 10
        let value' = succeed 5
        let result = lift2R intToString value value'
        match result with
            | Success r -> r |> should equal "50"
            | Failure _ -> failBranch()

    [<Fact>]
    let ``Litf two functions and not applies it if one of the results are on the Failure branch``() =
        let intToString i i' = (i * i').ToString()
        let value = fail "First value failed"
        let value' = succeed 10
        let result = lift2R intToString value value'
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors.[0] |> should equal "First value failed"

    [<Fact>]
    let ``Litf two functions and not applies it if one of the results are on the Failure branch'``() =
        let intToString i i' = (i * i').ToString()
        let value = succeed 10
        let value' = fail "Second value failed"
        let result = lift2R intToString value value'
        match result with
            | Success _ -> failBranch()
            | Failure errors -> errors.[0] |> should equal "Second value failed"

    [<Fact>]
    let ``Litf three functions and applies it if the results are on the Success branch``() =
        let intToString i i' i'' = (i * i' + i'').ToString()
        let value = succeed 10
        let value' = succeed 5
        let value'' = succeed 2
        let result = lift3R intToString value value' value''
        match result with
            | Success r -> r |> should equal "52"
            | Failure _ -> failBranch()

    [<Fact>]
    let ``Litf three functions and not applies it if one the results are on the Failure branch``() =
        let intToString i i' i'' = (i * i' + i'').ToString()
        let value = fail "First value failure"
        let value' = succeed 5
        let value'' = succeed 2
        let result = lift3R intToString value value' value''
        match result with
            | Success _ -> failBranch()
            | Failure errors-> errors.[0] |> should equal "First value failure"

    [<Fact>]
    let ``Litf three functions and not applies it if one the results are on the Failure branch'``() =
        let intToString i i' i'' = (i * i' + i'').ToString()
        let value = succeed 10
        let value' = fail "Second value failure"
        let value'' = succeed 2
        let result = lift3R intToString value value' value''
        match result with
            | Success _ -> failBranch()
            | Failure errors-> errors.[0] |> should equal "Second value failure"

    [<Fact>]
    let ``Litf three functions and not applies it if one the results are on the Failure branch''``() =
        let intToString i i' i'' = (i * i' + i'').ToString()
        let value = succeed 10
        let value' = succeed 5
        let value'' = fail "Third value failure"
        let result = lift3R intToString value value' value''
        match result with
            | Success _ -> failBranch()
            | Failure errors-> errors.[0] |> should equal "Third value failure"

    [<Fact>]
    let ``Litf three functions and not applies it if one the results are on the Failure branch'''``() =
        let expectedErrors = ["First value failure"; "Third value failure"]
        let intToString i i' i'' = (i * i' + i'').ToString()
        let value = fail expectedErrors.[0]
        let value' = succeed 5
        let value'' = fail expectedErrors.[1]
        let result = lift3R intToString value value' value''
        match result with
            | Success _ -> failBranch()
            | Failure errors-> errors |> List.iteri (fun i e -> e |> should equal expectedErrors.[i])