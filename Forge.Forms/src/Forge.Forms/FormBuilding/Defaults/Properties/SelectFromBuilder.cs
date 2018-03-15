using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;
using Humanizer;

namespace Forge.Forms.FormBuilding.Defaults.Properties
{
    internal class SelectFromBuilder : IFieldBuilder
    {
        public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
        {
            var selectFrom = property.GetCustomAttribute<SelectFromAttribute>();
            if (selectFrom == null)
            {
                return null;
            }

            var type = property.PropertyType;
            var field = new SelectionField(property.Name, property.PropertyType);
            if (selectFrom.DisplayPath != null)
            {
                field.DisplayPath = BoundExpression.ParseSimplified(selectFrom.DisplayPath);
            }

            if (selectFrom.ValuePath != null)
            {
                field.ValuePath = BoundExpression.ParseSimplified(selectFrom.ValuePath);
            }

            if (selectFrom.ItemStringFormat != null)
            {
                field.ItemStringFormat = BoundExpression.ParseSimplified(selectFrom.ItemStringFormat);
            }

            field.SelectionType = Utilities.GetResource<SelectionType>(selectFrom.SelectionType, SelectionType.ComboBox,
                Deserializers.Enum<SelectionType>());

            switch (selectFrom.ItemsSource)
            {
                case string expr:
                    var value = BoundExpression.Parse(expr);
                    if (!value.IsSingleResource)
                    {
                        throw new InvalidOperationException("ItemsSource must be a single resource reference.");
                    }

                    field.ItemsSource = value.Resources[0];
                    break;
                case IEnumerable<object> enumerable:
                    var objects = enumerable.ToList();
                    var elements = objects.Select(item =>
                    {
                        if (item is string expr)
                        {
                            return BoundExpression.ParseSimplified(expr);
                        }

                        return new LiteralValue(item);
                    }).ToList();

                    if (elements.All(item => item is LiteralValue))
                    {
                        field.ItemsSource = new LiteralValue(objects);
                    }
                    else
                    {
                        field.ItemsSource = new EnumerableStringValueProvider(elements);
                        field.DisplayPath = new LiteralValue(nameof(StringProxy.Value));
                        field.ValuePath = new LiteralValue(nameof(StringProxy.Key));
                    }

                    break;
                case Type enumType:
                    var addNull = false;
                    if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        enumType = Nullable.GetUnderlyingType(enumType);
                        addNull = true;
                    }

                    if (enumType != null && !enumType.IsEnum)
                    {
                        throw new InvalidOperationException("A type argument for ItemsSource must be an enum.");
                    }

                    var values = Enum.GetValues(enumType);
                    var collection = new List<KeyValuePair<ValueType, IValueProvider>>();
                    foreach (Enum enumValue in values)
                    {
                        var enumName = enumValue.ToString();
                        var memInfo = enumType.GetMember(enumName);
                        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDisplayAttribute), false);
                        IValueProvider name;
                        if (attributes.Length > 0)
                        {
                            var attr = (EnumDisplayAttribute)attributes[0];
                            name = BoundExpression.ParseSimplified(attr.Name);
                        }
                        else
                        {
                            name = new LiteralValue(enumName.Humanize());
                        }

                        collection.Add(new KeyValuePair<ValueType, IValueProvider>(enumValue, name));
                    }

                    field.ItemsSource = new EnumerableKeyValueProvider(collection, addNull);
                    field.DisplayPath = new LiteralValue(nameof(StringProxy.Value));
                    field.ValuePath = new LiteralValue(type == typeof(string)
                        ? nameof(StringProxy.Value)
                        : nameof(StringProxy.Key));
                    break;
            }

            return field;
        }
    }
}
