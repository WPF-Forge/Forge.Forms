using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Display;
using Forge.Forms.Livereload.Annotations;
using Forge.Forms.Mapper;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    public class LoginExtension : MaterialMapper<Login>
    {
        /// <inheritdoc />
        public LoginExtension()
        {
            AddClassAttribute(() => new ActionAttribute("oka", "Hello world!", 0));
        }

        /// <inheritdoc />
        public override void HandleAction(IActionContext context)
        {
            base.HandleAction(context);
        }
    }

    [Title("Login to continue")]
    [Action("cancel", "CANCEL", IsCancel = true, ClosesDialog = true)]
    [Action("login", "LOG IN", IsLoading = "{Binding Loading}", IsDefault = true, ClosesDialog = true)]
    [HotReload(true)]
    public class Login : IActionHandler
    {
        // Enums may be deserialized from strings.
        [Field(Icon = "Account")] public string Username { get; set; }

        // Or be dynamically assigned...
        [Field(Icon = "{Property PasswordIcon}")]
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