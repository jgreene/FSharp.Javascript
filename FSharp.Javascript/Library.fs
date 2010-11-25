module FSharp.Javascript.Library

open System

type System.DateTime with
//    [<ReflectedDefinition>]
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

let (?) (this : 'Source) (prop : string) : 'Result =
          let p = this.GetType().GetProperty(prop)
          p.GetValue(this, null) :?> 'Result

let (?<-) (this : 'Source) (property : string) (value : 'Value) =
    this.GetType().GetProperty(property).SetValue(this, value, null)