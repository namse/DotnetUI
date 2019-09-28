# DotnetUI
.csx file (React in C#) => WebAssembly

Simply saying, it's React in C#. React use jsx. DotnetUI use csx. CSX file is CSharp file but we can use xml syntax like <MyComponent>.

# How to test
- Use visual studio 2019
- Build roslyn-csx first
  - roslyn-csx/build-csx.ps1
- Run sln and enjoy

# I'm Working In Process for Prototype
- [x] Implement Component Managing System like React
  - No Reconciliation for prototype! Just redraw everything on changes.
  - [x] Redraw on state changed
  - [x] Redraw on props changed
- [ ] Implement Csx Compiler (https://github.com/namse/roslyn-csx)
  - [x] Self Closing Tag Element `<tag />`
  - [x] Open Close Tag Element `<tag></tag>`
  - [x] Parse Children `<tag>{{here}}</tag>`
    - [x] Element Children `<tag><tag></tag></tag>`
    - [x] Text Children `<tag>"hi"</tag>`
    - [x] C# Expression Children `<tag>{$"hello {name}"}</tag>`
  - [ ] Attributes(Props)
    - [x] String Attributes `<tag att="hello" />`
    - [ ] C# Expression Attribute `<tag att={$"hello {name}"} />`
- [ ] Create compile tool from csx to wasm
  - [ ] .csx => .cs
  - [ ] .cs => .wasm (using mono)


# Big Picture
- Multiplatform
  - Core
    - Dotnet Standard
  - Renderer
    - Web (wasm)
    - Game (Unity)
    - Mobile (Xamarin)
    - Desktop (Electron, Avalonia, WPF or UWF)
- Multi Thread Support using Task(async, await)
