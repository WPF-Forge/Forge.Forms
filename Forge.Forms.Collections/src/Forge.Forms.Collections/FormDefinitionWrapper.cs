using System;
using System.Collections.Generic;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Collections
{
    internal class FormDefinitionWrapper : IFormDefinition
    {
        private readonly IFormDefinition inner;

        public FormDefinitionWrapper(IFormDefinition inner, IReadOnlyList<FormRow> formRows)
        {
            this.inner = inner;
            FormRows = formRows;
        }

        public IReadOnlyList<FormRow> FormRows { get; }

        public double[] Grid => inner.Grid;

        public Type ModelType => inner.ModelType;

        public IDictionary<string, IValueProvider> Resources => inner.Resources;

        public object CreateInstance(IResourceContext context)
        {
            return inner.CreateInstance(context);
        }
    }
}