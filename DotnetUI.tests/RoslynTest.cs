using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    public class MyRewriter : CSharpSyntaxRewriter
    {
        public MyRewriter() : base(true) { }
        public override SyntaxNode VisitCsxSelfClosingTagElement(CsxSelfClosingTagElementSyntax node)
        {
            Console.WriteLine("VisitCsxSelfClosingTagElement");
            Console.WriteLine(node.ToFullString());
            return base.VisitCsxSelfClosingTagElement(node);
        }
    }
    [TestClass]
    public class RoslynTest
    {
        [TestMethod]
        public void TestRoslyn()
        {

            var div = SyntaxFactory.CsxSelfClosingTagElement("div");
            Console.WriteLine(div.ToFullString());

     //       var tree = CSharpSyntaxTree.ParseText(div.ToFullString());
            var tree = CSharpSyntaxTree.ParseText($@"
class C
{{
    void Func() \{{
        var a = ({div.ToFullString()});
    }}
}}");
            var root = (CompilationUnitSyntax) tree.GetRoot();
            var rewriter = new MyRewriter();
            var result = rewriter.Visit(root);

        }
    }
}
