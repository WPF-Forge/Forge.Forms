using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public class BoundExpression : IValueProvider
    {
        public BoundExpression(string value)
        {
            StringFormat = value ?? throw new ArgumentNullException(nameof(value));
            IsPlainString = true;
        }

        public BoundExpression(IValueProvider resource) : this(resource, null)
        {
        }

        public BoundExpression(IValueProvider resource, string stringFormat)
        {
            Resources = new List<IValueProvider>(1) { resource ?? throw new ArgumentNullException(nameof(resource)) };
            StringFormat = stringFormat;
        }

        public BoundExpression(IEnumerable<IValueProvider> resources, string stringFormat)
        {
            Resources = resources?.ToList() ?? throw new ArgumentNullException(nameof(resources));
            if (Resources.Count != 1)
            {
                StringFormat = stringFormat ?? throw new ArgumentNullException(nameof(stringFormat));
            }
            else
            {
                StringFormat = stringFormat;
            }

            if (Resources.Count == 0)
            {
                IsPlainString = true;
            }
        }

        public string StringFormat { get; }

        public IReadOnlyList<IValueProvider> Resources { get; }

        public bool IsPlainString { get; }

        public bool IsSingleResource => StringFormat == null && Resources != null && Resources.Count == 1;

        public BindingBase ProvideBinding(IResourceContext context)
        {
            if (Resources == null || Resources.Count == 0)
            {
                return new LiteralValue(StringFormat).ProvideBinding(context);
            }

            if (Resources.Count == 1)
            {
                var resource = Resources[0];
                var binding = resource.ProvideBinding(context);
                if (StringFormat != null)
                {
                    binding.StringFormat = StringFormat;
                }

                return binding;
            }

            var multiBinding = new MultiBinding
            {
                StringFormat = StringFormat
            };

            foreach (var binding in Resources.Select(resource => resource.ProvideBinding(context)))
            {
                multiBinding.Bindings.Add(binding);
            }

            return multiBinding;
        }

        public object ProvideValue(IResourceContext context)
        {
            if (Resources == null || Resources.Count == 0)
            {
                return UnescapedStringFormat();
            }

            return ProvideBinding(context);
        }

        internal IProxy GetProxy(IResourceContext context)
        {
            if (IsPlainString)
            {
                return new PlainObject(StringFormat);
            }

            if (IsSingleResource)
            {
                return Resources[0].GetValue(context);
            }

            if (StringFormat != null)
            {
                return this.GetStringValue(context);
            }

            return this.GetValue(context);
        }

        public IValueProvider GetValueProvider()
        {
            if (IsPlainString)
            {
                return new LiteralValue(StringFormat);
            }

            if (IsSingleResource)
            {
                return Resources[0];
            }

            return this;
        }

        public IValueProvider Simplified()
        {
            if (IsPlainString)
            {
                return new LiteralValue(UnescapedStringFormat());
            }

            if (IsSingleResource)
            {
                return Resources[0];
            }

            return this;
        }

        private string UnescapedStringFormat()
        {
            return StringFormat.Replace("{{", "{").Replace("}}", "}");
        }

        public static IValueProvider ParseSimplified(string expression)
        {
            return Parse(expression).Simplified();
        }

        public static BoundExpression Parse(string expression)
        {
            return Parse(expression, contextualResource: null);
        }

        public static BoundExpression Parse(string expression, IDictionary<string, object> contextualResources)
        {
            IValueProvider Factory(string name, bool oneTimeBinding, string converter)
            {
                if (!contextualResources.TryGetValue(name, out var value))
                {
                    var index = name.IndexOf('.');
                    var indexBracket = name.IndexOf('[');
                    var increment = 1;
                    if (index == -1 || indexBracket != -1 && indexBracket < index)
                    {
                        increment = 0;
                        index = indexBracket;
                    }

                    if (index == -1)
                    {
                        return null;
                    }

                    var source = name.Substring(0, index);
                    if (!contextualResources.TryGetValue(source, out value))
                    {
                        return null;
                    }

                    var path = name.Substring(index + increment);
                    switch (value)
                    {
                        case IProxy proxy:
                            return new ProxyResource(proxy, path, oneTimeBinding, converter);
                        case Func<IResourceContext, IProxy> lazyProxy:
                            return new DeferredProxyResource(lazyProxy, path, oneTimeBinding, converter);
                        case IValueProvider _:
                            throw new InvalidOperationException("Cannot use nested paths for a resource.");
                        default:
                            return new BoundValue(value, path, oneTimeBinding, converter);
                    }
                }

                switch (value)
                {
                    case IProxy proxy:
                        return new ProxyResource(proxy, null, oneTimeBinding, converter);
                    case Func<IResourceContext, IProxy> lazyProxy:
                        return new DeferredProxyResource(lazyProxy, null, oneTimeBinding, converter);
                    case IValueProvider valueProvider:
                        return valueProvider.Wrap(converter);
                    default:
                        return new LiteralValue(value, converter);
                }
            }

            return Parse(expression, Factory);
        }

        public static BoundExpression Parse(string expression,
            Func<string, bool, string, IValueProvider> contextualResource)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var i = 0;
            if (expression.StartsWith("\\"))
            {
                i = 1;
            }
            else if (expression.StartsWith("@"))
            {
                return new BoundExpression(expression.Substring(1));
            }

            var resources = new List<IValueProvider>();
            var stringFormat = new StringBuilder();
            var resourceType = new StringBuilder();
            var resourceName = new StringBuilder();
            var resourceConverter = new StringBuilder();
            var resourceFormat = new StringBuilder();
            var oneTimeBind = false;
            var length = expression.Length;
            char c;
            outside:
            if (i == length)
            {
                var format = stringFormat.ToString();
                return new BoundExpression(resources, format == "{0}" ? null : format);
            }

            c = expression[i];
            if (c == '{')
            {
                stringFormat.Append('{');
                if (++i == expression.Length)
                {
                    throw new FormatException("Invalid unescaped '{' at end of input.");
                }

                if (expression[i] == '{')
                {
                    i++;
                    stringFormat.Append('{');
                    goto outside;
                }

                goto readResource;
            }

            if (c == '}')
            {
                stringFormat.Append('}');
                if (++i == expression.Length)
                {
                    throw new FormatException("Invalid unescaped '}' at end of input.");
                }

                if (expression[i] == '}')
                {
                    i++;
                    stringFormat.Append('}');
                    goto outside;
                }

                throw new FormatException("Invalid unescaped '}'.");
            }

            stringFormat.Append(c);
            i++;
            goto outside;

            readResource:
            // A leading ^ indicates one time binding for contextual resources.
            if (expression[i] == '^')
            {
                if (++i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }

                oneTimeBind = true;
            }

            // Resource type.
            while (char.IsLetter(c = expression[i]))
            {
                resourceType.Append(c);
                if (++i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }
            }

            // Skip whitespace.
            while (char.IsWhiteSpace(expression[i]))
            {
                if (++i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }
            }

            // Resource name.
            if (expression[i] == '\'')
            {
                i++;
                if (i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }

                while ((c = expression[i]) != '\'')
                {
                    resourceName.Append(c);
                    if (++i == length)
                    {
                        throw new FormatException("Unexpected end of input.");
                    }
                }

                i++;
                if (i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }
            }
            else
            {
                while ((c = expression[i]) != ',' && c != ':' && c != '|')
                {
                    if (c == '{')
                    {
                        if (++i == length)
                        {
                            throw new FormatException("Unexpected end of input.");
                        }

                        if (expression[i] != '{')
                        {
                            throw new FormatException("Invalid unescaped '{'.");
                        }
                    }
                    else if (c == '}')
                    {
                        if (++i == length)
                        {
                            goto addResource;
                        }

                        if (expression[i] != '}')
                        {
                            goto addResource;
                        }
                    }

                    resourceName.Append(c);
                    if (++i == length)
                    {
                        throw new FormatException("Unexpected end of input.");
                    }
                }
            }

            // Skip whitespace between name and converter/format/end.
            while (char.IsWhiteSpace(expression[i]))
            {
                if (++i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }
            }

            c = expression[i];
            if (c == '}')
            {
                // Resource can close at this point assuming no converter and no string format.
                if (++i == length)
                {
                    goto addResource;
                }

                if (expression[i] != '}')
                {
                    goto addResource;
                }

                throw new FormatException("Invalid '}}' sequence.");
            }

            // Value converter, read while character is letter.
            if (c == '|')
            {
                if (++i == length)
                {
                    throw new FormatException("Unexpected end of input.");
                }

                while (char.IsLetter(c = expression[i]))
                {
                    resourceConverter.Append(c);
                    if (++i == length)
                    {
                        throw new FormatException("Unexpected end of input.");
                    }
                }

                // Skip whitespace between converter to format/end.
                while (char.IsWhiteSpace(expression[i]))
                {
                    if (++i == length)
                    {
                        throw new FormatException("Unexpected end of input.");
                    }
                }

                if (c == '}')
                {
                    if (++i == length)
                    {
                        goto addResource;
                    }

                    if (expression[i] != '}')
                    {
                        goto addResource;
                    }

                    throw new FormatException("Converter name cannot contain braces.");
                }
            }

            // String format, read until single '}'.
            if (c == ',' || c == ':')
            {
                resourceFormat.Append(c);
                while (true)
                {
                    if (++i == length)
                    {
                        throw new FormatException("Unexpected end of input.");
                    }

                    c = expression[i];
                    if (c == '}')
                    {
                        if (++i == length)
                        {
                            goto addResource;
                        }

                        if (expression[i] != '}')
                        {
                            goto addResource;
                        }

                        resourceFormat.Append('}');
                    }

                    resourceFormat.Append(c);
                }
            }

            throw new FormatException($"Invalid character '{c}'");

            addResource:
            var key = resourceName.ToString();
            IValueProvider resource;
            var converter = resourceConverter.ToString();
            var resourceTypeString = resourceType.ToString();
            switch (resourceTypeString)
            {
                case "Binding":
                    resource = new PropertyBinding(key, false, converter);
                    break;
                case "Property":
                    resource = new PropertyBinding(key, true, converter);
                    break;
                case "StaticResource":
                    resource = new StaticResource(key, converter);
                    break;
                case "DynamicResource":
                    resource = new DynamicResource(key, converter);
                    break;
                case "ContextBinding":
                    resource = new ContextPropertyBinding(key, false, converter);
                    break;
                case "ContextProperty":
                    resource = new ContextPropertyBinding(key, true, converter);
                    break;
                case "FileBinding":
                    resource = new FileBinding(key, true, converter);
                    break;
                case "File":
                    resource = new FileBinding(key, false, converter);
                    break;
                default:
                    resource = contextualResource?.Invoke(resourceTypeString + key, oneTimeBind, converter);
                    if (resource != null)
                    {
                        break;
                    }

                    throw new FormatException("Invalid resource type.");
            }

            var index = resources.IndexOf(resource);
            if (index == -1)
            {
                index = resources.Count;
                resources.Add(resource);
            }

            stringFormat.Append(index);
            if (resourceFormat.Length != 0)
            {
                stringFormat.Append(resourceFormat);
            }

            stringFormat.Append('}');

            resourceType.Clear();
            resourceName.Clear();
            resourceFormat.Clear();
            resourceConverter.Clear();
            oneTimeBind = false;
            goto outside;
        }

        public static implicit operator BoundExpression(string expression)
        {
            return Parse(expression);
        }
    }
}
