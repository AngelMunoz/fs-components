# FS Components

This is a sample repo (or not? depends on the community 😁) of how can you create web-components with F# visit

A quick overview of how everything is working together

1. Author Component.

    Check `src/Message.fs` there you will see how we add styles, initialize properties, handle classes, dispatch events and define a register function for our component

2. Export component.

    Inside `src/Main.js` you'll see we're using a javascript file to export everything from our component and from our `src/Library.fs` file this is to allow consumers to opt in for individual components rather than all of them.

3. Consume the component.

    Inside `index.html` you'll see that we are consuming our web component by calling `registerAll()` which was defined in `Library.fs` we do this mainly in development to just access all of the components, but ideally we should import only the ones we're going to use.

