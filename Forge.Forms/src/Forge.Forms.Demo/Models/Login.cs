using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Display;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    [Title("Login to continue")]
    [Action("cancel", "CANCEL", IsCancel = true, ClosesDialog = true)]
    [Action("login", "LOG IN", IsLoading = "{Binding Loading}",
        IsDefault = true, ClosesDialog = true, Validates = true)]
    public class Login : IActionHandler
    {
        // Enums may be deserialized from strings.
        [Field(Icon = "Account")]
        [Value(Must.NotBeEmpty)]
        public string Username { get; set; }

        // Or be dynamically assigned...
        [Field(Icon = "{Property PasswordIcon}", InitialFocus = true)]
        [Password]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool Loading { get; set; }

        public PackIconKind PasswordIcon => PackIconKind.Key;

        /// <inheritdoc />
        public void HandleAction(IActionContext actionContext)
        {
            Loading = !Loading;
        }
    }
}