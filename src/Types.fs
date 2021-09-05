module Types

open Fable.Core

[<StringEnum>]
type Kind =
    | [<CompiledName("primary")>] Primary
    | [<CompiledName("link")>] Link
    | [<CompiledName("info")>] Info
    | [<CompiledName("success")>] Success
    | [<CompiledName("warning")>] Warning
    | [<CompiledName("danger")>] Danger
    | [<CompiledName("default")>] Default

[<StringEnum; RequireQualifiedAccess>]
type OffcanvasPosition =
    | [<CompiledName("left")>] Left
    | [<CompiledName("right")>] Right
