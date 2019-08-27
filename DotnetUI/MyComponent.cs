using System;

namespace DotnetUI
{
    public struct MyComponentProps : DefaultComponentProps
    {
        public string Style { get; set; }
        public ReactElement[] Children { get; set; }
    }

    public class MyComponentState : DefaultComponentState
    {
        public bool isClicked { get; set; }
    }
    public class MyComponent : ReactComponent<MyComponentProps, MyComponentState>
    {
        public MyComponent(
            MyComponentProps props
        ) : base(props) { }

        public void OnClick()
        {
            state.isClicked = true;
            CommitState();
        }
        public override ReactElement Render()
        {
            return React.CreateElement<DivComponent, DivComponentProps>(new DivComponentProps
            {
                Style = props.Style,
                Children = props.Children,
            });
        }
    }
}
