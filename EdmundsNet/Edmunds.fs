namespace EdmundsNet

module Basics =
    open System
    open System.Net
    open System.Web
    open System.Json
    open Fleece

    let newHttpRequest (uri: Uri) =
        WebRequest.Create uri :?> HttpWebRequest

    let private baseEndpoint = "https://api.edmunds.com/api/vehicle/v2"
    // https://api.edmunds.com/api/vehicle/v2/vins/2G1FC3D33C9165616?fmt=json&api_key=

    let buildUrl service apiKey = 
        let qs = HttpUtility.ParseQueryString ""
        qs.["api_key"] <- apiKey
        qs.["fmt"] <- "json"
        let url = baseEndpoint + service + "?" + qs.ToString()
        Uri url

    let inline doRequest service apiKey =
        async {
            let request = newHttpRequest (buildUrl service apiKey)
            try
                use! response = request.AsyncGetResponse()
                use responseStream = response.GetResponseStream()
                let json = JsonValue.Load responseStream
                return fromJSON json
            with 
            | :? WebException as e -> 
                return Choice2Of2 (e.Status.ToString())
            | e -> 
                return Choice2Of2 (e.ToString())
        }

module Vehicles =

    [<RequireQualifiedAccess>]
    type State = New | Used | Future

    type Submodel = {
        Body: string
        ModelName: string
        NiceName: string
        //Fuel: string option
    }

    type Style = {
        Id: int
        Name: string
        Submodel: Submodel
        Trim: string
    }

    type Year = {
        Id: int
        Year: int
        States: State list
    }

    type YearWithStyles = {
        Year: Year
        Styles: Style list
    }

    type Model = {
        Id: string
        Name: string
        NiceName: string
    }

    type Make = {
        Id: int
        Name: string
        NiceName: string
    }

    type Price = {
        BaseMSRP: decimal
        BaseInvoice: decimal
        EstimateTmv: bool
    }

    type Attribute = {
        Name: string
        Value: string
    }

    [<RequireQualifiedAccess>]
    type VehicleOptionCategory = 
    | Interior
    | Exterior
    | Roof
    | InteriorTrim
    | Mechanical
    | Package
    | AdditionalFees
    | Other
    
    type VehicleOption = {
        Id: int
        Name: string
        Description: string
        EquipmentType: string
        Availability: string
        ManufactureOptionName: string
        ManufactureOptionCode: string
        Category: VehicleOptionCategory
        Price: Price
        Attributes: Attribute list
        Equipment: string list
    }

    type Equipment = {
        Id: string
        Name: string
        EquipmentType: string
        Availability: string
        Attributes: Attribute list
    }

    type Engine = {
        Id: string
        Name: string
        EquipmentType: string
        Availability: string
        CompressionRatio: decimal
        Cylinder: int
        Size: decimal
        Displacement: decimal
        Configuration: string
        FuelType: string
        HorsePower: int
        Torque: int
        TotalValves: int
        ManufacturerEngineCode: string
        Type: string
        Code: string
        CompressorType: string
    }

    type Transmission = {
        Id: string
        Name: string
        EquipmentType: string
        Availability: string
        AutomaticType: string
        TransmissionType: string
        NumberOfSpeeds: string
    }

    [<RequireQualifiedAccess>]
    type GeneralEquipment = 
    | Equipment of Equipment 
    | Engine of Engine 
    | Transmission of Transmission

    type Equipments = {
        Equipment: GeneralEquipment list
    }

    type VehicleCategory = {
        Market: string
        EPAClass: string
        VehicleSize: string
        PrimaryBodyType: string
        VehicleStyle: string
        VehicleType: string
    }

    type MPG = {
        Highway: string
        City: string
    }

    type VINLookupResponse = {
        Make: Make
        Model: Model
        DrivenWheels: string
        NumOfDoors: string
        Options: string list
        Colors: string list
        ManufacturerCode: string
        Price: Price
        Categories: VehicleCategory
        VIN: string
        SquishVIN: string
        Years: YearWithStyles list
        MatchingType: string
        MPG: MPG
    }


    // deserializers

    open FSharpPlus
    open System.Json
    open Fleece

    type Attribute with
        static member instance (FromJSON, _: Attribute, _: Attribute ChoiceS) =
            function
            | JObject o ->
                monad {
                    let! name = o .> "name"
                    let! value = o .> "value"
                    return { 
                        Attribute.Name = name
                        Value = value
                    }
                }
            | x -> Failure ("Expected attribute object, found " + x.ToString())

    type Submodel with
        static member instance (FromJSON, _: Submodel, _: Submodel ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! body = o .> "body"
                    let! modelName = o .> "modelName"
                    let! niceName = o .> "niceName"
                    return { 
                        Submodel.Body = body
                        ModelName = modelName
                        NiceName = niceName
                    }
                }
            | _ -> Failure ("Expected submodel object, found " + x.ToString())

    type Style with
        static member instance (FromJSON, _: Style, _: Style ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    let! name = o .> "name"
                    let! trim = o .> "trim"
                    let! submodel = o .> "submodel"
                    return { 
                        Style.Id = id
                        Name = name
                        Trim = trim
                        Submodel = submodel
                    }
                }
            | _ -> Failure ("Expected style object, found " + x.ToString())

    type Make with
        static member instance (FromJSON, _: Make, _: Make ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    let! name = o .> "name"
                    let! niceName = o .> "niceName"
                    return {
                        Make.Id = id
                        Name = name
                        NiceName = niceName
                    }
                }
            | _ -> Failure ("Expected make object, found " + x.ToString())

    type Model with
        static member instance (FromJSON, _: Model, _: Model ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    let! name = o .> "name"
                    let! niceName = o .> "niceName"
                    return {
                        Model.Id = id
                        Name = name
                        NiceName = niceName
                    }
                }
            | _ -> Failure ("Expected model object, found " + x.ToString())

    type MPG with
        static member instance (FromJSON, _: MPG, _: MPG ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! highway = o .> "highway"
                    let! city = o .> "city"
                    return {
                        MPG.Highway = highway
                        City = city
                    }
                }
            | _ -> Failure ("Expected MPG object, found " + x.ToString())

    type Price with
        static member instance (FromJSON, _: Price, _: Price ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! msrp = o .> "baseMSRP"
                    let! baseInvoice = o .> "baseInvoice"
                    let! tmv = o .> "estimateTmv"
                    return {
                        Price.BaseMSRP = msrp
                        BaseInvoice = baseInvoice
                        EstimateTmv = tmv
                    }
                }
            | _ -> Failure ("Expected price object, found " + x.ToString())

    type VehicleCategory with
        static member instance (FromJSON, _: VehicleCategory, _: VehicleCategory ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! market = o .> "market"
                    let! epaclass = o .> "EPAClass"
                    let! vehicleSize = o .> "vehicleSize"
                    let! primaryBodyType = o .> "primaryBodyType"
                    let! vehicleStyle = o .> "vehicleStyle"
                    let! vehicleType = o .> "vehicleType"
                    return {
                        VehicleCategory.Market = market
                        EPAClass = epaclass
                        VehicleSize = vehicleSize
                        PrimaryBodyType = primaryBodyType
                        VehicleStyle = vehicleStyle
                        VehicleType = vehicleType
                    }
                }
            | _ -> Failure ("Expected price object, found " + x.ToString())

    type Year with
        static member instance (FromJSON, _: Year, _: Year ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    let! year = o .> "year"
                    return {
                        Year.Id = id
                        Year = year
                        States = []
                    }
                }
            | _ -> Failure ("Expected Year object, found " + x.ToString())

    type YearWithStyles with
        static member instance (FromJSON, _: YearWithStyles, _: YearWithStyles ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! year = fromJSON x
                    let! styles = o .> "styles"
                    return {
                        YearWithStyles.Year = year
                        Styles = styles
                    }
                }
            | _ -> Failure ("Expected YearWithStyles object, found " + x.ToString())
            
    type VINLookupResponse with
        static member instance (FromJSON, _: VINLookupResponse, _: VINLookupResponse ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! make = o .> "make"
                    let! model = o .> "model"
                    let! wheels = o .> "drivenWheels"
                    let! doors = o .> "numOfDoors"
                    let! options = o .> "options"
                    let! colors = o .> "colors"
                    let! price = o .> "price"
                    let! category = o .> "categories"
                    let! mpg = o .> "MPG"
                    let! years = o .> "years"
                    return {
                        VINLookupResponse.Make = make
                        Model = model
                        DrivenWheels = wheels
                        NumOfDoors = doors
                        Options = options
                        Colors = colors
                        ManufacturerCode = ""
                        Price = price
                        Categories = category
                        VIN = ""
                        SquishVIN = ""
                        Years = years
                        MatchingType = ""
                        MPG = mpg
                    }
                }
            | _ -> Failure ("Expected VINLookupResponse object, found " + x.ToString())


    type Equipment with
        static member instance (FromJSON, _:Equipment, _: Equipment ChoiceS) = fun (x: JsonValue) ->
            match x with
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    let! name = o .> "name"
                    let! equipmentType = o .> "equipmentType"
                    let! availability = o .> "availability"
                    let! attributes = o .> "attributes"
                    return {
                        Equipment.Id = id
                        Name = name
                        EquipmentType = equipmentType
                        Availability = availability
                        Attributes = attributes
                    }
                }
            | _ -> Failure ("Expected Equipment object, found " + x.ToString())

    type Engine with
        static member instance (FromJSON, _:Engine, _:Engine ChoiceS) =
            function
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    let! name = o .> "name"
                    let! equipmentType = o .> "equipmentType"
                    let! availability = o .> "availability"
                    return {
                        Engine.Id = id
                        Name = name
                        EquipmentType = equipmentType
                        Availability = availability
                        CompressionRatio = 10m
                        Cylinder = 8
                        Size = 8m
                        Displacement = 123m
                        Configuration = ""
                        FuelType = ""
                        HorsePower = 8
                        Torque = 8
                        TotalValves = 23
                        ManufacturerEngineCode = ""
                        Type = ""
                        Code = ""
                        CompressorType = ""
                    }
                }
            | x -> Failure ("Expected Engine object, found " + x.ToString())

    type Transmission with
        static member instance (FromJSON, _: Transmission, _: Transmission ChoiceS) =
            function
            | JObject o ->
                monad {
                    let! id = o .> "id"
                    return {
                        Transmission.Id = id
                        Name = ""
                        EquipmentType = ""
                        Availability = ""
                        AutomaticType = ""
                        TransmissionType = ""
                        NumberOfSpeeds = ""
                    }
                }
            | x -> Failure ("Expected Transmission object, found " + x.ToString())

    type GeneralEquipment with
        static member instance (FromJSON, _:GeneralEquipment, _: GeneralEquipment ChoiceS) =
            function
            | JObject o as x ->
                monad {
                    let! equipmentType = o .> "equipmentType"
                    return!
                        match equipmentType with
                        | "TRANSMISSION" -> fromJSON x |> map GeneralEquipment.Transmission
                        | "ENGINE" -> fromJSON x |> map GeneralEquipment.Engine
                        | _ -> fromJSON x |> map GeneralEquipment.Equipment
                }
            | x -> Failure ("Expected GeneralEquipment object, found " + x.ToString())

    type Equipments with
        static member instance (FromJSON, _: Equipments, _: Equipments ChoiceS) =
            function
            | JObject o ->
                o .> "equipment" |> map (fun e -> { Equipments.Equipment = e })
            | x -> Failure ("Expected Equipments object, found " + x.ToString())
            
