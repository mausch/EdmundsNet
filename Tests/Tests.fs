open Fuchu
open EdmundsNet
open EdmundsNet.Vehicles
open Fleece


let integrationTests = 
    let edmunds = Service(apiKey = "")
    testList "Integration" [
        test "VIN lookup" {
            let response = edmunds.AsyncLookupByVIN "2G1FC3D33C9165616" |> Async.RunSynchronously
            printfn "%A" response
            ()
        }
    ]

let tests = 
    testList "Deserialization" [
        test "engine" {
            let json = """{"id":"200384237","name":"Engine","equipmentType":"ENGINE","availability":"OPTIONAL","options":[{"id":"200384236","name":"Keyless Go","description":"Transponder which lets the driver lock\/unlock the vehicle and start\/stop the engine without using a key or pressing a button on a remote keyfob","equipmentType":"OPTION","availability":"All","manufactureOptionName":"KEYLESS GO","manufactureOptionCode":"889","category":"Interior","price":{"baseMSRP":650.0,"baseInvoice":605.0,"estimateTmv":false},"attributes":[{"name":"Keyless Ignition","value":"keyless ignition"}]}],"code":"nullNAG","compressorType":"NA"}"""
            let response : Engine ParseResult = parseJSON json
            match response with
            | Choice2Of2 e -> failtestf "Failed parsing: %s" e
            | Choice1Of2 r -> 
                //printfn "%A" r
                ()
        }

        test "color" {
            let json = """
{
    "category": "Exterior",
    "options": [{
        "id": "200384178",
        "name": "Arctic White",
        "equipmentType": "COLOR",
        "availability": "USED"
    }]
}"""
            let response : Color ParseResult = parseJSON json
            match response with
            | Choice2Of2 e -> failtestf "Failed parsing: %s" e
            | Choice1Of2 r -> 
                //printfn "%A" r
                ()
        }

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
            let response : VINLookupResponse ParseResult = parseJSON json
            match response with
            | Choice2Of2 e -> failtestf "Failed parsing: %s" e
            | Choice1Of2 r -> 
                //printfn "%A" r
                ()
            ()
        }

        test "Equipment details by style id" {
            let json = """
{
    "equipment": [{
        "id": "10121261918",
        "name": "MIRRORS",
        "equipmentType": "MIRRORS",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Auto Dimming Rearview Mirror",
            "value": "electrochromatic"
        }, {
            "name": "1st Row Vanity Mirrors",
            "value": "dual illuminated vanity mirrors"
        }, {
            "name": "Side Mirror Memory",
            "value": "includes exterior mirrors"
        }, {
            "name": "Auto Dimming Side Mirrors",
            "value": "electrochromatic"
        }, {
            "name": "Heated Driver Side Mirror",
            "value": "heated"
        }, {
            "name": "Passenger Side Mirror Adjustment",
            "value": "power"
        }, {
            "name": "Power Retractable Side Mirrors",
            "value": "power retractable mirrors"
        }, {
            "name": "Reverse Tilt Side Mirrors",
            "value": "passenger mirror"
        }, {
            "name": "Driver Side Mirror Adjustment",
            "value": "power"
        }, {
            "name": "Heated Passenger Side Mirror",
            "value": "heated"
        }]
    }, {
        "id":"200060801",
        "name":"Engine",
        "equipmentType":"ENGINE",
        "availability":"STANDARD",
        "compressionRatio":10.0,
        "cylinder":8,
        "size":4.4,
        "displacement":4395.0,
        "configuration":"V",
        "fuelType":"premium unleaded (required)",
        "horsepower":560,
        "torque":500,
        "totalValves":32,
        "manufacturerEngineCode":"S63Tu",
        "type":"gas",
        "code":"8VTTG4.4",
        "compressorType":"twin turbocharger"
    }, {
        "id": "200477468",
        "name": "8A",
        "equipmentType": "TRANSMISSION",
        "availability": "STANDARD",
        "automaticType": "Shiftable automatic",
        "transmissionType": "AUTOMATIC",
        "numberOfSpeeds": "8"
    }, {
        "id": "10121261921",
        "name": "INSTRUMENTATION",
        "equipmentType": "OTHER",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Tire Pressure Monitoring System",
            "value": "tire pressure monitoring"
        }, {
            "name": "Low Fuel Level Indicator",
            "value": "low fuel level"
        }, {
            "name": "Tachometer",
            "value": "tachometer"
        }, {
            "name": "Trip Computer",
            "value": "trip computer"
        }, {
            "name": "Clock",
            "value": "clock"
        }]
    }, {
        "id": "10121261926",
        "name": "EXTERIOR_DIMENSIONS",
        "equipmentType": "OTHER",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Overall Width Without Mirrors",
            "value": "74.4"
        }, {
            "name": "Wheelbase",
            "value": "116.7"
        }, {
            "name": "Overall Length",
            "value": "193.5"
        }, {
            "name": "Minimum Ground Clearance",
            "value": "4.6"
        }, {
            "name": "Overall Height",
            "value": "57.3"
        }, {
            "name": "Overall Width With Mirrors",
            "value": "83.4"
        }, {
            "name": "Rear Track",
            "value": "62.3"
        }, {
            "name": "Front Track",
            "value": "64.1"
        }]
    }, {
        "id": "10121261927",
        "name": "FRONT_PASSENGER_SEAT",
        "equipmentType": "OTHER",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Height Adjustable Passenger Seat",
            "value": "height adjustable"
        }, {
            "name": "Passenger Seat Thigh Extension",
            "value": "passenger seat thigh extension"
        }, {
            "name": "Heated Passenger Seat",
            "value": "multi-level heating"
        }, {
            "name": "Passenger Seat Whiplash Protection",
            "value": "whiplash protection system"
        }, {
            "name": "Number Of Passenger Seat Power Adjustments",
            "value": "16"
        }, {
            "name": "Adjustable Passenger Seat Headrest",
            "value": "power adjustable headrests"
        }, {
            "name": "Passenger Seat Adjustable Lumbar",
            "value": "power adjustable lumbar support"
        }]
    }, {
        "id": "10121261929",
        "name": "SPECIFICATIONS",
        "equipmentType": "OTHER",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Gross Vehicle Weight",
            "value": "5313"
        }, {
            "name": "Epa Combined Mpg",
            "value": "16"
        }, {
            "name": "Manufacturer 0 100km Acceleration Time (seconds)",
            "value": "4.4"
        }, {
            "name": "Aerodynamic Drag (cd)",
            "value": "0.33"
        }, {
            "name": "Epa City Mpg",
            "value": "14"
        }, {
            "name": "Payload",
            "value": "970"
        }, {
            "name": "Turning Diameter",
            "value": "41.3"
        }, {
            "name": "Curb Weight",
            "value": "4387"
        }, {
            "name": "Tco Curb Weight",
            "value": "4387"
        }, {
            "name": "Epa Highway Mpg",
            "value": "20"
        }, {
            "name": "Manufacturer 0 60mph Acceleration Time (seconds)",
            "value": "4.2"
        }, {
            "name": "Fuel Capacity",
            "value": "21.1"
        }]
    }, {
        "id": "10121261936",
        "name": "INTERIOR_DIMENSIONS",
        "equipmentType": "OTHER",
        "availability": "STANDARD",
        "attributes": [{
            "name": "2nd Row Leg Room",
            "value": "36.1"
        }, {
            "name": "2nd Row Shoulder Room",
            "value": "56.2"
        }, {
            "name": "2nd Row Head Room",
            "value": "38.3"
        }, {
            "name": "1st Row Leg Room",
            "value": "41.4"
        }, {
            "name": "1st Row Head Room",
            "value": "40.5"
        }, {
            "name": "1st Row Shoulder Room",
            "value": "58.3"
        }]
    }, {
        "id": "200421170",
        "name": "Drivetrain, 4 years, 50000/L miles",
        "equipmentType": "WARRANTY",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Warranty Maximum Mileage",
            "value": "50000"
        }, {
            "name": "Warranty Miles Limited/unlimited",
            "value": "L"
        }, {
            "name": "Warranty Years Limited/unlimited",
            "value": "L"
        }, {
            "name": "Warranty Maximum Years",
            "value": "4"
        }, {
            "name": "Warranty Type",
            "value": "Drivetrain"
        }]
    }, {
        "id": "200421171",
        "name": "Roadside, 4 years, _/U miles",
        "equipmentType": "WARRANTY",
        "availability": "STANDARD",
        "attributes": [{
            "name": "Warranty Miles Limited/unlimited",
            "value": "U"
        }, {
            "name": "Warranty Years Limited/unlimited",
            "value": "L"
        }, {
            "name": "Warranty Maximum Years",
            "value": "4"
        }, {
            "name": "Warranty Type",
            "value": "Roadside"
        }]
    }, {
        "id": "200421173",
        "name": "Std. Audio system",
        "equipmentType": "AUDIO_SYSTEM",
        "availability": "STANDARD",
        "attributes": [{
            "name": "In Dash Cd",
            "value": "6 CD/DVD"
        }, {
            "name": "Total Number Of Speakers",
            "value": "12"
        }, {
            "name": "Audio Security System",
            "value": "audio security system"
        }, {
            "name": "Radio",
            "value": "AM/FM HD Radio"
        }, {
            "name": "Watts",
            "value": "500"
        }, {
            "name": "Surround Audio",
            "value": "5.1"
        }, {
            "name": "Subwoofer",
            "value": "2"
        }, {
            "name": "Digital Audio Input",
            "value": "auxiliary audio input and iPod integration"
        }, {
            "name": "Mp3 Player",
            "value": "CD MP3 Playback"
        }, {
            "name": "Antenna Type",
            "value": "diversity"
        }, {
            "name": "Usb Connection",
            "value": "USB connection"
        }, {
            "name": "Radio Data System",
            "value": "radio data system"
        }, {
            "name": "Dvd Audio",
            "value": "DVD-Audio"
        }]
    }],
    "equipmentCount": 44
}
"""
            let response : Equipments ParseResult = parseJSON json
            match response with
            | Choice2Of2 e -> failtestf "Failed parsing: %s" e
            | Choice1Of2 r -> 
                //printfn "%A" r
                ()
            ()
        }
    ]

[<EntryPoint>]
let main argv = 
    run tests
    //run integrationTests
