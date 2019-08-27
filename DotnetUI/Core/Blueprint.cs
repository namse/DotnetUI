using System;

namespace DotnetUI.Core
{
    public class PlatformSpecificComponentBlueprint : Blueprint
    {
        public PlatformSpecificComponentBlueprint(string ComponentName, object props) : base(null, props)
        {
        }
    }
    public partial class Blueprint
    {
        public readonly Type ComponentType;
        public readonly object Props;

        public Blueprint(Type componentType, object props)
        {
            ComponentType = componentType;
            Props = props;
        }
    }
}
