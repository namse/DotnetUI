using System;
using System.Collections.Generic;
using System.Linq;
using DotnetUI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetUI.tests
{

    [TestClass]
    public class PropsTest
    {
        private class PropsChangeTestComponentProps : DefaultComponentProps
        {
            public string Id { get; set; }
            public Blueprint[] Children { get; }
        }

        private class PropsChangeTestComponentState : DefaultComponentState
        {
            public int Length { get; set; }
        }

        private class PropsChangeTestComponent : Component<PropsChangeTestComponentProps, PropsChangeTestComponentState>
        {
            private static readonly Dictionary<string, PropsChangeTestComponent> ComponentIdMap = new Dictionary<string, PropsChangeTestComponent>();

            public PropsChangeTestComponent(PropsChangeTestComponentProps props) : base(props)
            {
                ComponentIdMap.Add(props.Id, this);
            }

            protected override PropsChangeTestComponentState State { get; set; } = new PropsChangeTestComponentState
            {
                Length = 0,
            };

            public static void IncreaseLength(string id)
            {
                var component = ComponentIdMap[id];

                component.State.Length += 1;

                component.CommitState();
            }

            public override Blueprint Render()
            {
                var children = new List<Blueprint>();

                for (var i = 0; i < this.State.Length; i += 1)
                {
                    children.Add(Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
                    {
                        Id = $"{Props.Id}-{i + 1}",
                    }));
                }

                return Blueprint.From<DivComponent, DivComponentProps>(new DivComponentProps
                {
                    Id = Props.Id,
                    Children = children.ToArray(),
                });
            }
        }

        [TestMethod]
        public void TestRedrawOnChangeProps()
        {
            // <PropsChangeTestComponent Id="1">
            // </PropsChangeTestComponent>

            const string id = "1";

            var rootBlueprint = Blueprint.From<PropsChangeTestComponent, PropsChangeTestComponentProps>(new PropsChangeTestComponentProps
            {
                Id = id,
            });

            var document = new TestHtmlDocument();
            var renderer = new DomRenderer(document);
            var renderNode = renderer.Mount(rootBlueprint);

            var expected = "<div id=\"1\"></div>";
            Assert.AreEqual(renderNode.RootNode.ToString(), expected);

            var childrenString = "";
            for (var i = 0; i < 10; i += 1)
            {
                PropsChangeTestComponent.IncreaseLength(id);

                childrenString += $"<div id=\"1-{i + 1}\"></div>";
                expected = $"<div id=\"1\">{childrenString}</div>";
                Assert.AreEqual(renderNode.RootNode.ToString(), expected);
            }
        }
    }
}
