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
                SyntaxFactory.IdentifierName("Parent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
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
                    SyntaxFactory.IdentifierName(parentName),
                    SyntaxFactory.List<CsxStringAttributeSyntax>(),
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
                    ? $@"Blueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{Id=""{id}""}})"
                    : $@"Blueprint.From<{tagName}, {tagName}Props>(new {tagName}Props {{Id=""{id}"", Children=new []{{{compiledCode}}}}})";

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
                SyntaxFactory.IdentifierName("Parent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
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

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"Blueprint.From<Parent, ParentProps>(new ParentProps {Children=new []{Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps{Text=""textChild""})}})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        private CsxTextNodeSyntax GenerateCsxTextNode(string text)
        {
            return SyntaxFactory.CsxTextNode(
                SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.ParseToken($"\"{text}\"")));
        }

        [TestMethod]
        public void TestOpenCloseTagComplexChildren()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                GenerateCsxTextNode("abc"),
                SyntaxFactory.CsxOpenCloseTagElement("Child",
                    SyntaxFactory.CsxCloseTag("Child")),
                GenerateCsxTextNode("def"),
                GenerateCsxTextNode("123"),
                SyntaxFactory.CsxSelfClosingTagElement("Child"),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.IdentifierName("Parent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("Parent"));

            var csxCodeBlock = @"<Parent>""abc""<Child></Child>""def""""123""<Child/></Parent>";
            Assert.AreEqual(parent.ToFullString(), csxCodeBlock);


            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"Blueprint.From<Parent, ParentProps>(new ParentProps {Children=new []{Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps{Text=""abc""}),Blueprint.From<Child, ChildProps>(new ChildProps {}),Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps{Text=""def""}),Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps{Text=""123""}),Blueprint.From<Child, ChildProps>(new ChildProps {})}})");

            Assert.AreEqual(expected, result.ToFullString());
        }
    }
}
