using System;
using Forge.Forms.Components.Fields;

namespace Forge.Forms.Interfaces
{
    /// <summary>
    /// Initializes built form fields.
    /// </summary>
    public interface IFieldInitializer
    {
        void Initialize(FormElement element, IFormProperty property, Func<string, object> deserializer);
    }
}
