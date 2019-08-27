using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // <div style="1">
            //   <div style="2"></div>
            // </div>

            var rootElement = React.CreateElement<DivComponent, DivComponentProps>(new DivComponentProps
            {
                Style = "1",
                Children = new[]
                {
                    React.CreateElement<DivComponent, DivComponentProps>(new DivComponentProps
                    {
                        Style = "2"
                    }),
                },
            });

            var html = ReactDom.ToHtml(rootElement);

            var expected = "<div style=\"1\"><div style=\"2\"></div></div>";
            Assert.AreEqual(html, expected);
        }

        [TestMethod]
        public void TestMyComponent()
        {
            // <MyComponent style="1">
            //   <MyComponent style="2"></MyComponent>
            // </MyComponent>

            // <div style="1">
            //   <div style="2"></div>
            // </div>

            var rootElement = React.CreateElement<MyComponent, MyComponentProps>(new MyComponentProps
            {
                Style = "1",
                Children = new[]
                {
                    React.CreateElement<MyComponent, MyComponentProps>(new MyComponentProps
                    {
                        Style = "2"
                    }),
                },
            });

            var html = ReactDom.ToHtml(rootElement);

            var expected = "<div style=\"1\"><div style=\"2\"></div></div>";
            Assert.AreEqual(html, expected);
        }
    }
}