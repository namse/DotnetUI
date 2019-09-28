using System;

namespace DotnetUI.Core
{
    public partial class Blueprint
    {
        public readonly Type ComponentType;
        public readonly DefaultComponentProps Props;

        public Blueprint(Type componentType, DefaultComponentProps props)
        {
            ComponentType = componentType;
            Props = props;
        }

        public static implicit operator Blueprint(string text) =>
            From<TextComponent, TextComponentProps>(new TextComponentProps
            {
                Text = text,
            });
    }
}
