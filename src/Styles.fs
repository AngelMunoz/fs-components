module Styles


open Fable.Core
open Browser.Types
open ShadowStyles.Types

[<ImportDefault("./fs-components.css")>]
let private fsStyles: string = jsNative

[<ImportDefault("./fs-message.css")>]
let private fsMessage: string = jsNative

[<ImportDefault("./fs-offcanvas.css")>]
let private fsOffcanvas: string = jsNative

[<ImportDefault("./fs-tab-item.css")>]
let private fsTabItem: string = jsNative


[<ImportDefault("./fs-tab-host.css")>]
let private fsTabHost: string = jsNative

let fsComponentsStyles = CSSStyleSheet.FromString(fsStyles)

let fsMessageStyles = CSSStyleSheet.FromString(fsMessage)

let fsOffcanvasStyles = CSSStyleSheet.FromString(fsOffcanvas)

let fsTabItemStyles = CSSStyleSheet.FromString(fsTabItem)

let fsTabHostStyles = CSSStyleSheet.FromString(fsTabHost)
