using System;

namespace DotnetUI
{
    public struct DivComponentProps : DefaultComponentProps
    {
        public ReactElement[] Children { get; set; }
        public string Style { get; set; }
    }
    public class DivComponent : ReactComponent<DivComponentProps>
    {
        public DivComponent(
            DivComponentProps props
        ): base(props) {
        }

        public override ReactElement Render()
        {
            throw new NotImplementedException();
        }
    }

    //public class DivElement : ReactElement
    //{
    //    public readonly new DivComponentProps Props;
    //    public readonly string Tag = "div";
    //    public DivElement(Type reactComponentType, object props) : base(reactComponentType, props)
    //    {
    //        Props = (DivComponentProps)props;
    //    }
    //}
}