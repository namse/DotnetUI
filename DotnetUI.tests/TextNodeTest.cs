using System;
using System.Linq;
using DotnetUI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{
    [TestClass]
    public class TextNodeTest
    {
        [TestMethod]
        public void TestTextNodeChild()
        {
            // <div>"abc"</div>

            var rootBlueprint = ComponentBlueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
            {
                Children = new Blueprint[]
                {
                    new ValueBlueprint("abc"),
                },
            });

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var htmlElement = renderer.Mount(rootBlueprint).RootNodes[0];

            var expected = @"<div>abc</div>";
            Assert.AreEqual(htmlElement.ToString(), expected);
        }
    }
}
