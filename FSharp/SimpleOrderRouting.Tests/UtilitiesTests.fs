namespace SimpleOrderRouting.Tests

open Xunit
open FsUnit.Xunit
open SimpleOrderRouting.Journey1.Rop

module ``Rop tests`` =

    [<Fact>]
    let ``Create a Success with no messages``() =
        let result = succeed 10
        match result with
            | Success v -> v |> should equal 10
            | Failure errors -> errors.Length |> should equal 0