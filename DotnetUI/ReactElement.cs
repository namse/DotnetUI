using System;

namespace DotnetUI
{
    public class ReactElement
    {
        public readonly Type ReactComponentType;
        public readonly object Props;

        public ReactElement(Type reactComponentType, object props)
        {
            ReactComponentType = reactComponentType;
            Props = props;
        }
    }
}
