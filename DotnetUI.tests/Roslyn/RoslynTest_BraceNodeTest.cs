using DotnetUI.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class RoslynTest_BraceNodeTest
    {
        [TestMethod]
        public void TestBraceNodeChild_string()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression("\"abc\"")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("DivComponent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{""abc""}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{""abc""}})");

            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<ComponentBlueprint>(expectedCsCode);

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div>abc</div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }

        [TestMethod]
        public void TestBraceNodeChild_int32()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression("123")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("DivComponent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{123}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{123}})");

            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<ComponentBlueprint>(expectedCsCode);

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div>123</div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }



        [TestMethod]
        public void TestBraceNodeChild_float()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression("123.456")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("DivComponent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{123.456}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{123.456}})");

            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<ComponentBlueprint>(expectedCsCode);

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div>123.456</div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }

        [TestMethod]
        public void TestBraceNodeChild_Component()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression("<DivComponent/>")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("DivComponent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{<DivComponent/>}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {})}})");
            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<ComponentBlueprint>(expectedCsCode);

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div><div></div></div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }

        [TestMethod]
        public void TestBraceNodeChild_primitiveArray()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression(@"new object[] {""abc"", 123, ""qwer"", ""123""}")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("DivComponent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{new object[] {""abc"", 123, ""qwer"", ""123""}}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{new object[] {""abc"", 123, ""qwer"", ""123""}}})");

            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<ComponentBlueprint>(expectedCsCode);

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div><span>abc</span><span>123</span><span>qwer</span><span>123</span></div>";
            Assert.AreEqual(expected, htmlElement.ToString());
        }

        [TestMethod]
        public void TestBraceNodeChild_complexArray()
        {
            var children = SyntaxFactory.List(new CsxNodeSyntax[]
            {
                SyntaxFactory.CsxBraceNode(SyntaxFactory.ParseExpression(@"new object[] {""abc"", 123, ""qwer"", ""123"", <DivComponent/>}")),
            });

            var parent = SyntaxFactory.CsxOpenCloseTagElement(
                SyntaxFactory.Identifier("DivComponent"),
                SyntaxFactory.List<CsxAttributeSyntax>(),
                children,
                SyntaxFactory.CsxCloseTag("DivComponent"));

            var csxCodeBlock = @"<DivComponent>{new object[] {""abc"", 123, ""qwer"", ""123"", <DivComponent/>}}</DivComponent>";
            Assert.AreEqual(csxCodeBlock, parent.ToFullString());

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expectedCsCode = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {Children=new Blueprint[]{new object[] {""abc"", 123, ""qwer"", ""123"", ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps {})}}})");

            Assert.AreEqual(expectedCsCode, result.ToFullString());

            var rootBlueprint = RoslynTestHelper.GetGeneratedExpressionCodeReturnValue<ComponentBlueprint>(expectedCsCode);

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div><span>abc</span><span>123</span><span>qwer</span><span>123</span><div></div></div>";
            Assert.AreEqual(expected, htmlElement.ToString());
        }
    }
}
