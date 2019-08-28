using DotnetUI.Core;
using System;
using System.Linq;

namespace DotnetUI
{
    public class TestRenderer
    {

        public RenderNode Mount(Blueprint blueprint)
        {
            var componentInstance = Instanticate(blueprint);

            var node = new RenderNode(blueprint, componentInstance);

            if (componentInstance is IPlatformSpecificComponent)
            {
                if (!(blueprint.Props.Children is null))
                {
                    foreach (var child in blueprint.Props.Children)
                    {
                        var childNode = Mount(child);
                        node.Children.Add(childNode);
                    }
                }
            }
            else
            {
                // TODO : call componentInstance.ComponentDidMount()
                var nextBlueprint = componentInstance.Render();
                var next = Mount(nextBlueprint);
                node.Children.Add(next);
            }

            return node;
        }

        private static Component Instanticate(Blueprint blueprint)
        {
            return (Component)Activator.CreateInstance(blueprint.ComponentType, blueprint.Props);
        }

        private static bool IsPlatformSpecificComponent(Blueprint blueprint)
        {
            if (blueprint.ComponentType == typeof(DivComponent))
            {
                return true;
            }
            return false;
        }
    }
}