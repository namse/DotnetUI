using DotnetUI.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetUI
{
    public class RenderTreeNode
    {
        public readonly Component ComponentInstance;
        public readonly Blueprint Blueprint;
        public RenderTreeNode Next;
    }
    public class DomRenderer
    {
        private static Dictionary<Type, string> ComponentTagDictionary = new Dictionary<Type, string>
        {
            [typeof(DivComponent)] = "div",
        };

        private readonly IHtmlDocument Document;

        public DomRenderer(IHtmlDocument document)
        {
            Document = document;
        }

        public IHtmlElement Mount(Blueprint blueprint)
        {
            return IsPlatformSpecificComponent(blueprint)
                ? MountPlatformSpecificComponent(blueprint)
                : MountUserDefinedComponent(blueprint);
        }

        private IHtmlElement MountPlatformSpecificComponent(Blueprint blueprint)
        {
            if (blueprint.ComponentType == typeof(DivComponent))
            {
                return MountDivComponent(blueprint);
            }
            throw new NotImplementedException();
        }

        private IHtmlElement MountDivComponent(Blueprint blueprint)
        {
            var tag = "div";

            var element = Document.CreateElement(tag);

            var props = (DivComponentProps)blueprint.Props;

            if (props.Style != default)
            {
                element.SetAttribute("style", props.Style);
            }

            if (!(props.Children is null))
            {
                foreach (var child in props.Children)
                {
                    var childElement = Mount(child);
                    element.AppendChild(childElement);
                }
            }
            
            return element;
        }

        private IHtmlElement MountUserDefinedComponent(Blueprint blueprint)
        {
            var componentInstance = Instanticate(blueprint);
            // TODO : call componentInstance.ComponentDidMount()
            var nextBlueprint = componentInstance.Render();
            return Mount(nextBlueprint);
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