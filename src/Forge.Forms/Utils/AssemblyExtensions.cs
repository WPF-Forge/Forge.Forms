using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forge.Forms.Utils
{
    /// <summary>
    ///     Assembly extensions
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Get all loadable types
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}