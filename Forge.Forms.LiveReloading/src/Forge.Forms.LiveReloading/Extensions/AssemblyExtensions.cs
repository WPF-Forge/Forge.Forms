using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Forge.Forms.LiveReloading.Extensions
{
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Determines whether [is assembly debug build].
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        ///   <c>true</c> if [is assembly debug build] [the specified assembly]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssemblyDebugBuild(this Assembly assembly)
        {
            return assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(da => da.IsJITTrackingEnabled);
        }
    }
}