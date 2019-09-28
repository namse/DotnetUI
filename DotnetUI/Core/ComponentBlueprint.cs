using System;

namespace DotnetUI.Core
{
    public partial class ComponentBlueprint: Blueprint
    {
        public readonly Type ComponentType;
        public readonly IDefaultComponentProps Props;

        public ComponentBlueprint(Type componentType, IDefaultComponentProps props)
        {
            ComponentType = componentType;
            Props = props;
        }
    }
}
