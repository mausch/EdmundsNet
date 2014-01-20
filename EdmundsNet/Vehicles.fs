namespace EdmundsNet.Vehicles

open System

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
    BaseInvoice: decimal option
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
    //CompressionRatio: decimal
    //Cylinder: int
    //Size: decimal
    //Displacement: decimal
    //Configuration: string
    //FuelType: string
    //HorsePower: int
    //Torque: int
    //TotalValves: int
    //ManufacturerEngineCode: string
    //Type: string
    Code: string
    CompressorType: string
}

type Transmission = {
    Id: string
    Name: string
    EquipmentType: string
    Availability: string
    AutomaticType: string option
    TransmissionType: string
    NumberOfSpeeds: string
}

[<RequireQualifiedAccess>]
type GeneralEquipment = 
| Equipment of Equipment 
| Engine of Engine 
| Transmission of Transmission
with 
    member x.Match(equipment: Func<_,_>, engine: Func<_,_>, transmission: Func<_,_>) =
        match x with
        | Equipment e -> equipment.Invoke e
        | Engine e -> engine.Invoke e
        | Transmission t -> transmission.Invoke t
        

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

type Color = {
    Id: string
    Category: string
    Name: string
    Availability: string
}

type VINLookupResponse = {
    Make: Make
    Model: Model
    DrivenWheels: string option
    NumOfDoors: string
    //Options: string list
    Colors: Color list
    ManufacturerCode: string
    Price: Price option
    Categories: VehicleCategory
    VIN: string
    SquishVIN: string
    Years: YearWithStyles list
    MatchingType: string
    MPG: MPG option
}


// deserializers

open FSharpPlus
open System.Json
open Fleece


type Attribute with
    static member FromJSON (_: Attribute) =
        function
        | JObject o ->
            monad {
                let! name = jget o "name"
                let! value = jget o "value"
                return { 
                    Attribute.Name = name
                    Value = value
                }
            }
        | x -> Failure ("Expected attribute object, found " + x.ToString())

type Submodel with
    static member FromJSON (_: Submodel) =
        function
        | JObject o ->
            monad {
                let! body = jget o "body"
                let! modelName = jget o "modelName"
                let! niceName = jget o "niceName"
                return { 
                    Submodel.Body = body
                    ModelName = modelName
                    NiceName = niceName
                }
            }
        | x -> Failure ("Expected submodel object, found " + x.ToString())

type Style with
    static member FromJSON (_: Style) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! name = jget o "name"
                let! trim = jget o "trim"
                let! submodel = jget o "submodel"
                return { 
                    Style.Id = id
                    Name = name
                    Trim = trim
                    Submodel = submodel
                }
            }
        | x -> Failure ("Expected style object, found " + x.ToString())

type Make with
    static member FromJSON (_: Make) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! name = jget o "name"
                let! niceName = jget o "niceName"
                return {
                    Make.Id = id
                    Name = name
                    NiceName = niceName
                }
            }
        | x -> Failure ("Expected make object, found " + x.ToString())

type Model with
    static member FromJSON (_: Model) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! name = jget o "name"
                let! niceName = jget o "niceName"
                return {
                    Model.Id = id
                    Name = name
                    NiceName = niceName
                }
            }
        | x -> Failure ("Expected model object, found " + x.ToString())

type MPG with
    static member FromJSON (_: MPG) =
        function
        | JObject o ->
            monad {
                let! highway = jget o "highway"
                let! city = jget o "city"
                return {
                    MPG.Highway = highway
                    City = city
                }
            }
        | x -> Failure ("Expected MPG object, found " + x.ToString())

type Price with
    static member FromJSON (_: Price) =
        function
        | JObject o ->
            monad {
                let! msrp = jget o "baseMSRP"
                let! baseInvoice = jgetopt o "baseInvoice"
                let! tmv = jget o "estimateTmv"
                return {
                    Price.BaseMSRP = msrp
                    BaseInvoice = baseInvoice
                    EstimateTmv = tmv
                }
            }
        | x -> Failure ("Expected price object, found " + x.ToString())

type VehicleCategory with
    static member FromJSON (_: VehicleCategory) =
        function
        | JObject o ->
            monad {
                let! market = jget o "market"
                let! epaclass = jget o "EPAClass"
                let! vehicleSize = jget o "vehicleSize"
                let! primaryBodyType = jget o "primaryBodyType"
                let! vehicleStyle = jget o "vehicleStyle"
                let! vehicleType = jget o "vehicleType"
                return {
                    VehicleCategory.Market = market
                    EPAClass = epaclass
                    VehicleSize = vehicleSize
                    PrimaryBodyType = primaryBodyType
                    VehicleStyle = vehicleStyle
                    VehicleType = vehicleType
                }
            }
        | x -> Failure ("Expected price object, found " + x.ToString())

type Year with
    static member FromJSON (_: Year) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! year = jget o "year"
                return {
                    Year.Id = id
                    Year = year
                    States = []
                }
            }
        | x -> Failure ("Expected Year object, found " + x.ToString())

type YearWithStyles with
    static member FromJSON (_: YearWithStyles) =
        function
        | JObject o as x ->
            monad {
                let! year = fromJSON x
                let! styles = jget o "styles"
                return {
                    YearWithStyles.Year = year
                    Styles = styles
                }
            }
        | x -> Failure ("Expected YearWithStyles object, found " + x.ToString())


type Equipment with
    static member FromJSON (_:Equipment) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! name = jget o "name"
                let! equipmentType = jget o "equipmentType"
                let! availability = jget o "availability"
                let attributes = 
                    match jget o "attributes" with
                    | Success x -> x
                    | Failure _ -> []

                return {
                    Equipment.Id = id
                    Name = name
                    EquipmentType = equipmentType
                    Availability = availability
                    Attributes = attributes
                }
            }
        | x -> Failure ("Expected Equipment object, found " + x.ToString())

type Color with
    static member FromJSON (_: Color) =
        function
        | JObject o ->
            monad {
                let! category = jget o "category"
                let! fakeEquipments = jget o "options" : Equipment list ParseResult
                match fakeEquipments with
                | fakeEquipment::_ -> 
                    return {
                        Color.Id = fakeEquipment.Id
                        Category = category
                        Name = fakeEquipment.Name
                        Availability = fakeEquipment.Availability
                    }
                | _ -> return! Failure (sprintf "Expected at least one option for color. Found: %A" o)
            }
        | x -> Failure (sprintf "Expected color object, found %A" x)
                    

type Engine with
    static member FromJSON (_:Engine) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! name = jget o "name"
                let! equipmentType = jget o "equipmentType"
                let! availability = jget o "availability"
                //let! compressionRatio = jget o "compressionRatio"
                //let! cylinder = jget o "cylinder"
                //let! size = jget o "size"
                //let! displacement = jget o "displacement"
                //let! configuration = jget o "configuration"
                //let! fuelType = jget o "fuelType"
                //let! horsepower = jget o "horsepower"
                //let! torque = jget o "torque"
                //let! totalValves = jget o "totalValves"
                //let! manufacturerEngineCode = jget o "manufacturerEngineCode"
                //let! typ = jget o "type"
                let! code = jget o "code"
                let! compressorType = jget o "compressorType"
                return {
                    Engine.Id = id
                    Name = name
                    EquipmentType = equipmentType
                    Availability = availability
                    //CompressionRatio = compressionRatio
                    //Cylinder = cylinder
                    //Size = size
                    //Displacement = displacement
                    //Configuration = configuration
                    //FuelType = fuelType
                    //HorsePower = horsepower
                    //Torque = torque
                    //TotalValves = totalValves
                    //ManufacturerEngineCode = manufacturerEngineCode
                    //Type = typ
                    Code = code
                    CompressorType = compressorType
                }
            }
        | x -> Failure ("Expected Engine object, found " + x.ToString())

type Transmission with
    static member FromJSON (_: Transmission) =
        function
        | JObject o ->
            monad {
                let! id = jget o "id"
                let! name = jget o "name"
                let! equipmentType = jget o "equipmentType"
                let! availability = jget o "availability"
                let! automaticType = jgetopt o "automaticType"
                let! transmissionType = jget o "transmissionType"
                let! numberOfSpeeds = jget o "numberOfSpeeds"
                return {
                    Transmission.Id = id
                    Name = name
                    EquipmentType = equipmentType
                    Availability = availability
                    AutomaticType = automaticType
                    TransmissionType = transmissionType
                    NumberOfSpeeds = numberOfSpeeds
                }
            }
        | x -> Failure ("Expected Transmission object, found " + x.ToString())

type GeneralEquipment with
    static member FromJSON (_:GeneralEquipment) =
        function
        | JObject o as x ->
            monad {
                let! equipmentType = jget o "equipmentType"
                return!
                    match equipmentType with
                    | "TRANSMISSION" -> fromJSON x |> map GeneralEquipment.Transmission
                    | "ENGINE" -> fromJSON x |> map GeneralEquipment.Engine
                    | _ -> fromJSON x |> map GeneralEquipment.Equipment
            }
        | x -> Failure ("Expected GeneralEquipment object, found " + x.ToString())

type Equipments with
    static member FromJSON (_: Equipments) =
        function
        | JObject o ->
            jget o "equipment" |> map (fun e -> { Equipments.Equipment = e })
        | x -> Failure ("Expected Equipments object, found " + x.ToString())

type VINLookupResponse with
    static member FromJSON (_: VINLookupResponse) =
        function
        | JObject o ->
            monad {
                let! make = jget o "make"
                let! model = jget o "model"
                let! wheels = jgetopt o "drivenWheels"
                let! doors = jget o "numOfDoors"
                //let! options = jget o "options"
                let! colors = jget o "colors"
                let! price = jgetopt o "price"
                let! category = jget o "categories"
                let! mpg = jgetopt o "MPG"
                let! years = jget o "years"
                return {
                    VINLookupResponse.Make = make
                    Model = model
                    DrivenWheels = wheels
                    NumOfDoors = doors
                    //Options = options
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
        | x -> Failure ("Expected VINLookupResponse object, found " + x.ToString())
