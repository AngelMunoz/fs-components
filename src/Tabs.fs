[<RequireQualifiedAccess>]
module Tabs


open Fable.Core
open Lit
open Haunted
open Fable.Core.JsInterop
open Browser.Types
open ShadowStyles
open Types
open Styles

[<AllowNullLiteral>]
type FsTabItemElement =
    inherit HTMLElement

    abstract member isSelected : bool option with get, set
    abstract member isClosable : bool option with get, set
    abstract member label : string option with get, set
    abstract member tabName : string option with get, set
    abstract member kind : Kind option with get, set


let private initializeTabItem (host: FsTabItemElement) _ =
    match host.isSelected with
    | Some _ -> ()
    | None -> host.isSelected <- Some false

    match host.isClosable with
    | Some _ -> ()
    | None -> host.isClosable <- Some false

    match host.kind with
    | Some _ -> ()
    | None -> host.kind <- Some Default

let private tabItemStyles = [ fsComponentsStyles; fsTabItemStyles ]

let private getClass (kind: Kind option) =
    seq {
        (match kind with
         | Some Primary -> "is-primary"
         | Some Info -> "is-info"
         | Some Link -> "is-link"
         | Some Success -> "is-success"
         | Some Warning -> "is-warning"
         | Some Danger -> "is-danger"
         | Some Default -> ""
         | None -> ""),
        true
    }


let private FsTabItem (host: FsTabItemElement) =
    ShadowStyles.adoptStyleSheets (host, tabItemStyles)
    Haunted.useEffect ((initializeTabItem host), [||])

    Haunted.useEffect (
        (fun _ ->
            match host.kind with
            | Some kind ->
                host.setAttribute ("kind", "")
                host.setAttribute ("kind", $"{kind}")
            | None -> ()

            ),
        [| host.kind |]
    )

    let onClose (ev: MouseEvent) =
        ev.preventDefault ()
        ev.stopImmediatePropagation ()
        ev.stopPropagation ()

        let ev =
            Haunted.createEvent (
                "on-fs-tab-close",
                {| bubbles = true
                   cancelable = true
                   composed = true |}
            )

        host |> dispatchEvent ev

    let onSelected _ =
        let ev =
            Haunted.createEvent (
                "on-fs-tab-selected",
                {| bubbles = true
                   cancelable = true
                   composed = true |}
            )

        host |> dispatchEvent ev

    let closableTpl () =
        match host.isClosable with
        | Some true -> html $"<button class='delete' @click={onClose}>&times;</button>"
        | _ -> Lit.nothing

    html
        $"""
        <section @click={onSelected}>
            <slot>
                <span>{host.label}</span>
            </slot>
            <slot name="icon">
                {closableTpl ()}
            </slot>
        </section>
        """


[<AllowNullLiteral>]
type FsTabHostElement =
    inherit HTMLElement

    abstract kind : Kind option with get, set

let private tabHostStyles = [ fsComponentsStyles; fsTabHostStyles ]

let private FsTabHost (host: FsTabHostElement) =
    ShadowStyles.adoptStyleSheets (host, tabHostStyles)

    Haunted.useEffect (
        (fun _ ->
            match host.kind with
            | Some _ -> ()
            | None -> host.kind <- Some Default),
        [||]
    )

    Haunted.useEffect (
        (fun _ ->
            match host.kind with
            | Some kind ->
                host.setAttribute ("kind", "")
                host.setAttribute ("kind", $"{kind}")
            | None -> ()

            ),
        [| host.kind |]
    )

    JS.console.log (host.kind)

    let onSelected (ev: Event) =
        ev.preventDefault ()
        ev.stopImmediatePropagation ()
        ev.stopPropagation ()

        let target = (ev.target :?> HTMLElement)
        let tabs = target.parentElement.children

        for i in 0 .. tabs.length - 1 do
            tabs.item(i).removeAttribute ("is-selected")

        target.setAttribute ("is-selected", "")

        let ev =
            Haunted.createCustomEvent (
                "on-fs-tab-selected",
                {| bubbles = true
                   cancelable = true
                   composed = true
                   detail = Some target |}
            )

        host |> dispatchEvent ev


    html
        $"""
        <article>
            <nav class={Lit.classes (getClass host.kind)}>
                <slot @on-fs-tab-selected={onSelected} name="tabs"></slot>
            </nav>
            <section class={Lit.classes (getClass host.kind)}>
                <slot></slot>
            </section>
        </article>
        """

[<Emit("FsTabHost")>]
let private FsTabHostRef: obj = jsNative

[<Emit("FsTabItem")>]
let private FsTabItemRef: obj = jsNative

let registerTabHost () =
    defineComponent "fs-tab-host" (Haunted.Component(FsTabHostRef, {| observedAttributes = [| "kind" |] |}))

let registerTabItem () =
    defineComponent
        "fs-tab-item"
        (Haunted.Component(
            FsTabItemRef,
            {| observedAttributes =
                   [| "is-selected"
                      "is-closable"
                      "kind"
                      "label"
                      "tab-name" |] |}
        ))
