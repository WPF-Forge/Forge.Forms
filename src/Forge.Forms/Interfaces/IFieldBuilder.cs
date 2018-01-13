using System;
using Forge.Forms.Components.Fields;

namespace Forge.Forms.Interfaces
{
    /// <summary>
    /// Intercepts properties and builds form elements if conditions are met.
    /// </summary>
    public interface IFieldBuilder
    {
        FormElement TryBuild(IFormProperty property, Func<string, object> deserializer);
    }
}
