using System;
using System.Collections.Generic;
using System.Linq;

namespace Forge.Forms.FormBuilding
{
    public interface IFormDefinition
    {
        IReadOnlyList<FormRow> FormRows { get; }

        double[] Grid { get; }

        Type ModelType { get; }

        IDictionary<string, IValueProvider> Resources { get; }

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
        
        public static void UpdateDefaultValue(this FormDefinition definition, string name, object value)
        {
            var element = (DataFormField)definition.GetElements().FirstOrDefault(e => e is DataFormField d && d.Key == name);
            if (element != null)
            {
                element.DefaultValue = new DynamicExpressions.LiteralValue(value);
            }
        }
        
    }
}
