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