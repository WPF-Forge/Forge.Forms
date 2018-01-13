using System;
using System.Collections.Generic;
using Forge.Forms.Components.Fields;

namespace Forge.Forms.Interfaces
{
    public interface IFormDefinition
    {
        IReadOnlyList<FormRow> FormRows { get; }

        double[] Grid { get; set; }

        Type ModelType { get; }

        IDictionary<string, IValueProvider> Resources { get; }

        object CreateInstance(IResourceContext context);
    }
}