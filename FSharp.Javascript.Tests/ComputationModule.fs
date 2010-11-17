module TestNamespace.ComputationModule

let test (quote:Microsoft.FSharp.Quotations.Expr) = 
    let typ = System.Type.GetType("TestNamespace.ComputationModule, FSharp.Javascript.Tests")
    QuotationsTestHelper.testWithType [typ] quote

type MaybeBuilder() =
    [<ReflectedDefinition>]
    member this.Bind(x,f) =
        match x with
        | Some(t) when t >= 0 && t <= 100 -> f(t)
        | _ -> None
    [<ReflectedDefinition>]
    member this.Delay(f) = f()
    [<ReflectedDefinition>]
    member this.Return(x) = Some x

[<ReflectedDefinition>]
let maybe = new MaybeBuilder()


[<ReflectedDefinition>]
let print() = System.Console.WriteLine("got here two")

[<ReflectedDefinition>]
print()


(*State Monad*)
type State<'a, 'state> = State of ('state -> 'a * 'state)

type StateBuilder() =
  [<ReflectedDefinition>]
  member x.YieldFrom m = m
  [<ReflectedDefinition>]
  member x.ReturnFrom m = m
  [<ReflectedDefinition>]
  member x.Return a = State(fun s -> a, s)
  [<ReflectedDefinition>]
  member x.Bind(m, f) = State (fun s -> let v, s2 = let (State f_) = m in f_ s
                                        let (State f2) = f v in f2 s2)
  [<ReflectedDefinition>]
  member x.Zero() = x.Return(())

[<ReflectedDefinition>]
let state = new StateBuilder()
[<ReflectedDefinition>]
let getState = State(fun s -> s, s)
[<ReflectedDefinition>]
let setState s = State(fun _ -> (), s) 
[<ReflectedDefinition>]
let executeState m s = let (State f) = m in f s

[<ReflectedDefinition>]
let addInt t = state {
                    let! a = state { return t::[] }
                    return a
               }

type node =
    | Directory of string * node list
    | File of string