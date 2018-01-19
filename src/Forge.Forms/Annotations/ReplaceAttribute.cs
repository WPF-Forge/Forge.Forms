using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Allows specifying regex replacements before applying the field value.
    /// Multiple attributes can be defined for the same property,
    /// which causes the output of each replacement to be piped
    /// to the input of the next replacement.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ReplaceAttribute : Attribute
    {
        public ReplaceAttribute(string pattern, string replacement, [CallerLineNumber] int position = 0)
        {
            Pattern = pattern;
            Replacement = replacement;
            Position = position;
        }

        /// <summary>
        /// Pattern to replace on conversion.
        /// Accepts a bound expression.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Replacement string, which can contain regex replacement expressions.
        /// Accepts a bound expression.
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        /// Regex search options.
        /// Accepts a dynamic resource or <see cref="System.Text.RegularExpressions.RegexOptions" />.
        /// </summary>
        public object RegexOptions { get; set; }

        internal int Position { get; }

        internal RegexReplacement GetReplacement()
        {
            return new RegexReplacement(
                Utilities.GetStringResource(Pattern),
                Utilities.GetStringResource(Replacement),
                Utilities.GetResource<RegexOptions>(RegexOptions, default(RegexOptions),
                    Deserializers.Enum<RegexOptions>()));
        }
    }
}
