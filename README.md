# DotnetUI
.csx file (React in C#) => WebAssemly

Simply saying, it's React in C#. React use jsx. DotnetUI use csx. CSX file is CSharp file but we can use xml syntax like <MyComponent>.

# I'm Workig In Process for Prototype
- [ ] Implement Component Managing System like React
  - No Reconcilation for prototype! Just redraw everything on changes.
- [ ] Implement Csx Compiler
  - [ ] Self Closing Tag Element (<tag />)
  - [ ] Attributes(Props)
    - [ ] String Attributes <tag att="hello" />
    - [ ] C# Expression Attribute <tag att={@"hello {name}"} />
- [ ] Create compile tool from csx to wasm
  - [ ] .csx => .cs
  - [ ] .csx => .wasm (using mono)
