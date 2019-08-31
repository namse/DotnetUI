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
        public override SyntaxNode VisitCsxSelfClosingTagElement(CsxSelfClosingTagElementSyntax node)
        {
            Console.WriteLine("VisitCsxSelfClosingTagElement");
            Console.WriteLine(node.ToFullString());
            return SyntaxFactory.ParseExpression($"Blueprint.From<{node.TagName}, {node.TagName}Props>(new {node.TagName}Props {{}})");

            //// 
            //return base.VisitCsxSelfClosingTagElement(node);
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
        public void TestCsxSelfClosingTagElement_Attributes()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc""/>";

            var tree = CSharpSyntaxTree.ParseText(GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = GenerateCodeForExpression(@"Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {
    Id = ""abc"",
})");

            Assert.AreEqual(expected, result.ToFullString());
        }
    }
}
