namespace FSharp.Javascript.Ast

open System.Linq.Expressions


type node =
| Assign of node * node
| AutoProperty of obj * node
| BinaryOp of node * node * ExpressionType
| Identifier of string * bool
| Number of int option * float option
| Function of node * node list * string option
| AssignmentBlock of node list * bool
| Block of node list
| Boolean of bool
| BreakNode of string
| ForInNode of node * node * node
| ForStepNode of node * node * node * node
| Call of node * node list
| Catch of node * node
| Continue of string
| Delete of node
| If of node * node * node option * bool
| IndexAccess of node * node
| In of node * node
| InstanceOf of node * node
| Logical of node * node* ExpressionType
| MemberAccess of string * node
| New of node * node list * node list option
| NewArray of node * node list
| Null
| PostfixOperator of node * ExpressionType
| Regex of string * string
| Return of node
| StrictCompare of node * node * ExpressionType
| Switch of node * node * (node * node) list * string
| String of string * char
| Throw of node
| Try of node * node option * node option
| TypeOf of node
| UnaryOp of node * ExpressionType
| UnsignedRightShift of node * node
| Void of node
| While of node * node * bool
| With of node * node
| Ignore
