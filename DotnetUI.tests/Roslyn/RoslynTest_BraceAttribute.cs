using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class RoslynTest_BraceAttribute
    {
        [TestMethod]
        public void TestOpenCloseTagComplexChildren()
        {
            var csxCodeBlock = @"<Parent Str=""abc"" Attr={$""hello {value}""}>""abc""<Child src={source + 1}></Child></Parent>";

            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                RoslynTestHelper.GenerateCsxTextNode("abc"),
                SyntaxFactory.CsxOpenCloseTagElement(
                    SyntaxFactory.Identifier("Child"),
                    SyntaxFactory.List(new CsxAttributeSyntax[] {
                        SyntaxFactory.CsxBraceAttribute("src", "source + 1"),
                    }),
                    SyntaxFactory.List<CsxNodeSyntax>(),
                    SyntaxFactory.CsxCloseTag("Child")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("Parent"),
                SyntaxFactory.List(new CsxAttributeSyntax[] {
                    SyntaxFactory.CsxStringAttribute("Str", "abc"),
                    SyntaxFactory.CsxBraceAttribute("Attr", @"$""hello {value}"""),
                }),
                children,
                SyntaxFactory.CsxCloseTag("Parent"));

            Assert.AreEqual(csxCodeBlock, parent.ToFullString());


            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<Parent, ParentProps>(new ParentProps {Str=""abc"",Attr=$""hello {value}"",Children=new Blueprint[]{""abc"",ComponentBlueprint.From<Child, ChildProps>(new ChildProps {src=source + 1})}})");

            Assert.AreEqual(expected, result.ToFullString());
        }
    }
}
