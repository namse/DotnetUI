
State를 Attribute로 할것이니?
NO. Provide "Commit State" function instead.


UserDefinedComponent
PlatformSpecificComponent

ComponentClass
Blueprint
ComponentClass Instance

Blueprinter
Renderer

DotnetUI.CreateBlueprint()
DotnetUI.Render(Blueprint)

CommitState => Update => Blueprinter => Renderer

칠드런은 누가 관리하냐? 처음으로 칠드런을 렌더시키는게 누구냐는 말임.
Host가 함. 잘 해놨음.

Everything is "Tree"
User provides Blueprint Tree
Renderer renders blueprint into their platform specification

Renderer mounts component instance
Renderer renders component instance

Component
    - UserDefinedComponent
    - PlatformSpecificComponent
Blueprint
Modeler
Renderer

CSX (CSharp XML)
Forest Core
Forest WASM
Forest Unity3D
Forest Winform

Tree
Node
Instance
Seed

Peel

- Children
  - No Children
  - Typed Children

  
```
<Button>
    <Text></Text>
</Button>
```

UpdateSeed

Seed


"Append, Insert"
Delete Detach, Remove

Component
Instance
Element
