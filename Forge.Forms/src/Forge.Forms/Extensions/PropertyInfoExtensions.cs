using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forge.Forms.Extensions
{
    /// <summary>Property info extensions</summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        ///     Get the last property from a type based on a name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyInfo GetHighestProperty(this Type type, string name)
        {
            for (; type != (Type) null; type = type.BaseType)
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
    }
}