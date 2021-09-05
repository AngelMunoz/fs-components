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
    abstract member withOverlay : bool option with get, set
    abstract member closable : bool option with get, set
    abstract member position : OffcanvasPosition option with get, set


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


let private OffCanvas (host: FsOffCanvasElement) =
    ShadowStyles.adoptStyleSheets (host, styles)

    Haunted.useEffect (
        // ensure values are initialized
        (fun _ ->
            match host.isOpen with
            | Some true
            | Some false -> ()
            | None -> host.isOpen <- Some false

            match host.closable with
            | Some true
            | Some false -> ()
            | None -> host.closable <- Some false

            match host.withOverlay with
            | Some true
            | Some false -> ()
            | None -> host.withOverlay <- Some true

            match host.position with
            | Some pos -> host.position <- Some pos
            | None -> host.position <- Some OffcanvasPosition.Left),
        [||]
    )

    let fallbackContent () =
        if host.closable |> Option.defaultValue true then
            html $"""<button class="delete" @click={(tryClose host)}>&times;</button>"""
        else
            Lit.nothing

    let overlayContent () =
        JS.console.log (host)

        let handler =
            match host.closable with
            | Some true -> (tryClose host) |> Some
            | _ -> None

        match host.withOverlay with
        | Some true -> html $"""<div class="overlay" @click={handler}></div>"""
        | _ -> Lit.nothing

    match host.isOpen with
    | Some true ->
        html
            $"""
             {overlayContent ()}
             <aside>
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
                      "with-overlay" |] |}
        ))
