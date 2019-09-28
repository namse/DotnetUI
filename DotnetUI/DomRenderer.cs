using DotnetUI.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetUI
{
    public class DomRenderer: IUpdater
    {
        private readonly IHtmlDocument _document;
        private readonly Dictionary<Component, RenderNode> _componentRenderNodeMap = new Dictionary<Component, RenderNode>();
        private static Type[] _platformSpecificComponentTypes = {
            typeof(DivComponent),
            typeof(TextComponent),
        };

        public DomRenderer(IHtmlDocument document)
        {
            _document = document;
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
            _componentRenderNodeMap[componentInstance] = renderNode;

            if (blueprint.ComponentType == typeof(DivComponent))
            {
                return MountDivComponent(blueprint, renderNode);
            }
            if (blueprint.ComponentType == typeof(TextComponent))
            {
                return MountTextComponent(blueprint, renderNode);
            }
            throw new NotImplementedException();
        }

        private RenderNode MountDivComponent(Blueprint blueprint, RenderNode renderNode)
        {
            const string tag = "div";

            var element = _document.CreateElement(tag);

            var props = (DivComponentProps)blueprint.Props;

            if (props.Style != default)
            {
                element.SetAttribute("style", props.Style);
            }

            if (props.Id != default)
            {
                element.SetAttribute("id", props.Id);
            }

            if (!(props.Children is null))
            {
                foreach (var child in props.Children)
                {
                    var childNode = Mount(child);
                    renderNode.Children.Add(childNode);
                    element.AppendChild(childNode.RootNode);
                }
            }

            renderNode.Node = element;

            return renderNode;
        }

        private RenderNode MountTextComponent(Blueprint blueprint, RenderNode renderNode)
        {
            var props = (TextComponentProps)blueprint.Props;
            var textNode = _document.CreateTextNode(props.Text);

            renderNode.Node = textNode;

            return renderNode;
        }

        private RenderNode MountUserDefinedComponent(Blueprint blueprint)
        {
            var componentInstance = Instanticate(blueprint);

            var renderNode = new RenderNode(blueprint, componentInstance);
            _componentRenderNodeMap[componentInstance] = renderNode;

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
            return _platformSpecificComponentTypes.Contains(blueprint.ComponentType);
        }

        public void CommitUpdate(Component component)
        {
            var renderNode = _componentRenderNodeMap[component];

            var nextBlueprint = renderNode.Component.Render();

            renderNode.Children.Clear();

            var nextRenderNode = Mount(nextBlueprint);

            renderNode.Children.Add(nextRenderNode);
        }
    }
}