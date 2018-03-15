using System.IO;
using System.Linq;

namespace Forge.Forms.LiveReloading.Extensions
{
    internal static class DirectoryExtensions
    {
        /// <summary>
        /// Finds the project root.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="maxUpwards">The maximum folders to search upwards.</param>
        /// <returns></returns>
        public static string FindProjectRoot(this string directoryPath, int maxUpwards = 6)
        {
            var directory = new DirectoryInfo(directoryPath);
            var count = 0;
            while (directory?.Parent != null && count < maxUpwards)
            {
                if (directory.GetFiles().Any(i => i.Extension == ".csproj"))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
                count++;
            }

            return "";
        }
    }
}