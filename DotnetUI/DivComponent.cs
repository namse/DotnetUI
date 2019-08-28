using DotnetUI.Core;
using System;

namespace DotnetUI
{
    public struct DivComponentProps : DefaultComponentProps
    {
        public Blueprint[] Children { get; set; }
        public string Style { get; set; }
    }
    public class DivComponent : PlatformSpecificComponent<DivComponentProps>
    {
        public DivComponent(
            DivComponentProps props
        ): base(props) {
        }
    }

    //public class DivElement : Blueprint
    //{
    //    public readonly new DivComponentProps Props;
    //    public readonly string Tag = "div";
    //    public DivElement(Type ComponentType, object props) : base(ComponentType, props)
    //    {
    //        Props = (DivComponentProps)props;
    //    }
    //}
}