using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Forge.Forms.Livereload;
using Material.Application.Infrastructure;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Routes
{
    public class CsharpExamplesRoute : Route, IActionHandler
    {
        private readonly INotificationService notificationService;

        private object compiledDefinition;
        private string csharpString;

        public CsharpExamplesRoute(INotificationService notificationService)
        {
            this.notificationService = notificationService;
            RouteConfig.Title = "CSharp Examples";
            RouteConfig.Icon = PackIconKind.Xml;
            BuildDefinitionCommand = Command(BuildDefinition);
        }

        /// <summary>
        /// Gets the compiled definition.
        /// </summary>
        /// <value>
        /// The compiled definition.
        /// </value>
        public object CompiledDefinition
        {
            get => compiledDefinition;
            private set
            {
                if (Equals(compiledDefinition, value))
                {
                    return;
                }

                compiledDefinition = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the csharp string.
        /// </summary>
        /// <value>
        /// The csharp string.
        /// </value>
        public string CsharpString
        {
            get => csharpString;
            set
            {
                if (Equals(csharpString, value))
                {
                    return;
                }

                csharpString = value;

                NotifyPropertyChanged();
                BuildDefinition();
            }
        }

        public ICommand BuildDefinitionCommand { get; }

        public void HandleAction(IActionContext actionContext)
        {
            notificationService.Notify($"Action '{actionContext.Action}'");
        }

        protected override void RouteInitializing()
        {
            CsharpString =
                @"using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Display;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    [Title(""Login to continue"")]
    [Action(""cancel"", ""CANCEL"", IsCancel = true)]
    [Action(""login"", ""LOG IN"", IsLoading = ""{Binding Loading}"", IsDefault = true)]
    public class Login : IActionHandler
    {
        // Enums may be deserialized from strings.
        [Field(Icon = ""Account"")]
        public string Username { get; set; }

        // Or be dynamically assigned...
        [Field(Icon = ""{Property PasswordIcon}"")]
        [Password]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool Loading { get; set; }

        public PackIconKind PasswordIcon => PackIconKind.Key;

        /// <inheritdoc />
        public void HandleAction(object model, string action, object parameter)
        {
            Loading = !Loading;
        }
    }
}
";

            BuildDefinition();
        }

        /// <summary>
        /// Builds the definition.
        /// </summary>
        private void BuildDefinition()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (CompiledDefinition == null)
                    {
                        CompiledDefinition =
                            Activator.CreateInstance(HotReloadManager.CompileCode(CsharpString).First());
                    }
                    else
                    {
                        HotReloadManager.ApplyTypesToDynamicForms(HotReloadManager.CompileCode(CsharpString).ToList());
                    }
                }
                catch (Exception ex)
                {
                    notificationService.ForceNotify(ex.Message, "COPY", () => Clipboard.SetText(ex.ToString()));
                }
            });
        }
    }
}
