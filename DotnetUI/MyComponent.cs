using DotnetUI.Core;
using System;
using System.Collections.Generic;

namespace DotnetUI
{
    public struct MyComponentProps : DefaultComponentProps
    {
        public string Style { get; set; }
        public string Id { get; set; }
        public Blueprint[] Children { get; set; }
    }

    public class MyComponentState : DefaultComponentState
    {
        public bool IsClicked { get; set; }
    }
    public class MyComponent : Component<MyComponentProps, MyComponentState>
    {
        public static Dictionary<string, MyComponent> IdInstanceMap = new Dictionary<string, MyComponent>();
        public MyComponent(
            MyComponentProps props
        ) : base(props)
        {
            IdInstanceMap[props.Id] = this;
        }

        protected override MyComponentState State { get; set; } = new MyComponentState
        {
            IsClicked = false,
        };

        public static void Click(string id)
        {
            IdInstanceMap[id]?.OnClick();
        }

        public void OnClick()
        {
            State.IsClicked = true;
            CommitState();
        }
        public override Blueprint Render()
        {
            var style = State.IsClicked
                ? $"{Props.Style} clicked"
                : Props.Style;

            return Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
            {
                Style = style,
                Children = Props.Children,
            });
        }
    }
}
