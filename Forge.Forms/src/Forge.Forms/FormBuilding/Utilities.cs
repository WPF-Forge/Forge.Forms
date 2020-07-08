using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.DynamicExpressions.BooleanExpressions;
using Forge.Forms.Validation;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.FormBuilding
{
    internal static partial class Utilities
    {
        /// <summary>
        ///     Get the last property from a type based on a name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyInfo GetHighestProperty(this Type type, string name)
        {
            for (; type != (Type)null; type = type.BaseType)
            {
                PropertyInfo property = type.GetProperty(name,
                    BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                if (property != null)
                    return property;
            }

            return null;
        }

        /// <summary>Gets the highest property value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static object GetHighestPropertyValue(this object obj, string property)
        {
            return obj.GetType().GetHighestProperty(property).GetValue(obj, null);
        }

        /// <summary>Get all properties, keeping the token position.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyWrapper> GetOutmostProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .GroupBy(i => i.Name)
                .Select(
                    i => new PropertyWrapper
                    {
                        PropertyInfo = i.First(),
                        Token = i.Last().MetadataToken
                    });
        }

        public static string TryReadFile(string filePath)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public static List<PropertyInfo> GetProperties(Type type, DefaultFields mode)
        {
            if (type == null)
            {
                throw new ArgumentException(nameof(type));
            }

            // First requirement is that properties and getters must be public.
            var properties = type
                .GetOutmostProperties()
                .Where(p => p.PropertyInfo.CanRead && p.PropertyInfo.GetGetMethod(true).IsPublic)
                .OrderBy(p => p.Token)
                .Select(i => i.PropertyInfo);

            switch (mode)
            {
                case DefaultFields.AllIncludingReadonly:
                    return properties
                        .Where(p => p.GetCustomAttribute<FieldIgnoreAttribute>() == null)
                        .ToList();
                case DefaultFields.AllExcludingReadonly:
                    return properties.Where(p =>
                    {
                        if (p.GetCustomAttribute<FieldIgnoreAttribute>() != null)
                        {
                            return false;
                        }

                        if (p.GetCustomAttribute<FieldAttribute>() != null)
                        {
                            return true;
                        }

                        return p.CanWrite && p.GetSetMethod(true).IsPublic;
                    }).ToList();
                case DefaultFields.None:
                    return properties.Where(p =>
                    {
                        if (p.GetCustomAttribute<FieldIgnoreAttribute>() != null)
                        {
                            return false;
                        }

                        return p.GetCustomAttribute<FieldAttribute>() != null;
                    }).ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid DefaultFields value.");
            }
        }

        public static IValueProvider GetResource<T>(object value, object defaultValue,
            Func<string, object> deserializer)
        {
            if (value == null)
            {
                return new LiteralValue(defaultValue);
            }

            if (value is string expression)
            {
                var boundExpression = BoundExpression.Parse(expression);
                switch (boundExpression.Resources.Count)
                {
                    case 0:
                        return new LiteralValue(
                            deserializer != null && boundExpression.StringFormat != null
                            ? deserializer(boundExpression.UnescapedStringFormat())
                            : boundExpression.UnescapedStringFormat());
                    case 1 when boundExpression.StringFormat == null:
                        return new CoercedValueProvider<T>(boundExpression.Resources[0], defaultValue);
                    default:
                        if (typeof(T) == typeof(bool))
                        {
                            var expr = boundExpression.UnescapedStringFormat();
                            string converter = null;
                            for (var i = expr.Length - 1; i >= 0; i--)
                            {
                                var c = expr[i];
                                if (c == '}')
                                {
                                    var next = i > 0 ? expr[i - 1] : '\0';
                                    if (next == '}')
                                    {
                                        i--;
                                        continue;
                                    }

                                    break;
                                }

                                if (c == '|')
                                {
                                    var next = i > 0 ? expr[i - 1] : '\0';
                                    if (next == '|')
                                    {
                                        // This will throw later anyway...
                                        break;
                                    }

                                    converter = expr.Substring(i + 1);
                                    expr = expr.Substring(0, i);
                                    break;
                                }

                            }

                            var ast = BooleanExpression.Parse(expr);
                            return new MultiBooleanBinding(ast, boundExpression.Resources, converter);
                        }

                        throw new ArgumentException(
                            $"The expression '{expression}' is not a valid resource because it does not define a single value source.",
                            nameof(value));
                }
            }

            if (value is T)
            {
                return new LiteralValue(value);
            }

            throw new ArgumentException(
                $"The provided value must be a bound resource or a literal value of type '{typeof(T).FullName}'.",
                nameof(value));
        }

        public static IValueProvider GetIconResource(object value)
        {
            if (value is -1 || value is string s && string.Equals(s, "empty", StringComparison.OrdinalIgnoreCase))
            {
                return new LiteralValue((PackIconKind)(-1));
            }

            return GetResource<PackIconKind>(value, (PackIconKind)(-2), Deserializers.Enum<PackIconKind>());
        }

        public static IValueProvider GetStringResource(string expression)
        {
            return expression == null ? new LiteralValue(null) : BoundExpression.ParseSimplified(expression);
        }

        public static BindingProxy GetValueProxy(IResourceContext context, string propertyKey)
        {
            var key = new BindingProxyKey(propertyKey);
            if (context.TryFindResource(key) is BindingProxy proxy)
            {
                return proxy;
            }

            proxy = new BindingProxy();
            context.AddResource(key, proxy);
            return proxy;
        }

        public static Func<IResourceContext, IProxy> GetValueProvider(string propertyKey)
        {
            BindingProxy ValueProvider(IResourceContext context)
            {
                return GetValueProxy(context, propertyKey);
            }

            return ValueProvider;
        }

        public static Func<IResourceContext, IErrorStringProvider> GetErrorProvider(string message, string propertyKey)
        {
            var func = GetValueProvider(propertyKey);
            var boundExpression = BoundExpression.Parse(message, new Dictionary<string, object>
            {
                ["Value"] = func
            });

            if (boundExpression.IsPlainString)
            {
                var errorMessage = boundExpression.StringFormat;
                return context => new PlainErrorStringProvider(errorMessage);
            }

            if (boundExpression.Resources.Any(
                res => res is DeferredProxyResource resource && resource.ProxyProvider == func))
            {
                var key = propertyKey;
                return context =>
                    new ValueErrorStringProvider(boundExpression.GetStringValue(context), GetValueProxy(context, key));
            }

            return context => new ErrorStringProvider(boundExpression.GetStringValue(context));
        }

        public static double[] GetGridWidths(string gridExpression)
        {
            if (string.IsNullOrEmpty(gridExpression))
            {
                return null;
            }

            var parts = gridExpression.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var grid = parts.Select(expr =>
            {
                expr = expr.Trim();
                var isPixel = false;
                if (expr.EndsWith("px", StringComparison.OrdinalIgnoreCase))
                {
                    isPixel = true;
                    expr = expr.Substring(0, expr.Length - 2);
                }
                else if (expr.EndsWith("*"))
                {
                    expr = expr.Substring(0, expr.Length - 1);
                }

                var value = double.Parse(expr);
                if (value <= 0d)
                {
                    throw new InvalidOperationException("Invalid grid column values.");
                }

                if (isPixel)
                {
                    value = -value;
                }

                return value;
            }).ToArray();

            return grid.Length != 0 ? grid : null;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var t in enumerable)
            {
                action(t);
            }
        }

        public static XAttribute GetSingleAttribute(this XElement element, string attribute)
        {
            return element.Attributes()
                .Single(xa =>
                    string.Equals(xa.Name.LocalName,
                        attribute,
                        StringComparison.OrdinalIgnoreCase));
        }

        public static XAttribute GetSingleOrDefaultAttribute(this XElement element, string attribute)
        {
            return element.Attributes()
                .SingleOrDefault(xa =>
                    string.Equals(xa.Name.LocalName,
                        attribute,
                        StringComparison.OrdinalIgnoreCase));
        }

        public static string TryGetAttribute(this XElement element, string attribute)
        {
            return GetSingleOrDefaultAttribute(element, attribute)?.Value;
        }

        public static string GetAttributeOrValue(this XElement element, string attribute)
        {
            var value = TryGetAttribute(element, attribute);
            if (value != null)
            {
                return value;
            }

            value = element.Value.Trim();
            return value == ""
                ? null
                : value;
        }

        public static FieldAttribute GetFieldAttributeFromElement(XElement element)
        {
            return new FieldAttribute
            {
                Name = element.TryGetAttribute("label"),
                DefaultValue = element.TryGetAttribute("defaultValue"),
                IsVisible = BoolOrText(element.TryGetAttribute("visible")),
                IsEnabled = BoolOrText(element.TryGetAttribute("enabled")),
                InitialFocus = BoolOrText(element.TryGetAttribute("initialFocus")),
                IsReadOnly = BoolOrText(element.TryGetAttribute("readonly")),
                Icon = element.TryGetAttribute("icon"),
                ToolTip = element.TryGetAttribute("tooltip")
            };
        }

        public static BindingAttribute GetBindingAttributeFromElement(XElement element)
        {
            var attr = new BindingAttribute
            {
                StringFormat = element.TryGetAttribute("stringformat"),
                ConverterCulture = element.TryGetAttribute("conversionCulture"),
                ConversionErrorMessage = element.TryGetAttribute("conversionError")
            };

            var expr = element.TryGetAttribute("updateSourceTrigger");
            if (expr != null)
            {
                attr.UpdateSourceTrigger = (UpdateSourceTrigger)Enum.Parse(typeof(UpdateSourceTrigger), expr, true);
            }

            return attr;
        }

        public static List<SelectOption> GetSelectOptionsFromElement(XElement element)
        {
            var result = new List<SelectOption>();
            foreach (var child in element.Descendants("option"))
            {
                var value = child.TryGetAttribute("value");
                var name = child.GetAttributeOrValue("name") ?? value ?? "";
                value = value ?? name;
                result.Add(new SelectOption(name, value));
            }

            return result;
        }

        // Source: https://stackoverflow.com/questions/4692340/find-types-in-all-assemblies
        public static IEnumerable<Type> FindTypes(Func<Type, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    Type[] exportedTypes = null;
                    try
                    {
                        exportedTypes = assembly.GetExportedTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        exportedTypes = e.Types;
                    }

                    if (exportedTypes != null)
                    {
                        foreach (var type in exportedTypes)
                        {
                            if (predicate(type))
                                yield return type;
                        }
                    }
                }
            }
        }

        public static IEnumerable<ValueAttribute> GetValidatorsFromElement(XElement element)
        {
            var validators = new List<ValueAttribute>();
            foreach (var child in element.Descendants("validate"))
            {
                var type = child.TryGetAttribute("must");
                if (type == null)
                {
                    continue;
                }

                var condition = Parse(type);
                var argument = child.TryGetAttribute("value");
                var converter = child.TryGetAttribute("converter");

                ValueAttribute attr;
                if (converter != null)
                {
                    attr = argument != null
                        ? new ValueAttribute(converter, condition, argument)
                        : new ValueAttribute(converter, condition);
                }
                else
                {
                    attr = argument != null
                        ? new ValueAttribute(condition, argument)
                        : new ValueAttribute(condition);
                }

                attr.Message = child.GetAttributeOrValue("message");
                attr.When = child.TryGetAttribute("when");
                var expr = child.TryGetAttribute("strict");
                if (expr != null)
                {
                    attr.StrictValidation = bool.Parse(expr);
                }

                expr = child.TryGetAttribute("validatesOnTargetUpdated");
                if (expr != null)
                {
                    attr.ValidatesOnTargetUpdated = bool.Parse(expr);
                }

                expr = child.TryGetAttribute("onValueChanged");
                if (expr != null)
                {
                    attr.ArgumentUpdatedAction = (ValidationAction)Enum.Parse(typeof(ValidationAction), expr, true);
                }

                expr = child.TryGetAttribute("ignoreNullOrEmpty");
                if (expr != null)
                {
                    attr.IgnoreNullOrEmpty = bool.Parse(expr);
                }

                validators.Add(attr);
            }

            return validators;
        }

        public static T WithBaseProperties<T>
            (this T attribute, XElement element) where T : FormContentAttribute
        {
            attribute.IsVisible = element.TryGetAttribute("visible");
            return attribute;
        }

        public static T WithTextProperties<T>(this T attribute, XElement element) where T : TextElementAttribute
        {
            attribute.IconPadding = element.TryGetAttribute("iconPadding");
            return attribute;
        }

        private static object BoolOrText(string value)
        {
            if (value != null && bool.TryParse(value, out var result))
            {
                return result;
            }

            return value;
        }

        public static ActionAttribute GetAction(XElement element)
        {
            var action = new ActionAttribute(element.TryGetAttribute("name"), element.GetAttributeOrValue("content"))
            {
                IsDefault = BoolOrText(element.TryGetAttribute("isDefault")),
                IsCancel = BoolOrText(element.TryGetAttribute("isCancel")),
                Parameter = element.TryGetAttribute("parameter"),
                IsEnabled = BoolOrText(element.TryGetAttribute("enabled")),
                IsLoading = BoolOrText(element.TryGetAttribute("isLoading")),
                Icon = element.TryGetAttribute("icon"),
                Validates = BoolOrText(element.TryGetAttribute("validates")),
                ClosesDialog = BoolOrText(element.TryGetAttribute("closesDialog")),
                IsReset = BoolOrText(element.TryGetAttribute("resets") ?? element.TryGetAttribute("isReset")),
                IsPrimary = BoolOrText(element.TryGetAttribute("isPrimary"))
            };

            return action;
        }

        private static Must Parse(string value)
        {
            return (Must)Enum.Parse(typeof(Must), value, true);
        }

        public static double ParseDouble(string value)
        {
            return ParseDouble(value, 0d);
        }

        public static double? ParseNullableDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        public static double ParseDouble(string value, double defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        public static Thickness ParseThickness(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new Thickness();
            }

            var values = value.Split(' ', ',').Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToArray();
            switch (values.Length)
            {
                case 0:
                    return new Thickness();
                case 1:
                    return new Thickness(values[0]);
                case 2:
                    return new Thickness(values[0], values[1], values[0], values[1]);
                case 3:
                    return new Thickness(values[0], values[1], values[2], values[1]);
                default:
                    return new Thickness(values[0], values[1], values[2], values[3]);
            }
        }

        public static TEnum TryParse<TEnum>(string value, TEnum defaultValue) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(value, true, out var result) ? result : defaultValue;
        }
    }
}
