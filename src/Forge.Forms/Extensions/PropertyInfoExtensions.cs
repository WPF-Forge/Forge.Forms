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
                if (property != (PropertyInfo) null)
                    return property;
            }

            return (PropertyInfo) null;
        }

        /// <summary>Gets the highest property value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static object GetHighestPropertyValue(this object obj, string property)
        {
            return obj.GetType().GetHighestProperty(property).GetValue(obj, (object[]) null);
        }

        /// <summary>Get all properties, keeping the token position.</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyWrapper> GetHighestProperties(this Type type)
        {
            return ((IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                .GroupBy<PropertyInfo, string>((Func<PropertyInfo, string>) (i => i.Name))
                .Select<IGrouping<string, PropertyInfo>, PropertyWrapper>(
                    (Func<IGrouping<string, PropertyInfo>, PropertyWrapper>) (i => new PropertyWrapper()
                    {
                        PropertyInfo = i.First<PropertyInfo>(),
                        Token = i.Last<PropertyInfo>().MetadataToken
                    }));
        }
    }
}