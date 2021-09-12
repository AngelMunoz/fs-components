# FS Components

This is a sample repo (or not? depends on the community 😁) of how can you create web-components with F#

## Overview

A quick overview of how everything is working together

1. Author Component.

    Check `src/Message.fs` there you will see how we add styles, initialize properties, handle classes, dispatch events and define a register function for our component

2. Export component.

    Inside `src/Main.js` you'll see we're using a javascript file to export everything from our component and from our `src/Library.fs` file this is to allow consumers to opt in for individual components rather than all of them.

3. Consume the component.

    Inside `index.html` you'll see that we are consuming our web component by calling `registerAll()` which was defined in `Library.fs` we do this mainly in development to just access all of the components, but ideally we should import only the ones we're going to use.


## Usage


Add this css somewhere in your website (style tag or css file is ok). You can customize these colors they are just to show how you can open styles for modification.
```css
:root {
    --primary-color: #00d1b2;
    --primary-color-light: #c0fff6;
    --primary-color-dark: #00927d;

    --link-color: #485fc7;
    --link-color-light: #dadff4;
    --link-color-dark: #3348a6;
    
    --info-color: #3e8ed0;
    --info-color-light: #d6eaf8;
    --info-color-dark: #2c5a9a;

    --success-color: #48c78e;
    --success-color-light: #dff4e6;
    --success-color-dark: #2c7a5a;

    --warning-color: #ffe08a;
    --warning-color-light: #fff8e1;
    --warning-color-dark: #c99a2c;

    --danger-color: #f14668;
    --danger-color-light: #fce6e6;
    --danger-color-dark: #a63a3a;

    --light-color: #f5f5f5;
    --dark-color: #363636;
    --white-color: #ffffff;
    --black-color: #000000;
}
```

### Register Components

Via **npm**

```sh
pnpm install fsharp-components #or npm install
```

```js
// for dev purposes use this will register
// everything in the package;
import { registerAll } from 'fsharp-components';
registerAll();

// to cherry pick do
import { registerMessage /* registerOffCanvas */ } from 'fsharp-components';
// registerOffCanvas();
registerMessage();
```


Via **CDN**


```html
<script type="module">
    //For dev purposes use
    // check skypack docs for prod URL's
    import { registerAll } from 'https://cdn.skypack.dev/fsharp-components';
    registerAll();
</script>
```

**Cherry Pick Components**

```html
<script type="module">
    // to cherry pick do
    // check skypack docs for prod URL's
    import { registerMessage /* registerOffCanvas */ } from 'https://cdn.skypack.dev/fsharp-components';
    // registerOffCanvas();
    registerMessage();
</script>
```

## Sample Components

> `kind` is an string enum:
> - primary
> - info
> - link
> - success
> - warning
> - danger


- fs-message
    - Attributes
        - kind : `kind`
        - header : string
        - is-open : bool
    - CSS Custom Properties
        - `--delete-color: var(--header-color);`
        - `--header-bg: var(--dark-color);`
        - `--header-color: var(--white-color);`
        - `--body-bg: #dbdbdb;`
        - `--body-color: var(--black-color);`
- fs-off-canvas
    - Attributes
        - is-open : bool
        - position : 'left' | 'right'
        - closable : bool
        - no-overlay : bool
        - kind: `kind`

    - CSS Custom Properties
        - `--fs-offcanvas-overlay-color: var(--dark-color);`
        - `--fs-offcanvas-overlay-opacity: 0.5;`
        - `--fs-offcanvas-overlay-z-index: 2;`
        - `--fs-offcanvas-z-index: 2;`
        - `--fs-offcanvas-width: 300px;`
        - `--fs-offcanvas-max-width: 320px;`
        - `--fs-offcanvas-background-color: var(--light-color);`
        - `--delete-color: var(--dark-color);`

- fs-tab-host
    - Attributes
        - kind : `kind`
    - CSS Custom Properties
        - `--fs-tab-host-tabs-border: var(--dark-color);`

- fs-tab-item
    - Attributes
        - is-selected : bool
        - is-closable : bool
        - kind : `kind`
        - label : string
        - tab-name : string

    - CSS Custom Properties
        - `--fs-tab-item-color: var(--dark-color);`
        - `--fs-tab-item-close-color: var(--dark-color);`
        - `--fs-tab-item-width: 20%;`


### Overview

A quick overview of how everything is working together

1. Author Component.

    Check `src/Message.fs` there you will see how we add styles, initialize properties, handle classes, dispatch events and define a register function for our component

2. Export component.

    Inside `src/Main.js` you'll see we're using a javascript file to export everything from our component and from our `src/Library.fs` file this is to allow consumers to opt in for individual components rather than all of them.

3. Consume the component.

    Inside `index.html` you'll see that we are consuming our web component by calling `registerAll()` which was defined in `Library.fs` we do this mainly in development to just access all of the components, but ideally we should import only the ones we're going to use.

