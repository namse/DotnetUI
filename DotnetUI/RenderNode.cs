using DotnetUI.Core;
using System.Collections.Generic;

namespace DotnetUI
{
    public class RenderNode
    {
        public List<RenderNode> Children = new List<RenderNode>();
        public readonly Blueprint Blueprint;
        public readonly Component Component;

        public IHtmlNode Node { get; internal set; }
        public IHtmlNode RootNode => Node ?? Children[0]?.Node;

        public RenderNode(Blueprint blueprint, Component component)
        {
            Blueprint = blueprint;
            Component = component;
        }

    }
}