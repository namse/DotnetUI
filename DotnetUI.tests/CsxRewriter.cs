using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetUI.tests
{
    public class CsxRewriter : CSharpSyntaxRewriter
    {
        public CsxRewriter() : base(true) { }

        private SyntaxNode ConvertCsxTagElementSyntaxToExpression(CsxTagElementSyntax csxTagElement)
        {
            var tagName = csxTagElement.TagName;
            var attributes = csxTagElement.Attributes;

            var propsStringChunks = new List<string>();
            if (attributes != default)
            {
                var attributeString = string.Join(',', attributes
                    .Select(attribute => $"{attribute.Key}={attribute.Value}"));
                propsStringChunks.Add(attributeString);
            }

            if (csxTagElement is CsxOpenCloseTagElementSyntax openCloseTagElement
                && openCloseTagElement.Children.Count > 0)
            {
                // new [] { adsf, asdf, asdf}
                var childrenString =
                    $"Children=new []{{{string.Join(",", openCloseTagElement.Children.Select(ConvertCsxTagElementSyntaxToExpression))}}}";

                propsStringChunks.Add(childrenString);
            }

            var joinedPropsString = string.Join(", ", propsStringChunks);

            return SyntaxFactory.ParseExpression($"Blueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{{joinedPropsString}}})");
        }

        public override SyntaxNode VisitCsxSelfClosingTagElement(CsxSelfClosingTagElementSyntax node) =>
            ConvertCsxTagElementSyntaxToExpression(node);

        public override SyntaxNode VisitCsxOpenCloseTagElement(CsxOpenCloseTagElementSyntax node) =>
            ConvertCsxTagElementSyntaxToExpression(node);
    }
}