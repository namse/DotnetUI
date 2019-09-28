using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class RoslynTest
    {
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
                "MyComponent",
                SyntaxFactory.CsxCloseTag("MyComponent"));

            var csxCodeBlock = @"<MyComponent></MyComponent>";
            Assert.AreEqual(csxSelfClosingTagElement.ToFullString(), csxCodeBlock);
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElement_NoAttributes()
        {
            var csxCodeBlock = @"<MyComponent/>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElement_NoAttributes()
        {
            var csxCodeBlock = @"<MyComponent></MyComponent>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElement_One_Attribute()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc""/>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc""})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElement_One_Attribute()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc""></MyComponent>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc""})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxSelfClosingTagElement_Multiple_Attributes()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc"" ClassName=""clicked""/>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc"",ClassName=""clicked""})");

            Assert.AreEqual(expected, result.ToFullString());
        }

        [TestMethod]
        public void TestCsxOpenCloseTagElement_Multiple_Attributes()
        {
            var csxCodeBlock = @"<MyComponent Id=""abc"" ClassName=""clicked""></MyComponent>";

            var tree = CSharpSyntaxTree.ParseText(RoslynTestHelper.GenerateCodeForExpression(csxCodeBlock));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new CsxRewriter();
            var result = rewriter.Visit(root);

            var expected = RoslynTestHelper.GenerateCodeForExpression(@"ComponentBlueprint.From<MyComponent, MyComponentProps>(new MyComponentProps {Id=""abc"",ClassName=""clicked""})");

            Assert.AreEqual(expected, result.ToFullString());
        }
    }
}
