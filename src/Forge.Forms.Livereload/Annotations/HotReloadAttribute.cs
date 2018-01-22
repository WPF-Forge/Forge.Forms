using System;
using System.Runtime.CompilerServices;

namespace Forge.Forms.Livereload.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HotReloadAttribute : Attribute
    {
        public HotReloadAttribute(bool isPersistent = false, [CallerFilePath] string filePath = "")
        {
            IsPersistent = isPersistent;
            FilePath = filePath;
        }

        public string FilePath { get; }

        public bool IsPersistent { get; }
    }
}
