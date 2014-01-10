namespace EdmundsNet

open EdmundsNet.Basics
open EdmundsNet.Vehicles
open Fleece

type Service(apiKey: string) =
    member x.AsyncLookupByVIN(vin: string) : VINLookupResponse ChoiceS Async =
        let service = "/vins/" + vin
        doRequest service apiKey

    member x.AsyncGetEquipmentByStyle(styleID: int) : Equipments ChoiceS Async =
        let service = sprintf "/styles/%d/equipment" styleID
        doRequest service apiKey