module Library

// as means of simplicity you can always have one module
// where you register everything which is very useful
// for development, once they are ready they can opt in/out from
// the unused modules via the main file
let registerAll () = Message.register ()
