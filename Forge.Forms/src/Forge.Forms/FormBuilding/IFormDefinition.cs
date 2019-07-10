using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Forms.DynamicExpressions;

namespace Forge.Forms.FormBuilding
{
    /// <summary>
    /// Define a read-only interface so that we can still
    /// access it in read-only mode after it is built
    /// </summary>
    public interface IReadOnlyFormDefinition
    {
        IReadOnlyDictionary<string, IValueProvider> Resources { get; }

        IReadOnlyDictionary<string, string> Metadata { get; }

        IReadOnlyCollection<IFormProperty> FormProperties { get; }

        Type ModelType { get; }
    }
    public interface IFormDefinition: IReadOnlyFormDefinition
    {
        IReadOnlyList<FormRow> FormRows { get; }

        double[] Grid { get; }
        
        IDictionary<string, IValueProvider> Resources { get; }

        IDictionary<string, string> Metadata { get; }

        object CreateInstance(IResourceContext context);
    }

    public static class FormDefinitionExtensions
    {
        public static IEnumerable<FormElement> GetElements(this IFormDefinition definition)
        {
            return definition.FormRows.SelectMany(r => r.Elements).SelectMany(c => c.Elements);
        }

        public static void FreezeAll(this FormDefinition definition)
        {
            definition.Freeze();
            foreach (var element in definition.FormRows.SelectMany(r => r.Elements).SelectMany(c => c.Elements))
            {
                element.Freeze();
            }
        }

        public static void UpdateDefaultValue(this IFormDefinition definition, string name, object value)
        {
            var element = (DataFormField)definition.GetElements().FirstOrDefault(e => e is DataFormField d && d.Key == name);
            if (element != null)
            {
                element.DefaultValue = value is IValueProvider p ? p : new LiteralValue(value);
                element.Resources[nameof(DataFormField.DefaultValue)] = element.DefaultValue ?? LiteralValue.Null;
            }
        }
    }
}
