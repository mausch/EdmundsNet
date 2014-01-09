open Fuchu
open EdmundsNet
open EdmundsNet.Vehicles
open Fleece

let integrationTests = 
    let apiKey = ""
    testList "Integration" [
        test "VIN lookup" {
            let response = lookupByVIN "2G1FC3D33C9165616" apiKey |> Async.RunSynchronously
            printfn "%A" response
            ()
        }
    ]

let tests = 
    testList "Deserialization" [
        test "VIN lookup response" {
            // https://api.edmunds.com/api/vehicle/v2/vins/2G1FC3D33C9165616?fmt=json&api_key=

            let json = """
{
    "make": {
        "id": 200000404,
        "name": "Chevrolet",
        "niceName": "chevrolet"
    },
    "model": {
        "id": "Chevrolet_Camaro",
        "name": "Camaro",
        "niceName": "camaro"
    },
    "drivenWheels": "rear wheel drive",
    "numOfDoors": "2",
    "options": [],
    "colors": [],
    "manufacturerCode": "1EH67",
    "price": {
        "baseMSRP": 34180.0,
        "baseInvoice": 32813.0,
        "deliveryCharges": 900.0,
        "usedTmvRetail": 24237.0,
        "usedPrivateParty": 22956.0,
        "usedTradeIn": 21258.0,
        "estimateTmv": false,
        "tmvRecommendedRating": 0
    },
    "categories": {
        "market": "Performance",
        "EPAClass": "Compact Cars",
        "vehicleSize": "Midsize",
        "primaryBodyType": "Car",
        "vehicleStyle": "Convertible",
        "vehicleType": "Car"
    },
    "vin": "2G1FC3D33C9165616",
    "squishVin": "2G1FC3D3C9",
    "years": [{
        "id": 100531911,
        "year": 2012,
        "styles": [{
            "id": 101395591,
            "name": "LT 2dr Convertible w/2LT (3.6L 6cyl 6M)",
            "submodel": {
                "body": "Convertible",
                "modelName": "Camaro Convertible",
                "niceName": "convertible"
            },
            "trim": "LT"
        }]
    }],
    "matchingType": "SQUISHVIN",
    "MPG": {
        "highway": "28",
        "city": "17"
    }
}
"""
            let response : VINLookupResponse ChoiceS = parseJSON json
            match response with
            | Choice2Of2 e -> failtestf "Failed parsing: %s" e
            | Choice1Of2 r -> 
                printfn "%A" r
                ()
            ()
        }
    ]

[<EntryPoint>]
let main argv = 
    //run tests
    run integrationTests
