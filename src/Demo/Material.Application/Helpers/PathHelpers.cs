using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Material.Application.Helpers
{
    public static class PathHelpers
    {
        public static string GetUniqueFileName(string directory, string name, string extension)
        {
            var counter = 1;
            var candidate = Path.Combine(directory, name + extension);
            while (File.Exists(candidate))
            {
                counter++;
                candidate = Path.Combine(directory, name + $" ({counter})" + extension);
            }

            return candidate;
        }

        public static string GetUniqueDirectoryName(string directory, string name)
        {
            var counter = 1;
            var candidate = Path.Combine(directory, name);
            while (Directory.Exists(candidate))
            {
                counter++;
                candidate = Path.Combine(directory, name + $" ({counter})");
            }

            return candidate;
        }

        public static IEnumerable<string> GetFilesInDirectory(string directory, string extension)
        {
            return Directory.GetFiles(directory)
                .Where(file => string.Equals(Path.GetExtension(file), extension, StringComparison.Ordinal))
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public static bool IsValidFileName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
