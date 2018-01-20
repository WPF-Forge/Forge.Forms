using System;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class FileBinding : Resource
    {
        public FileBinding(string filePath, string valueConverter)
            : base(valueConverter)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public string FilePath { get; }

        public override bool IsDynamic => true;

        public override BindingBase ProvideBinding(IResourceContext context)
        {
            return new Binding(nameof(IProxy.Value))
            {
                Source = new FileWatcher(FilePath),
                Converter = GetValueConverter(context),
                Mode = BindingMode.OneWay
            };
        }

        public override bool Equals(Resource other)
        {
            if (other is FileBinding resource)
            {
                return FilePath == resource.FilePath
                       && ValueConverter == resource.ValueConverter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }
    }
}
