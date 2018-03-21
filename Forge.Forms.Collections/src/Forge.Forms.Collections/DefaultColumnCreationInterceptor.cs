using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Forge.Forms.Annotations;
using Forge.Forms.Collections.Extensions;
using Forge.Forms.Collections.Interfaces;
using Humanizer;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Collections
{
    internal class DefaultColumnCreationInterceptor : IColumnCreationInterceptor
    {
        public DataGridColumn Intercept(IColumnCreationInterceptorContext context)
        {
            var path = context.Property.Name;

            if (context.Property.PropertyType.GetConstructor(Type.EmptyTypes) != null)
            {
                if (Resolve(context, ref path)) 
                    return null;
            }
            else if (context.Property.PropertyType.Namespace != null &&
                     !context.Property.PropertyType.Namespace.StartsWith("System"))
            {
                return null;
            }


            return new MaterialDataGridTextColumn
            {
                Header = context.Property.GetCustomAttribute<FieldAttribute>() is FieldAttribute fieldAttribute &&
                         !string.IsNullOrEmpty(fieldAttribute.Name)
                    ? fieldAttribute.Name
                    : context.Property.Name.Humanize(),
                Binding = context.Property.CreateBinding(path),
                EditingElementStyle =
                    context.Parent.TryFindResource("MaterialDesignDataGridTextColumnPopupEditingStyle") as Style,
                MaxLength = context.Property.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength ?? 0
            };
        }

        private static bool Resolve(IColumnCreationInterceptorContext context, ref string path)
        {
            var newItem = Activator.CreateInstance(context.Property.PropertyType);
            if (string.Equals(newItem.ToString(), context.Property.PropertyType.ToString(),
                StringComparison.OrdinalIgnoreCase))
            {
                if (context.Property.GetCustomAttribute<SelectFromAttribute>() is SelectFromAttribute
                        selectFromAttribute && !string.IsNullOrEmpty(selectFromAttribute.DisplayPath))
                {
                    path =
                        $"{context.Property.Name}.{context.Property.PropertyType.GetProperty(selectFromAttribute.DisplayPath)?.Name}";
                }
                else if (context.Property.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute
                             displayNameAttribute && !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                {
                    path =
                        $"{context.Property.Name}.{context.Property.PropertyType.GetProperty(displayNameAttribute.DisplayName)?.Name}";
                }
                else if (context.Property.PropertyType.GetCustomAttribute<DisplayNameAttribute>() is
                             DisplayNameAttribute
                             displayNameAttribute1 && !string.IsNullOrEmpty(displayNameAttribute1.DisplayName))
                {
                    path =
                        $"{context.Property.Name}.{context.Property.PropertyType.GetProperty(displayNameAttribute1.DisplayName)?.Name}";
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}