using System;
using System.Linq;
using DotnetUI.Core;
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

            var rootBlueprint = Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
            {
                Style = "1",
                Children = new[]
                {
                    Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
                    {
                        Style = "2"
                    }),
                },
            });

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootElement;

            var expected = "<div style=\"1\"><div style=\"2\"></div></div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }

        [TestMethod]
        public void TestRedrawOnChangeState()
        {
            // <MyComponent style="1" id="first">
            //   <MyComponent style="2" id="second"></MyComponent>
            // </MyComponent>

            // <div style="1">
            //   <div style="2"></div>
            // </div>

            var rootBlueprint = Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps
            {
                Style = "1",
                Id = "first",
                Children = new[]
                {
                    Blueprint.From<MyComponent, MyComponentProps>(new MyComponentProps
                    {
                        Style = "2",
                        Id = "second",
                    }),
                },
            });


            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var renderNode = renderer.Mount(rootBlueprint);

            var expected = "<div style=\"1\"><div style=\"2\"></div></div>";
            Assert.AreEqual(renderNode.RootElement.ToString(), expected);

            MyComponent.Click("first");

            expected = "<div style=\"1 clicked\"><div style=\"2\"></div></div>";
            Assert.AreEqual(renderNode.RootElement.ToString(), expected);
        }
    }
}
