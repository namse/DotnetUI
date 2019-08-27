using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetUI
{
    public static class ReactDom
    {
        public static string ToHtml(ReactElement element)
        {
            element = Resolve(element);

            if (element.ReactComponentType == typeof(DivComponent))
            {
                return ToHtml((DivComponentProps)element.Props);
            }
            throw new NotImplementedException();
        }
        public static string ToHtml(DivComponentProps props)
        {
            var childrenString = props.Children is null
                ? ""
                : string.Join("", props.Children.Select(ToHtml));

            var attributeTuples = new List<(string key, string value)>();

            if (props.Style != default)
            {
                attributeTuples.Add(("style", props.Style));
            }

            var attributeString = attributeTuples.Count == 0
                ? ""
                : string.Join("", attributeTuples.Select(prop =>
                {
                    return $" {prop.key}=\"{prop.value}\"";
                }));

            return $"<div{attributeString}>{childrenString}</div>";
        }

        public static ReactElement Resolve(ReactElement reactElement)
        {
            if (IsUnitComponent(reactElement))
            {
                return reactElement;
            }


            var instance = Instanticate(reactElement);
            var nextReactElement = instance.Render();

            return Resolve(nextReactElement);
        }

        private static ReactComponent Instanticate(ReactElement reactElement)
        {
            return (ReactComponent) Activator.CreateInstance(reactElement.ReactComponentType, reactElement.Props);
        }

        private static bool IsUnitComponent(ReactElement reactElement)
        {
            if (reactElement.ReactComponentType == typeof(DivComponent))
            {
                return true;
            }
            return false;
        }
    }
}