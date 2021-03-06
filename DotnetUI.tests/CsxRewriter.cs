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
            return node switch
            {
                CsxTagElementSyntax csxTagElement =>
                    ConvertCsxTagElementSyntaxToExpression(csxTagElement),
                CsxTextNodeSyntax csxTextNode =>
                    ConvertCsxTextNodeToExpression(csxTextNode),
                CsxBraceNodeSyntax csxBraceNode =>
                    ConvertCsxBraceNodeToExpression(csxBraceNode),
                _ => throw new Exception($"Unknown csx node kind {node.Kind()}"),
            };
        }

        private SyntaxNode ConvertCsxBraceNodeToExpression(CsxBraceNodeSyntax csxBraceNode)
        {
            var csxRewriter = new CsxRewriter();
            return csxRewriter.Visit(csxBraceNode.Expression);
        }

        private SyntaxNode ConvertCsxTextNodeToExpression(CsxTextNodeSyntax csxTextNode)
        {
            return SyntaxFactory.ParseExpression($"{csxTextNode.Text}");
        }

        private string ConvertAttributesToString(SyntaxList<CsxAttributeSyntax> attributes)
        {
            return string.Join(',', attributes
                .Select(attribute => attribute switch
                    {
                        CsxStringAttributeSyntax stringAttribute => $"{stringAttribute.Key}={stringAttribute.Value}",
                        CsxBraceAttributeSyntax braceAttribute => $"{braceAttribute.Key}={braceAttribute.Value}",
                        _ => throw new Exception("Unknown attribute syntax"),
                    }));
        }

        private SyntaxNode ConvertCsxTagElementSyntaxToExpression(CsxTagElementSyntax csxTagElement)
        {
            var tagName = csxTagElement.TagName;
            var attributes = csxTagElement.Attributes;

            var propsStringChunks = new List<string>();
            if (attributes != default)
            {
                var attributeString = ConvertAttributesToString(attributes);
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

            var joinedPropsString = string.Join(",", propsStringChunks);

            return SyntaxFactory.ParseExpression($"ComponentBlueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{{joinedPropsString}}})");
        }

        public override SyntaxNode VisitCsxSelfClosingTagElement(CsxSelfClosingTagElementSyntax node) =>
            ConvertCsxTagElementSyntaxToExpression(node);

        public override SyntaxNode VisitCsxOpenCloseTagElement(CsxOpenCloseTagElementSyntax node) =>
            ConvertCsxTagElementSyntaxToExpression(node);
    }
}