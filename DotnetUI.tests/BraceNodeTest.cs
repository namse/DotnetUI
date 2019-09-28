using System;
using System.Linq;
using DotnetUI.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class BraceNodeTest
    {
        [TestMethod]
        public void TestBraceNodeChild_string()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression("\"abc\"")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.IdentifierName("DivComponent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{""abc""}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{""abc""}})");

            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps { Children=new Blueprint[] { "abc"}});

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNode;

            var expected = @"<div>abc</div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }

        //[TestMethod]
        //public void TestBraceNodeChild_int()
        //{
        //    var rootBlueprint = Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
        //    {
        //        Children=new Blueprint[]
        //        {
        //            Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps {Text="abc"}),
        //        },
        //    });

        //    var document = new TestHtmlDocument();
        //    var renderer = new DomRenderer(document);
        //    var htmlElement = renderer.Mount(rootBlueprint).RootNode;

        //    var expected = @"<div>123</div>";
        //    Assert.AreEqual(htmlElement.ToString(), expected);
        //}

        //[TestMethod]
        //public void TestBraceNodeChild_float()
        //{
        //    var rootBlueprint = Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
        //    {
        //        Children=new Blueprint[]
        //        {
        //            Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps {Text="abc"}),
        //        },
        //    });

        //    var document = new TestHtmlDocument();
        //    var renderer = new DomRenderer(document);
        //    var htmlElement = renderer.Mount(rootBlueprint).RootNode;

        //    var expected = @"<div>123.456</div>";
        //    Assert.AreEqual(htmlElement.ToString(), expected);
        //}

        [TestMethod]
        public void TestBraceNodeChild_Component()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression("<DivComponent/>")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.IdentifierName("DivComponent"),
                SyntaxFactory.List<CsxStringAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{<DivComponent/>}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {})}})");
            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<Blueprint>(expectedCsCode);
            //var rootBlueprint = Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
            //    {Children=new Blueprint[] {Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps { })}});

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNode;

            var expected = @"<div><div></div></div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }

        //[TestMethod]
        //public void TestBraceNodeChild_IEnumerable_Component()
        //{
        //    var rootBlueprint = Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
        //    {
        //        Children=new Blueprint[]
        //        {
        //            Blueprint.From<TextComponent, TextComponentProps>(new TextComponentProps {Text="abc"}),
        //        },
        //    });

        //    var document = new TestHtmlDocument();
        //    var renderer = new DomRenderer(document);
        //    var htmlElement = renderer.Mount(rootBlueprint).RootNode;

        //    var expected = @"<div><div></div><div></div><div></div><div></div><div></div></div>";
        //    Assert.AreEqual(htmlElement.ToString(), expected);
        //}
    }
}
