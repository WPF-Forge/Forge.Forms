using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Interfaces;
using Material.Application.Properties;

namespace Forge.Forms.Forms.Base
{
    public abstract class FormBase : IActionHandler, INotifyPropertyChanged
    {
        public event EventHandler<ActionEventArgs> ActionPerformed;

        public void HandleAction(object model, string action, object parameter)
        {
            OnAction(action, parameter);
            ActionPerformed?.Invoke(this, new ActionEventArgs(action));
        }

        protected virtual void OnAction(string action, object parameter)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
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
