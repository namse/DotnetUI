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

        private SyntaxNode ConvertCsxNodeSyntaxToExpression(CsxNodeSyntax node)
        {
            switch (node)
            {
                case CsxTagElementSyntax csxTagElement:
                    return ConvertCsxTagElementSyntaxToExpression(csxTagElement);
                case CsxTextNodeSyntax csxTextNode:
                    return ConvertCsxTextNodeToExpression(csxTextNode);
                case CsxBraceNodeSyntax csxBraceNode:
                    return ConvertCsxBraceNodeToExpression(csxBraceNode);
                default:
                    throw new Exception($"Unknown csx node kind {node.Kind()}");
            }
        }

        private SyntaxNode ConvertCsxBraceNodeToExpression(CsxBraceNodeSyntax csxBraceNode)
        {
            var csxRewriter = new CsxRewriter();
            return csxRewriter.Visit(csxBraceNode.Expression);
            throw new Exception("STOP RIGHT THERE YOU ARE UNDER ARREST");
            // Primitive Value
            // - string
            // - number
            // - etc

            // Array of primitive value

            // Component

            // Array ofComponent

            // csxBraceNode.Expression
        }

        private SyntaxNode ConvertCsxTextNodeToExpression(CsxTextNodeSyntax csxTextNode)
        {
            return SyntaxFactory.ParseExpression($"Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps{{Text={csxTextNode.Text}}})");
        }

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
                var childrenCode = string.Join(",", openCloseTagElement.Children
                    .Select(ConvertCsxNodeSyntaxToExpression));
                var childrenString =
                    $"Children=new Blueprint[]{{{childrenCode}}}";

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