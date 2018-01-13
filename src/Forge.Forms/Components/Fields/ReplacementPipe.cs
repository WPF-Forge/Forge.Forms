using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Forge.Forms.Interfaces;
using Forge.Forms.Utils;

namespace Forge.Forms.Components.Fields
{
    public class ReplacementPipe
    {
        private readonly Func<string, CultureInfo, object> deserializer;
        private readonly List<RegexReplacement> replacements;

        public ReplacementPipe(Func<string, CultureInfo, object> deserializer)
            : this(deserializer, null)
        {
        }

        public ReplacementPipe(Func<string, CultureInfo, object> deserializer,
            IEnumerable<RegexReplacement> replacements)
        {
            this.deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            this.replacements = replacements?.ToList();
        }

        public Func<string, CultureInfo, object> CreateDeserializer(IResourceContext context)
        {
            if (replacements == null || replacements.Count == 0)
            {
                return deserializer;
            }

            var compiledReplacements = replacements.Select(r => r.Compile(context));
            return (value, culture) =>
            {
                foreach (var replacement in compiledReplacements)
                {
                    value = replacement.Replace(value);
                }

                return deserializer(value, culture);
            };
        }
    }

    public class RegexReplacement
    {
        public RegexReplacement(IValueProvider pattern, IValueProvider replacement, IValueProvider regexOptions)
        {
            Pattern = pattern;
            Replacement = replacement;
            RegexOptions = regexOptions;
        }

        public IValueProvider Pattern { get; }

        public IValueProvider Replacement { get; }

        public IValueProvider RegexOptions { get; }

        internal CompiledRegexReplacement Compile(IResourceContext context)
        {
            return new CompiledRegexReplacement(
                Pattern.GetStringValue(context),
                Replacement.GetStringValue(context),
                Replacement.GetValue(context));
        }
    }

    internal class CompiledRegexReplacement
    {
        private readonly StringProxy pattern;
        private readonly BindingProxy regexOptions;
        private readonly StringProxy replacement;

        public CompiledRegexReplacement(StringProxy pattern, StringProxy replacement, BindingProxy regexOptions)
        {
            this.pattern = pattern;
            this.replacement = replacement;
            this.regexOptions = regexOptions;
        }

        public string Replace(string value)
        {
            if (!(regexOptions.Value is RegexOptions options))
            {
                options = default(RegexOptions);
            }

            return Regex.Replace(value, pattern.Value, replacement.Value ?? "", options);
        }
    }
}
