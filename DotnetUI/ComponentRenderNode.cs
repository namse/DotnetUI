using DotnetUI.Core;
using System.Collections.Generic;

namespace DotnetUI
{
    public class ComponentRenderNode : RenderNode
    {
        public readonly Component Component;

        public ComponentRenderNode(Component component)
        {
            Component = component;
        }

        public override Blueprint Render()
        {
            return Component.Render();
        }
    }
}