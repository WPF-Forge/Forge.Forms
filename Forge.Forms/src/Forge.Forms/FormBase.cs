using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Forge.Forms
{
    /// <summary>
    /// Utility base class for forms.
    /// Provides a default implementation of <see cref="IActionHandler"/> and <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class FormBase : IActionHandler, INotifyPropertyChanged
    {
        /// <summary>
        /// Handles <see cref="IActionHandler.HandleAction"/> and raises the <see cref="ActionPerformed"/> event.
        /// </summary>
        /// <param name="actionContext"></param>
        public void HandleAction(IActionContext actionContext)
        {
            var action = actionContext.Action;
            var parameter = actionContext.ActionParameter;
            OnAction(action, parameter);
            ActionPerformed?.Invoke(this, new ActionEventArgs(actionContext));
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Is raised when an action is handled by this form.
        /// </summary>
        public event EventHandler<ActionEventArgs> ActionPerformed;

        /// <summary>
        /// Gets called when an action is handled by this form.
        /// </summary>
        /// <param name="action">Action identifier.</param>
        /// <param name="parameter">Action parameter.</param>
        protected virtual void OnAction(object action, object parameter)
        {
        }

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
