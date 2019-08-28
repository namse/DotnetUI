using DotnetUI.Core;
using System.Collections.Generic;

namespace DotnetUI
{
    public class RenderNode
    {
        public List<RenderNode> Children = new List<RenderNode>();
        public readonly Blueprint Blueprint;
        public readonly Component Component;

        // TODO : HMM?
        public object Element { get; internal set; }
        public object RootElement => Element ?? Children[0]?.Element;

        public RenderNode(Blueprint blueprint, Component component)
        {
            Blueprint = blueprint;
            Component = component;
        }

    }
}