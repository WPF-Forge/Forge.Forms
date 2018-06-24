namespace Forge.Forms.Behaviors
{
    /// <summary>
    /// Handles property changed events of the form model.
    /// </summary>
    public interface IPropertyChangedBehavior
    {
        /// <summary>
        /// Handles property changed events of form models.
        /// </summary>
        /// <param name="context">Object that carries event information.</param>
        void PropertyChanged(IPropertyChangedContext context);
    }
}
