using System;

namespace Forge.Forms.Interfaces
{
    public interface IProxy
    {
        object Value { get; }

        Action ValueChanged { get; set; }
    }

    public interface IStringProxy
    {
        string Value { get; }
    }

    public interface IBoolProxy
    {
        bool Value { get; }
    }

    internal class PlainObject : IProxy
    {
        public PlainObject(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public Action ValueChanged { get; set; }
    }

    internal class PlainBool : IBoolProxy
    {
        public PlainBool(bool value)
        {
            Value = value;
        }

        public bool Value { get; }
    }

    internal class PlainString : IStringProxy
    {
        public PlainString(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
