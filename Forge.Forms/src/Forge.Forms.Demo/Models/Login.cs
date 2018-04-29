using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    [Title("Log in to continue")]
    [Action("cancel", "CANCEL", IsCancel = true, ClosesDialog = true)]
    [Action("login", "LOG IN", IsLoading = "{Binding Loading}",
        IsDefault = true, ClosesDialog = true, Validates = true)]
    public class Login : IActionHandler, INotifyPropertyChanged
    {
        private string username;
        private string password;
        private bool rememberMe;
        private bool loading;

        // Enums may be deserialized from strings.
        [Field(Icon = "Account")]
        [Value(Must.NotBeEmpty)]
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        // Or be dynamically assigned...
        [Field(Icon = "{Property PasswordIcon}")]
        [Password]
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public bool RememberMe
        {
            get => rememberMe;
            set
            {
                rememberMe = value;
                OnPropertyChanged();
            }
        }

        [FieldIgnore]
        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public PackIconKind PasswordIcon => PackIconKind.Key;

        /// <inheritdoc />
        public void HandleAction(IActionContext actionContext)
        {
            var action = actionContext.Action;
            switch (action)
            {
                case "cancel":
                    Loading = false;
                    break;
                case "login":
                    Loading = true;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}