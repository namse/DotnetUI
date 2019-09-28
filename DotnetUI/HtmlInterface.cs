using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetUI
{
    public interface IHtmlNode
    {
    }
    public interface IHtmlTextNode: IHtmlNode
    {
        string Text { get; set; }
    }
    public interface IHtmlElement: IHtmlNode
    {
        string Tag { get; }
        string Id { get; set; }
        void SetAttribute(string key, string value);
        List<IHtmlNode> ChildNodes { get; }
        List<IHtmlElement> ChildElements { get; }

        void AppendChild(IHtmlNode node);
    }
    public interface IHtmlDocument
    {
        IHtmlElement Body { get; }
        IHtmlElement GetElementById(string id);
        IHtmlElement CreateElement(string tag);
        IHtmlNode CreateTextNode(string text);
    }
}
