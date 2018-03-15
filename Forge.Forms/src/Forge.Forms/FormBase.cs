using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Forge.Forms
{
    public abstract class FormBase : IActionHandler, INotifyPropertyChanged
    {
        public void HandleAction(IActionContext actionContext)
        {
            var action = actionContext.Action;
            var parameter = actionContext.ActionParameter;
            OnAction(action, parameter);
            ActionPerformed?.Invoke(this, new ActionEventArgs(actionContext));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ActionEventArgs> ActionPerformed;

        protected virtual void OnAction(object action, object parameter)
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
