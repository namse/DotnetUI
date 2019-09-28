using System;

namespace DotnetUI.Core
{
    public class ValueBlueprint : Blueprint
    {
        public readonly object Value;

        public ValueBlueprint(object value)
        {
            Value = value;
        }

        public static implicit operator ValueBlueprint(string value)
        {
            return new ValueBlueprint(value);
        }   
    }
}
