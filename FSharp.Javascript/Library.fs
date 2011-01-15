module FSharp.Javascript.Library

open System

type System.DateTime with
    static member TryParse2(x:string) =
        try
            let date = DateTime.Parse(x)
            if date = DateTime.MinValue then 
                None
            else
                Some(date)
        with
        | _ -> None

type System.Int16 with
    static member TryParse2(x:string) =
        try
            let success,result = Int16.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type System.Int32 with
    static member TryParse2(x:string) =
        try
            let success,result = Int32.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type System.Int64 with
    static member TryParse2(x:string) =
        try
            let success,result = Int64.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type System.Decimal with
    static member TryParse2(x:string) =
        try
            let success,result = Decimal.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type System.Double with
    static member TryParse2(x:string) =
        try
            let success,result = Double.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type System.Single with
    static member TryParse2(x:string) =
        try
            let success,result = Single.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type System.Boolean with
    static member TryParse2(x:string) =
        try
            let success,result = Boolean.TryParse(x)
            if success then Some result else None
        with
        | _ -> None

type JObject() =
    let dict = System.Collections.Hashtable()

    member this.Item
        with get(i:string) = dict.[i]
        and set (i:string) (v:obj) = dict.[i] <- v

let (?) (this : JObject) (prop : string) =
    this.[prop]

let (?<-) (this : JObject) (property : string) (value : 'Value) =
    this.[property] <- value