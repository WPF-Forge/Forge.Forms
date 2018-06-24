using Forge.Forms.Controls;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Behaviors
{
    /// <summary>
    /// Provides information related to a form event.
    /// </summary>
    public interface IEventContext
    {
        /// <summary>
        /// Gets the current form model.
        /// </summary>
        object Model { get; }

        /// <summary>
        /// Gets the form definition that is built from the model source.
        /// </summary>
        IFormDefinition FormDefinition { get; }

        /// <summary>
        /// Gets the form definition source, which is the raw value
        /// provided to <see cref="DynamicForm.Model"/>.
        /// </summary>
        object FormDefinitionSource { get; }

        /// <summary>
        /// Gets the context associated with the dynamic form.
        /// </summary>
        object Context { get; }

        /// <summary>
        /// Gets the resource context associated with the dynamic form.
        /// </summary>
        IResourceContext ResourceContext { get; }
    }

    internal class EventContext : IEventContext
    {
        public EventContext(object model, IFormDefinition formDefinition, object formDefinitionSource, object context, IResourceContext resourceContext)
        {
            Model = model;
            FormDefinition = formDefinition;
            FormDefinitionSource = formDefinitionSource;
            Context = context;
            ResourceContext = resourceContext;
        }

        public object Model { get; }

        public IFormDefinition FormDefinition { get; }

        public object FormDefinitionSource { get; }

        public object Context { get; }

        public IResourceContext ResourceContext { get; }
    }
}