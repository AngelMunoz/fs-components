[<RequireQualifiedAccess>]
module Message

open Fable.Core
open Lit
open Haunted
open Browser.Types
open ShadowStyles
open ShadowStyles.Types
open Types

[<ImportDefault("./fs-components.css")>]
let private fsComponentsStyles: string = jsNative

[<ImportDefault("./fs-message.css")>]
let private fsMessageStyles: string = jsNative


let private styles =
    [ CSSStyleSheet.FromString(fsMessageStyles)
      CSSStyleSheet.FromString(fsComponentsStyles) ]

[<AllowNullLiteral>]
type FsMessageElement =
    inherit HTMLElement

    abstract kind : Kind option with get, set
    abstract header : string option with get, set
    abstract isOpen : bool option with get, set


let private Message (host: FsMessageElement) =

    Haunted.useEffect (
        (fun _ ->
            // Initialize properties you want to have a default value
            // attributes are okay because they are strings
            host.isOpen <- host.isOpen |> Option.orElse (true |> Some)
            host.kind <- host.kind |> Option.orElse (Kind.Default |> Some)),
        [||]
    )

    ShadowStyles.adoptStyleSheets (host, styles)

    let messageStyles =
        let kind = defaultArg host.kind Default

        seq {
            "message", true

            match kind with
            | Primary -> "is-primary", true
            | Link -> "is-link", true
            | Info -> "is-info", true
            | Success -> "is-success", true
            | Warning -> "is-warning", true
            | Danger -> "is-danger", true
            | Default -> "", false
        }

    let tryCloseMessage _ =
        // composed = true allows the event go through the shadow DOM
        let evt =
            Haunted.createEvent (
                "fs-close-message",
                {| composed = true
                   bubbles = true
                   cancelable = true |}
            )

        host |> dispatchEvent evt

    let header () =
        if Option.isSome host.header then
            html
                $"""
                <div part="message-header" class="message-header">
                    <p>{host.header}</p>
                    <slot name="close-button">
                        <button class="delete" aria-label="delete" @click={tryCloseMessage}>&times;</button>
                    </slot>
                </div>
                """
        else
            Lit.nothing

    // let's open our messages by default, check the index.html for more details
    if host.isOpen |> Option.defaultValue true then
        html
            $"""
            <article class={Lit.classes messageStyles}>
              {header ()}
              <div part="message-body" class="message-body">
                <slot></slot>
              </div>
            </article>
            """
    else
        Lit.nothing

// ensure we the reference to the function, otherwise fable will pass a curried function
// and not the function itself and that doesn't work
[<Emit("Message")>]
let private MessageRef: obj = jsNative


let register () =
    // for your own apps it's fine if you define the component automatically
    // but for libraries it's best to let the user decide if they want to use
    // the component or not.
    defineComponent
        "fs-message"
        (Haunted.Component(MessageRef, {| observedAttributes = [| "kind"; "header"; "isOpen" |] |}))
