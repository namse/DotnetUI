using DotnetUI.Core;
using System;

namespace DotnetUI
{
    public struct DivComponentProps : IDefaultComponentProps
    {
        public string Id { get; set; }
        public Blueprint[] Children { get; set; }
        public string Style { get; set; }
    }
    public class DivComponent : PlatformSpecificComponent<DivComponentProps>
    {
        public DivComponent(
            DivComponentProps props
        ) : base(props)
        {
        }
    }
}