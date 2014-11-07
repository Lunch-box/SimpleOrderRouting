namespace SimpleOrderRouting.Tests

open Xunit
open FsUnit.Xunit
open SimpleOrderRouting.Journey1.Rop

module ``Rop tests`` =

    [<Fact>]
    let ``Create a Success with no messages``() =
        
        true |> should be True