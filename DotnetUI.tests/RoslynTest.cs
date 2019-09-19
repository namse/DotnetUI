using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    public class CsxRewriter : CSharpSyntaxRewriter
    {
        public CsxRewriter() : base(true) { }

        private SyntaxNode ConvertCsxElementSyntaxToExpression(
            IdentifierNameSyntax tagName,
            SyntaxList<CsxStringAttributeSyntax> attributes)
        {
            var propsString = "";
            if (attributes != default)
            {
                propsString = string.Join(',', attributes
                    .Select(attribute => $"{attribute.Key}={attribute.Value}"));
            }

            Console.WriteLine(propsString);

            return SyntaxFactory.ParseExpression($"Blueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{{propsString}}})");
        }

        public override SyntaxNode VisitCsxSelfClosingTagElement(CsxSelfClosingTagElementSyntax node)
        {
            return ConvertCsxElementSyntaxToExpression(node.TagName, node.Attributes);
        }

        public override SyntaxNode VisitCsxOpenCloseTagElement(CsxOpenCloseTagElementSyntax node)
        {
            return ConvertCsxElementSyntaxToExpression(node.CsxOpenTag.TagName, node.CsxOpenTag.Attributes);
        }
    }
    [TestClass]
    public class RoslynTest
    {
        public string GenerateCodeForExpression(string expression)
        {
            return $@"
class C
{{
    void Func() \{{
        var a = ({expression});
    }}
}}";
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElementGeneration()
        {
            var csxSelfClosingTagElement = SyntaxFactory.CsxSelfClosingTagElement("MyComponent");
            var csxCodeBlock = @"<MyComponent/>";
            Assert.AreEqual(csxSelfClosingTagElement.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElementGeneration()
        {
            var csxSelfClosingTagElement = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.CsxOpenTagElement("MyComponent"),
                SyntaxFactory.CsxCloseTagElement("MyComponent"));

            var csxCodeBlock = @"<MyComponent></MyComponent>";
            Assert.AreEqual(csxSelfClosingTagElement.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElement_NoAttributes()
        {
            var csxCodeBlock = @"<MyComponent/>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElement_NoAttributes()
        {
            var csxCodeBlock = @"<MyComponent></MyComponent>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElement_One_Attribute()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc""/>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc""})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElement_One_Attribute()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc""></MyComponent>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc""})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElement_Multiple_Attributes()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc"" ClassName=""clicked""/>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc"",ClassName=""clicked""})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElement_Multiple_Attributes()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc"" ClassName=""clicked""></MyComponent>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc"",ClassName=""clicked""})");

            Assert.AreEqual(expected, result.ToFullString());
        }
    }
}
