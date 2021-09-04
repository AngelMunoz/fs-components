const { defineConfig } = require('vite');
const path = require('path');

module.exports = defineConfig({
    build: {
        lib: {
            entry: path.resolve(__dirname, 'src/Main.js'),
            name: 'fs-components',
            fileName: (format) => `fs-components.${format}.js`,
            formats: ['es', 'umd'],
        },
        rollupOptions: {
            treeshake: true
        }
    }
});