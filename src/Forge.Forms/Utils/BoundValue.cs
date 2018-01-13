using System;
using System.Windows.Data;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Utils
{
    public sealed class BoundValue : Resource
    {
        public BoundValue(object source, string propertyPath, bool oneTimeBinding)
            : this(source, propertyPath, oneTimeBinding, null)
        {
        }

        public BoundValue(object source, string propertyPath, bool oneTimeBinding, string valueConverter)
            : base(valueConverter)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            PropertyPath = propertyPath;
            OneTimeBinding = oneTimeBinding;
        }

        public object Source { get; }

        public string PropertyPath { get; }

        public bool OneTimeBinding { get; }

        public override bool IsDynamic => !OneTimeBinding;

        public override bool Equals(Resource other)
        {
            if (other is BoundValue resource)
            {
                return ReferenceEquals(Source, resource.Source)
                       && PropertyPath == resource.PropertyPath
                       && OneTimeBinding == resource.OneTimeBinding
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            return new Binding(PropertyPath)
            {
                Source = Source,
                Converter = GetValueConverter(context),
                Mode = OneTimeBinding ? BindingMode.OneTime : BindingMode.OneWay
            };
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ (PropertyPath?.GetHashCode() ?? 0);
        }
    }
}
