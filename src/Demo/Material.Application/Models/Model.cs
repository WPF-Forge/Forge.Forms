using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Material.Application.Properties;

namespace Material.Application.Models
{
    public abstract class Model : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public virtual bool IsValid => !HasErrors;

        public bool HasErrors => errors.Count != 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            List<string> errorList;
            errors.TryGetValue(propertyName, out errorList);
            return errorList;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AddError(string error, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                return true;
            }

            errors[propertyName] = new List<string> { error };
            NotifyErrorsChanged(propertyName);
            return true;
        }

        public bool RemoveError([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                return true;
            }

            if (errors.Remove(propertyName))
            {
                NotifyErrorsChanged(propertyName);
            }

            return true;
        }

        protected bool ValidateProperty(bool isValid, string error, [CallerMemberName] string propertyName = null)
        {
            if (isValid)
            {
                RemoveError(propertyName);
            }
            else
            {
                AddError(error, propertyName);
            }

            return isValid;
        }

        public void ClearErrors() => errors.Clear();

        public virtual bool Validate() => IsValid;

        public virtual void NotifyErrorsChanged(string propertyName)
            => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        protected RefreshSource RefreshSource() => new RefreshSource(this);

        [NotifyPropertyChangedInvocator]
        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
