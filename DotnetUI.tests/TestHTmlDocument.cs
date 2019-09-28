using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetUI.tests
{
    public abstract class TestHtmlNode: IHtmlNode
    {

    }

    public class TestHtmlTextNode : TestHtmlNode, IHtmlTextNode
    {
        public string Text { get; set; }

        public TestHtmlTextNode(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
    public class TestHtmlElement: TestHtmlNode, IHtmlElement
    {
        public string Tag { get; }
        public string Id { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public List<IHtmlNode> ChildNodes { get; } = new List<IHtmlNode>();

        public List<IHtmlElement> ChildElements { get; } = new List<IHtmlElement>();
        
        public TestHtmlElement(string tag)
        {
            Tag = tag;
        }

        public void AppendChild(IHtmlNode node)
        {
            ChildNodes.Add(node);
            if (node is IHtmlElement element)
            {
                ChildElements.Add(element);
            }
        }

        public override string ToString()
        {
            var attributeString = Attributes.Count == 0
                ? ""
                : string.Join("", Attributes
                    .Select(tuple => $" {tuple.Key}=\"{tuple.Value}\""));

            var contentString = ChildNodes.Count == 0
                ? ""
                : string.Join("", ChildNodes.Select(child => child.ToString()));

            return $"<{Tag}{attributeString}>{contentString}</{Tag}>";
        }

        public void SetAttribute(string key, string value)
        {
            Attributes[key] = value;
        }
    }

    public class TestHtmlDocument: IHtmlDocument
    {
        public IHtmlElement Body { get; } = new TestHtmlElement("body");

        public IHtmlElement FindElementById(string id)
        {
            var elements = new Queue<IHtmlElement>();
            elements.Enqueue(Body);

            while (elements.TryDequeue(out var element))
            {
                if (element.Id == id)
                {
                    return element;
                }
                element.ChildElements.ForEach(elements.Enqueue);
            }

            return null;
        }

        public IHtmlElement GetElementById(string id)
        {
            throw new NotImplementedException();
        }

        public IHtmlElement CreateElement(string tag)
        {
            return new TestHtmlElement(tag);
        }

        public IHtmlNode CreateTextNode(string text)
        {
            return new TestHtmlTextNode(text);
        }
    }
}
