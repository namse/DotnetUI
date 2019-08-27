using DotnetUI.Core;
using System;

namespace DotnetUI
{
    public struct MyComponentProps : DefaultComponentProps
    {
        public string Style { get; set; }
        public Blueprint[] Children { get; set; }
    }

    public class MyComponentState : DefaultComponentState
    {
        public bool isClicked { get; set; }
    }
    public class MyComponent : Component<MyComponentProps, MyComponentState>
    {
        public MyComponent(
            MyComponentProps props
        ) : base(props) { }

        public void OnClick()
        {
            state.isClicked = true;
            CommitState();
        }
        public override Blueprint Render()
        {
            return Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
            {
                Style = props.Style,
                Children = props.Children,
            });
        }
    }
}
