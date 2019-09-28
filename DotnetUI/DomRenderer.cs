using DotnetUI.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DotnetUI
{
    public class DomRenderer: IUpdater
    {
        private readonly IHtmlDocument _document;
        private readonly Dictionary<Component, RenderNode> _componentRenderNodeMap = new Dictionary<Component, RenderNode>();
        private static readonly Type[] PlatformSpecificComponentTypes = {
            typeof(DivComponent),
        };

        public DomRenderer(IHtmlDocument document)
        {
            _document = document;
        }

        public RenderNode Mount(Blueprint blueprint)
        {
            return blueprint switch
            {
                ComponentBlueprint componentBlueprint =>
                    IsPlatformSpecificComponent(componentBlueprint)
                        ? MountPlatformSpecificComponent(componentBlueprint)
                        : MountUserDefinedComponent(componentBlueprint),
                ValueBlueprint valueBlueprint =>
                    MountValueBlueprint(valueBlueprint),
                _ =>
                    throw new Exception($"Unknown Blueprint Type {blueprint.GetType()}"),
            };
        }

        private RenderNode WrapWithSpan(RenderNode renderNode)
        {
            const string tag = "span";

            var element = _document.CreateElement(tag);

            element.ChildNodes.Add(renderNode.Node);
            
            renderNode.Node = element;

            return renderNode;
        }

        private RenderNode MountValueBlueprint(ValueBlueprint valueBlueprint)
        {
            var renderNode = new ValueRenderNode(valueBlueprint);

            if (!(valueBlueprint.Value is string)
                && valueBlueprint.Value is IEnumerable enumerable)
            {
                foreach (var child in enumerable)
                {
                    var childBlueprint = child is ComponentBlueprint componentBlueprint
                        ? (Blueprint) componentBlueprint
                        : new ValueBlueprint(child);

                    var childNode = Mount(childBlueprint);

                    if (childNode.Node is IHtmlTextNode)
                    {
                        childNode = WrapWithSpan(childNode);
                    }

                    renderNode.Children.Add(childNode);
                }

                return renderNode;
            }

            var element = _document.CreateTextNode(valueBlueprint.Value.ToString());
            renderNode.Node = element;

            return renderNode;
        }

        private RenderNode MountPlatformSpecificComponent(ComponentBlueprint componentBlueprint)
        {
            var componentInstance = Instanticate(componentBlueprint);
            var renderNode = new ComponentRenderNode(componentInstance);
            _componentRenderNodeMap[componentInstance] = renderNode;

            if (componentBlueprint.ComponentType == typeof(DivComponent))
            {
                return MountDivComponent(componentBlueprint, renderNode);
            }
            throw new NotImplementedException();
        }

        private RenderNode MountDivComponent(ComponentBlueprint componentBlueprint, RenderNode renderNode)
        {
            const string tag = "div";

            var element = _document.CreateElement(tag);

            var props = (DivComponentProps)componentBlueprint.Props;

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

                    foreach (var childNodeRootNode in childNode.RootNodes)
                    {
                        element.AppendChild(childNodeRootNode);
                    }
                }
            }

            renderNode.Node = element;

            return renderNode;
        }

        private RenderNode MountUserDefinedComponent(ComponentBlueprint componentBlueprint)
        {
            var componentInstance = Instanticate(componentBlueprint);

            var renderNode = new ComponentRenderNode(componentInstance);
            _componentRenderNodeMap[componentInstance] = renderNode;

            // TODO : call componentInstance.ComponentDidMount()
            var nextBlueprint = componentInstance.Render();
            var nextRenderNode = Mount(nextBlueprint);

            renderNode.Children.Add(nextRenderNode);
            return renderNode;
        }
        private Component Instanticate(ComponentBlueprint componentBlueprint)
        {
            var component = (Component) Activator
                .CreateInstance(componentBlueprint.ComponentType, componentBlueprint.Props);

            component.Updater = this;

            return component;
        }

        private static bool IsPlatformSpecificComponent(ComponentBlueprint componentBlueprint)
        {
            return PlatformSpecificComponentTypes.Contains(componentBlueprint.ComponentType);
        }

        public void CommitUpdate(Component component)
        {
            var renderNode = _componentRenderNodeMap[component];

            var nextBlueprint = renderNode.Render();

            renderNode.Children.Clear();

            var nextRenderNode = Mount(nextBlueprint);

            renderNode.Children.Add(nextRenderNode);
        }
    }
}