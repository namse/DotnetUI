using DotnetUI.Core;
using System.Collections.Generic;

namespace DotnetUI
{
    public class ValueRenderNode : RenderNode
    {
        private readonly object _value;

        public ValueRenderNode(ValueBlueprint valueBlueprint)
        {
            _value = valueBlueprint.Value;
        }

        public override Blueprint Render()
        {
            return new ValueBlueprint(_value);
        }
    }
}