[<RequireQualifiedAccess>]
module Message

open Fable.Core
open Lit
open Haunted
open Browser.Types
open ShadowStyles
open Types
open Styles

[<AllowNullLiteral>]
type FsMessageElement =
    inherit HTMLElement

    abstract kind : Kind option with get, set
    abstract header : string option with get, set
    abstract isOpen : bool option with get, set

let private messageStyles kind =
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

let private tryCloseMessage (host: FsMessageElement) _ =
    // composed = true allows the event go through the shadow DOM
    let evt =
        Haunted.createEvent (
            "fs-close-message",
            {| composed = true
               bubbles = true
               cancelable = true |}
        )

    host |> dispatchEvent evt

let private styles = [ fsComponentsStyles; fsMessageStyles ]

let private Message (host: FsMessageElement) =
    ShadowStyles.adoptStyleSheets (host, styles)

    Haunted.useEffect (
        (fun _ ->
            // Initialize properties you want to have a default value
            // attributes are okay because they are strings
            host.isOpen <- host.isOpen |> Option.orElse (true |> Some)
            host.kind <- host.kind |> Option.orElse (Default |> Some)),
        [||]
    )

    let header () =
        if Option.isSome host.header then
            html
                $"""
                <div part="message-header" class="message-header">
                    <p>{host.header}</p>
                    <slot name="close-button">
                        <button class="delete" aria-label="delete" @click={(tryCloseMessage host)}>&times;</button>
                    </slot>
                </div>
                """
        else
            Lit.nothing

    html
        $"""
        <article class={Lit.classes (messageStyles (defaultArg host.kind Default))}>
          {header ()}
          <div part="message-body" class="message-body">
            <slot></slot>
          </div>
        </article>
        """

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
