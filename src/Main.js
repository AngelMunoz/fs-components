export { register as registerMessage } from './Message.fs.js';
export { register as registerOffCanvas } from './OffCanvas.fs.js';
export { TabItem, registerTabHost, registerTabItem } from './Tabs.fs.js';
export * from './Library.fs.js';

// use named exports when possible to let your users use tree-shake if they want.