using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Forge.Forms.FormBuilding
{
    public class FormDefinition : IFormDefinition
    {
        private bool frozen;
        private readonly Dictionary<string, IValueProvider> resources;
        private readonly Dictionary<string, string> metadata;

        public FormDefinition(Type modelType)
        {
            ModelType = modelType;
            resources = new Dictionary<string, IValueProvider>();
            metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Grid = new[] { 1d };
            FormRows = new List<FormRow>();
            FormProperties = new List<IFormProperty>();
        }

        public List<FormRow> FormRows { get; set; }

        public Type ModelType { get; }

        public IDictionary<string, string> Metadata => metadata;
        public IDictionary<string, IValueProvider> Resources => resources;

        public double[] Grid { get; set; }

        IReadOnlyList<FormRow> IFormDefinition.FormRows => FormRows;

        internal List<IFormProperty> FormProperties { get; }
        

        IReadOnlyCollection<IFormProperty> IReadOnlyFormDefinition.FormProperties => FormProperties;

        IReadOnlyDictionary<string, IValueProvider> IReadOnlyFormDefinition.Resources => resources;

        IReadOnlyDictionary<string, string> IReadOnlyFormDefinition.Metadata => metadata;

        public object CreateInstance(IResourceContext context)
        {
            if (ModelType != null)
            {
                return Activator.CreateInstance(ModelType);
            }

            if (!frozen)
            {
                throw new InvalidOperationException("Cannot create dynamic models without freezing this object.");
            }

            var expando = new ExpandoObject();
            IDictionary<string, object> dictionary = expando;
            foreach (var field in FormRows
                .SelectMany(row => row.Elements
                    .SelectMany(c => c.Elements)))
            {
                if (field is DataFormField dataField && dataField.Key != null && !dataField.IsDirectBinding)
                {
                    dictionary[dataField.Key] = dataField.GetDefaultValue(context);
                }
            }

            return expando;
        }

        protected internal virtual void Freeze()
        {
            if (frozen)
            {
                return;
            }

            frozen = true;
        }
    }
}
