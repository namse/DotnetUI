using DotnetUI.Core;
using System.Collections.Generic;
using System.Linq;

namespace DotnetUI
{
    public abstract class RenderNode
    {
        public List<RenderNode> Children = new List<RenderNode>();

        public IHtmlNode Node { get; internal set; }
        public IHtmlNode[] RootNodes
        {
            get
            {
                return Node is null
                    ? Children.SelectMany(child => child.RootNodes).ToArray()
                    : new[] { Node };
            }
        }

        public abstract Blueprint Render();
    }
}