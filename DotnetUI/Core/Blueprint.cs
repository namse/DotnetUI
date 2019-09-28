using System;
using System.Collections;

namespace DotnetUI.Core
{
    public abstract class Blueprint
    {
        public static implicit operator Blueprint(string value) => new ValueBlueprint(value);

        public static implicit operator Blueprint(short value) => new ValueBlueprint(value);
        public static implicit operator Blueprint(int value) => new ValueBlueprint(value);
        public static implicit operator Blueprint(long value) => new ValueBlueprint(value);
        public static implicit operator Blueprint(float value) => new ValueBlueprint(value);
        public static implicit operator Blueprint(double value) => new ValueBlueprint(value);

        public static implicit operator Blueprint(bool value) => new ValueBlueprint(value);

        public static implicit operator Blueprint(object[] value) => new ValueBlueprint(value);
        public static implicit operator Blueprint(IEnumerable[] value) => new ValueBlueprint(value);
    }
}
