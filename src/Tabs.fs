[<RequireQualifiedAccess>]
module Tabs


open Fable.Core
open Lit
open Haunted
open Browser.Types
open ShadowStyles
open Types
open Styles

[<AttachMembers>]
type TabItem =
    { id: string
      label: string
      closable: bool }

    static member FromObject
        (tabLike: {| id: string
                     label: string option
                     closable: bool option |})
        : TabItem =
        { id = tabLike.id
          label = tabLike.label |> Option.defaultValue tabLike.id
          closable = tabLike.closable |> Option.defaultValue false }

[<AllowNullLiteral>]
type FsTabItemElement =
    inherit HTMLElement

    abstract member selected : bool option with get, set
    abstract member label : string option with get, set
    abstract member closable : bool option with get, set
    abstract member tabItem : TabItem option with get, set


let private initializeTabItem (host: FsTabItemElement) _ =
    match host.tabItem with
    | Some _ -> ()
    | None ->
        host.tabItem <-
            {| id = host.id
               label = host.label
               closable = host.closable |}
            |> TabItem.FromObject
            |> Some

let private tabItemStyles = [ fsComponentsStyles; fsTabItemStyles ]

let private FsTabItem (host: FsTabItemElement) =
    ShadowStyles.adoptStyleSheets (host, tabItemStyles)
    Haunted.useEffect ((initializeTabItem host), [||])


    let onClose (host: FsTabItemElement) _ =
        let ev =
            Haunted.createEvent (
                "on-fs-tab-close",
                {| composed = true
                   bubbles = true
                   cancelable = true |}
            )

        host |> dispatchEvent ev

    let onSelected (host: FsTabItemElement) _ =
        let ev =
            Haunted.createEvent (
                "on-fs-tab-selected",
                {| composed = true
                   bubbles = true
                   cancelable = true |}
            )

        host |> dispatchEvent ev

    let labelTpl () =
        match host.label with
        | Some label -> html $"<span>{label}</span>"
        | None ->
            match host.tabItem with
            | Some tabItem -> html $"<span>{tabItem.label}</span>"
            | None -> html $"<span>{host.id}</span>"

    let closeTpl () =
        match host.closable with
        | Some true -> html $"<button class='close' @click={onClose host}>&times;</button>"
        | Some false -> Lit.nothing
        | None ->
            match host.tabItem with
            | Some tabItem ->
                match tabItem.closable with
                | true -> html $"<button class='close' @click={onClose host}>&times;</button>"
                | false -> Lit.nothing
            | None -> Lit.nothing

    html
        $"""
        <section @click={onSelected host}>
            {closeTpl ()}
            {labelTpl ()}
        </section>
        """


[<AllowNullLiteral>]
type FsTabHostElement =
    inherit HTMLElement

    abstract member tabs : TabItem array option with get, set
    abstract member selectedTab : TabItem option with get, set

let private initializeTabHost (host: FsTabHostElement) _ =
    match host.tabs with
    | Some _ -> ()
    | None -> host.tabs <- Some Array.empty<TabItem>

let private tabHostStyles = [ fsComponentsStyles; fsTabHostStyles ]

let private FsTabHost (host: FsTabHostElement) =
    ShadowStyles.adoptStyleSheets (host, tabHostStyles)
    Haunted.useEffect ((initializeTabHost host), [||])

    let onTabClose (ev: Event) =
        match host.tabs with
        | Some tabs ->
            let tab = (ev.target :?> FsTabItemElement).tabItem

            match tab with
            | None -> ()
            | Some tab ->
                host.tabs <-
                    tabs
                    |> Array.filter (fun t -> t.id <> tab.id)
                    |> Some

                ev.preventDefault ()
                ev.stopPropagation ()
                ev.stopImmediatePropagation ()
        | None -> ()

    let setSelectedTab (host: FsTabHostElement) (ev: Event) =
        let target = (ev.target :?> FsTabItemElement)

        host.selectedTab <-
            TabItem.FromObject(target.tabItem |> unbox)
            |> Some

    let tabsTpl (tabs: TabItem array) =
        seq {
            for tab in tabs do
                let isSelected =
                    match host.selectedTab with
                    | None -> false
                    | Some selected -> selected.id = tab.id

                html
                    $"""
                    <fs-tab-item selected={isSelected} .tabItem={tab} @on-fs-tab-selected={setSelectedTab host} @on-fs-tab-closed={onTabClose}></fs-tab-item>
                    """
        }

    html
        $"""
        <article>
            <nav>
                <slot name="tabs">
                    {tabsTpl (host.tabs |> Option.defaultValue [||])}
                </slot>
            </nav>
            <slot name="tab-content"></slot>
        </article>
        """

[<Emit("FsTabHost")>]
let private FsTabHostRef: obj = jsNative

[<Emit("FsTabItem")>]
let private FsTabItemRef: obj = jsNative

let registerTabHost () =
    defineComponent "fs-tab-host" (Haunted.Component(FsTabHostRef))

let registerTabItem () =
    defineComponent
        "fs-tab-item"
        (Haunted.Component(
            FsTabItemRef,
            {| observedAttributes =
                   [| "selected"
                      "closable"
                      "id"
                      "label" |] |}
        ))
