using Forge.Forms.FormBuilding;

namespace Forge.Forms.Behaviors
{
    /// <inheritdoc />
    /// <summary>
    /// Provides information related to a property changed event.
    /// </summary>
    public interface IPropertyChangedContext : IEventContext
    {
        /// <summary>
        /// Gets the property name that raised the event.
        /// </summary>
        string PropertyName { get; }
    }

    internal class PropertyChangedContext : IPropertyChangedContext
    {
        public PropertyChangedContext(object model, IFormDefinition formDefinition, object formDefinitionSource, object context, IResourceContext resourceContext, string propertyName)
        {
            Model = model;
            FormDefinition = formDefinition;
            FormDefinitionSource = formDefinitionSource;
            Context = context;
            ResourceContext = resourceContext;
            PropertyName = propertyName;
        }

        public object Model { get; }

        public IFormDefinition FormDefinition { get; }

        public object FormDefinitionSource { get; }

        public object Context { get; }

        public IResourceContext ResourceContext { get; }

        public string PropertyName { get; }
    }
}