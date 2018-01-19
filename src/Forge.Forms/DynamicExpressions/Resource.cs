using System;
using System.Collections.Generic;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions.ValueConverters;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public abstract class Resource : IEquatable<Resource>, IValueProvider
    {
        /// <summary>
        /// Global cache for value converters accessible from expressions.
        /// </summary>
        public static readonly Dictionary<string, IValueConverter> ValueConverters =
            new Dictionary<string, IValueConverter>(StringComparer.OrdinalIgnoreCase)
            {
                ["IsNull"] = new IsNullConverter(),
                ["IsNotNull"] = new IsNotNullConverter(),
                ["AsBool"] = new AsBoolConverter(),
                ["Negate"] = new NegateConverter(),
                ["IsEmpty"] = new IsEmptyConverter(),
                ["IsNotEmpty"] = new IsNotEmptyConverter(),
                ["ToUpper"] = new ToUpperConverter(),
                ["ToLower"] = new ToLowerConverter(),
                ["Length"] = new LengthValueConverter(),
                ["ToString"] = new ToStringConverter(),
                ["ToVisibility"] = new VisibilityConverter()
            };

        protected Resource(string valueConverter)
        {
            ValueConverter = valueConverter;
        }

        public string ValueConverter { get; }

        public abstract bool IsDynamic { get; }

        public abstract bool Equals(Resource other);

        public abstract BindingBase ProvideBinding(IResourceContext context);

        public virtual object ProvideValue(IResourceContext context)
        {
            return ProvideBinding(context);
        }

        protected IValueConverter GetValueConverter(IResourceContext context)
        {
            return GetValueConverter(context, ValueConverter);
        }

        protected internal static IValueConverter GetValueConverter(IResourceContext context, string valueConverter)
        {
            if (string.IsNullOrEmpty(valueConverter))
            {
                return null;
            }

            if (ValueConverters.TryGetValue(valueConverter, out var c))
            {
                return c;
            }

            if (context != null && context.TryFindResource(valueConverter) is IValueConverter converter)
            {
                return converter;
            }

            throw new InvalidOperationException($"Value converter '{valueConverter}' not found.");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Resource)obj);
        }

        public abstract override int GetHashCode();

        public static string FormatPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
            }

            if (path[0] == '[')
            {
                return path;
            }

            return "." + path;
        }
    }
}
