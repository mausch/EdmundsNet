namespace EdmundsNet

open EdmundsNet.Vehicles

type Service(apiKey: string) =
    member x.AsyncLookupByVIN(vin: string) =
        lookupByVIN vin apiKey

