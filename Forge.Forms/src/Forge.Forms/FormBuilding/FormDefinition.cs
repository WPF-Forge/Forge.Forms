using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Forge.Forms.FormBuilding
{
    public class FormDefinition : IFormDefinition
    {
        private bool frozen;

        public FormDefinition(Type modelType)
        {
            ModelType = modelType;
            Resources = new Dictionary<string, IValueProvider>();
            Grid = new[] { 1d };
            FormRows = new List<FormRow>();
        }

        public List<FormRow> FormRows { get; set; }

        public Type ModelType { get; }

        public IDictionary<string, IValueProvider> Resources { get; set; }

        public double[] Grid { get; set; }

        IReadOnlyList<FormRow> IFormDefinition.FormRows => FormRows;

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
