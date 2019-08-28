using DotnetUI.Core;
using System;
using System.Collections.Generic;

namespace DotnetUI
{
    public class DomRenderer: IUpdater
    {
        private readonly IHtmlDocument Document;
        private readonly Dictionary<Component, RenderNode> ComponentRenderNodeMap = new Dictionary<Component, RenderNode>();

        public DomRenderer(IHtmlDocument document)
        {
            Document = document;
        }

        public RenderNode Mount(Blueprint blueprint)
        {
            return IsPlatformSpecificComponent(blueprint)
                ? MountPlatformSpecificComponent(blueprint)
                : MountUserDefinedComponent(blueprint);
        }

        private RenderNode MountPlatformSpecificComponent(Blueprint blueprint)
        {
            var componentInstance = Instanticate(blueprint);
            var renderNode = new RenderNode(blueprint, componentInstance);
            ComponentRenderNodeMap[componentInstance] = renderNode;

            if (blueprint.ComponentType == typeof(DivComponent))
            {
                return MountDivComponent(blueprint, renderNode);
            }
            throw new NotImplementedException();
        }

        private RenderNode MountDivComponent(Blueprint blueprint, RenderNode renderNode)
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
                    var childNode = Mount(child);
                    renderNode.Children.Add(childNode);
                    element.AppendChild((IHtmlElement)childNode.RootElement);
                }
            }

            renderNode.Element = element;

            return renderNode;
        }

        private RenderNode MountUserDefinedComponent(Blueprint blueprint)
        {
            var componentInstance = Instanticate(blueprint);

            var renderNode = new RenderNode(blueprint, componentInstance);
            ComponentRenderNodeMap[componentInstance] = renderNode;

            // TODO : call componentInstance.ComponentDidMount()
            var nextBlueprint = componentInstance.Render();
            var nextRenderNode = Mount(nextBlueprint);

            renderNode.Children.Add(nextRenderNode);
            return renderNode;
        }
        private Component Instanticate(Blueprint blueprint)
        {
            var component = (Component) Activator
                .CreateInstance(blueprint.ComponentType, blueprint.Props);

            component.Updater = this;

            return component;
        }

        private static bool IsPlatformSpecificComponent(Blueprint blueprint)
        {
            if (blueprint.ComponentType == typeof(DivComponent))
            {
                return true;
            }
            return false;
        }

        public void CommitUpdate(Component component)
        {
            var renderNode = ComponentRenderNodeMap[component];

            var nextBlueprint = renderNode.Component.Render();

            renderNode.Children.Clear();

            var nextRenderNode = Mount(nextBlueprint);

            renderNode.Children.Add(nextRenderNode);
        }
    }
}