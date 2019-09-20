using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class RoslynTest_Children
    {
        [TestMethod]
        public void TestOneSelfClosingChild()
        {
            var child = SyntaxFactory.CsxSelfClosingTagElement("Child");

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.IdentifierName("Parent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
                SyntaxFactory.List<CsxTagElementSyntax>(new[]
                {
                    child
                }),
                SyntaxFactory.CsxCloseTagElement("Parent"));

            var csxCodeBlock = @"<Parent><Child/></Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestOneOpenCloseChild()
        {
            var child = SyntaxFactory.CsxOpenCloseTagElement("Child",
                SyntaxFactory.CsxCloseTagElement("Child"));

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.IdentifierName("Parent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
                SyntaxFactory.List<CsxTagElementSyntax>(new[]
                {
                    child
                }),
                SyntaxFactory.CsxCloseTagElement("Parent"));

            var csxCodeBlock = @"<Parent><Child></Child></Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestNDepthFamily()
        {
            var child = SyntaxFactory.CsxOpenCloseTagElement("Child",
                SyntaxFactory.CsxCloseTagElement("Child"));
            var csxCodeBlock = @"<Child></Child>";
            for (var i = 0; i < 10; i += 1)
            {
                var parentName = $"Parent_{i}";
                var parent = SyntaxFactory.CsxOpenCloseTagElement(
                    SyntaxFactory.IdentifierName(parentName),
                    SyntaxFactory.List<CsxStringAttributeSyntax>(),
                    SyntaxFactory.List<CsxTagElementSyntax>(new[]
                    {
                        child
                    }),
                    SyntaxFactory.CsxCloseTagElement(parentName));

                csxCodeBlock = $"<{parentName}>{csxCodeBlock}</{parentName}>";
                Assert.AreEqual(parent.ToFullString(), csxCodeBlock);

                child = parent;
            }
        }


        [TestMethod]
        public void TestNDepthFamilyWithAttributes()
        {
            var csxCodeBlock = "";
            var compiledCode = "";
            for (var i = 0; i < 10; i += 1)
            {
                var tagName = $"Name_{i}";
                var id = i.ToString();
                csxCodeBlock = $@"<{tagName} Id=""{id}"">{csxCodeBlock}</{tagName}>";

                var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
                var root = (CompilationUnitSyntax)tree.GetRoot();
                var rewriter = new CsxRewriter();
                var result = rewriter.Visit(root);

                compiledCode = i == 0
                    ? $@"Blueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{Id=""{id}""}})"
                    : $@"Blueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{Id=""{id}"", Children=new []{{{compiledCode}}}}})";

                var expected = RoslynTestHelper.GenerateCodeForExpression(compiledCode);

                Assert.AreEqual(expected, result.ToFullString());
            }
        }
    }
}
