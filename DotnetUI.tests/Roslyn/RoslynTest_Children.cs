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
                SyntaxFactory.Identifier("Parent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                SyntaxFactory.List<CsxNodeSyntax>(new[]
                {
                    child
                }),
                SyntaxFactory.CsxCloseTag("Parent"));

            var csxCodeBlock = @"<Parent><Child/></Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestOneOpenCloseChild()
        {
            var child = SyntaxFactory.CsxOpenCloseTagElement("Child",
                SyntaxFactory.CsxCloseTag("Child"));

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("Parent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                SyntaxFactory.List<CsxNodeSyntax>(new[]
                {
                    child
                }),
                SyntaxFactory.CsxCloseTag("Parent"));

            var csxCodeBlock = @"<Parent><Child></Child></Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestNDepthFamily()
        {
            var child = SyntaxFactory.CsxOpenCloseTagElement("Child",
                SyntaxFactory.CsxCloseTag("Child"));
            var csxCodeBlock = @"<Child></Child>";
            for (var i = 0; i < 10; i += 1)
            {
                var parentName = $"Parent_{i}";
                var parent = SyntaxFactory.CsxOpenCloseTagElement(
                    SyntaxFactory.Identifier(parentName),
                    SyntaxFactory.List<CsxAttributeSyntax>(),
                    SyntaxFactory.List<CsxNodeSyntax>(new[]
                    {
                        child
                    }),
                    SyntaxFactory.CsxCloseTag(parentName));

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
                    ? $@"ComponentBlueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{Id=""{id}""}})"
                    : $@"ComponentBlueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{Id=""{id}"",Children=new Blueprint[]{{{compiledCode}}}}})";

                var expected = RoslynTestHelper.GenerateCodeForExpression(compiledCode);

                Assert.AreEqual(expected, result.ToFullString());
            }
        }

        [TestMethod]
        public void TestOneOpenCloseTagTextChild()
        {
            var child = SyntaxFactory.CsxTextNode(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.ParseToken("\"textChild\"")));

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("Parent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                SyntaxFactory.List<CsxNodeSyntax>(new[]
                {
                    child
                }),
                SyntaxFactory.CsxCloseTag("Parent"));

            var csxCodeBlock = @"<Parent>""textChild""</Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestParsingOneOpenCloseTagTextChild()
        {
            var csxCodeBlock = @"<Parent>""textChild""</Parent>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<Parent, ParentProps>(new ParentProps {Children=new Blueprint[]{""textChild""}})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestOpenCloseTagComplexChildren()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                RoslynTestHelper.GenerateCsxTextNode("abc"),
                SyntaxFactory.CsxOpenCloseTagElement("Child",
                    SyntaxFactory.CsxCloseTag("Child")),
                RoslynTestHelper.GenerateCsxTextNode("def"),
                RoslynTestHelper.GenerateCsxTextNode("123"),
                SyntaxFactory.CsxSelfClosingTagElement("Child"),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("Parent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("Parent"));

            var csxCodeBlock = @"<Parent>""abc""<Child></Child>""def""""123""<Child/></Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);


            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<Parent, ParentProps>(new ParentProps {Children=new Blueprint[]{""abc"",ComponentBlueprint.From<Child, ChildProps>(new ChildProps {}),""def"",""123"",ComponentBlueprint.From<Child, ChildProps>(new ChildProps {})}})");

            Assert.AreEqual(expected, result.ToFullString());
        }
    }
}
