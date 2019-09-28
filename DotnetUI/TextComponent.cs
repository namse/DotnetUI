using DotnetUI.Core;
using System;

namespace DotnetUI
{
    public struct TextComponentProps : DefaultComponentProps
    {
        public Blueprint[] Children { get; set; }
        public string Text { get; set; }
    }
    public class TextComponent : PlatformSpecificComponent<TextComponentProps>
    {
        public TextComponent(
            TextComponentProps props
        ) : base(props)
        {
        }
    }
}