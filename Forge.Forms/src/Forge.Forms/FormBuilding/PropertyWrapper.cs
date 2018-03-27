using System.Reflection;

namespace Forge.Forms.FormBuilding
{
    internal static partial class Utilities
    {
        /// <summary>PropertyInfo wrapper maintaining the token.</summary>
        internal class PropertyWrapper
        {
            /// <summary>The propertyInfo.</summary>
            public PropertyInfo PropertyInfo { get; set; }

            /// <summary>The original token.</summary>
            public int Token { get; set; }
        }
    }
}
