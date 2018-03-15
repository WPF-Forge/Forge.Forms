using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Collections
{
    internal class UpdateFormDefinition : IFormDefinition
    {
        private readonly IFormDefinition inner;

        public UpdateFormDefinition(
            IFormDefinition inner,
            object model,
            IReadOnlyList<FormRow> formRows)
        {
            this.inner = inner;
            FormRows = formRows;
            Model = model;
            Snapshot = new Snapshot(model, new HashSet<string>(formRows
                .SelectMany(r => r.Elements.SelectMany(e => e.Elements))
                .Where(e => e is DataFormField)
                .Select(f => ((DataFormField)f).Key)));
        }

        public object Model { get; }

        public Snapshot Snapshot { get; }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;

        public object CreateInstance(IResourceContext context)
        {
            return Model;
        }
    }
}