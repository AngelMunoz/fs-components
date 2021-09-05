[<RequireQualifiedAccess>]
module Offcanvas


open Fable.Core
open Lit
open Haunted
open Browser.Types
open ShadowStyles
open Types
open Styles


[<AllowNullLiteral>]
type FsOffCanvasElement =
    inherit HTMLElement

    abstract member isOpen : bool option with get, set
    abstract member noOverlay : bool option with get, set
    abstract member closable : bool option with get, set
    abstract member position : OffcanvasPosition option with get, set
    abstract member kind : Kind option with get, set


let private styles =
    [ fsComponentsStyles
      fsOffcanvasStyles ]

let private tryClose (host: FsOffCanvasElement) _ =
    let evt =
        Haunted.createEvent (
            "fs-close-off-canvas",
            {| composed = true
               bubbles = true
               cancelable = true |}
        )

    host |> dispatchEvent evt

let initializeHost (host: FsOffCanvasElement) _ =
    match host.isOpen with
    | Some _ -> ()
    | None -> host.isOpen <- Some false

    match host.closable with
    | Some _ -> ()
    | None -> host.closable <- Some false

    match host.noOverlay with
    | Some _ -> ()
    | None -> host.noOverlay <- Some false

    match host.position with
    | Some _ -> ()
    | None -> host.position <- Some OffcanvasPosition.Left

    match host.kind with
    | Some _ -> ()
    | None -> host.kind <- Some Kind.Default

let private OffCanvas (host: FsOffCanvasElement) =
    ShadowStyles.adoptStyleSheets (host, styles)

    Haunted.useEffect (initializeHost host, [||])

    let fallbackContent () =
        match host.closable with
        | Some true -> html $"""<button class="delete" @click={(tryClose host)}>&times;</button>"""
        | _ -> Lit.nothing

    let overlayContent (classes: (string * bool) seq) =
        let handler =
            match host.closable with
            | Some true -> (tryClose host) |> Some
            | _ -> None

        let classes () =
            seq {
                "overlay", true
                yield! classes
            }
            |> Lit.classes

        match host.noOverlay with
        | Some true -> Lit.nothing
        | _ ->
            html
                $"""
                <div
                    class={classes ()}
                    @click={handler}>
                </div>
                """

    let asideClasses () =
        seq {
            match host.position with
            | Some OffcanvasPosition.Right -> "is-right", true
            | _ -> "is-right", false

            match host.kind with
            | Some Primary -> "is-primary", true
            | Some Link -> "is-link", true
            | Some Success -> "is-success", true
            | Some Info -> "is-info", true
            | Some Warning -> "is-warning", true
            | Some Danger -> "is-danger", true
            | _ -> "is-default", true
        }

    match host.isOpen with
    | Some true ->
        let classes = asideClasses ()

        html
            $"""
             {overlayContent classes}
             <aside class={Lit.classes classes}>
                <header>
                    <slot name="header-text"></slot>
                    <slot name="header-icon">
                        {fallbackContent ()}
                    </slot>
                </header>
                <slot></slot>
             </aside>
             """
    | _ -> Lit.nothing

[<Emit("OffCanvas")>]
let private OffCanvasRef: obj = jsNative

let register () =
    defineComponent
        "fs-off-canvas"
        (Haunted.Component(
            OffCanvasRef,
            {| observedAttributes =
                   [| "is-open"
                      "position"
                      "closable"
                      "no-overlay"
                      "kind" |] |}
        ))
