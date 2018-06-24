namespace Forge.Forms.Behaviors
{
    /// <summary>
    /// Handles model changes of forms.
    /// </summary>
    public interface IModelChangedBehavior
    {
        /// <summary>
        /// Handles model changed events of forms.
        /// </summary>
        /// <param name="context">Object that carries event information.</param>
        void ModelChanged(IEventContext context);
    }
}
