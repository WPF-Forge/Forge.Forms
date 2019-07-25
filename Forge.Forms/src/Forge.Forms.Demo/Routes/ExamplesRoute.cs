using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Forge.Application.Infrastructure;
using Forge.Application.Models;
using Forge.Application.Routing;
using Forge.Forms.Demo.Infrastructure;
using Forge.Forms.Demo.Models;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Routes
{
    public class ExamplesRoute : Route, IActionHandler
    {
        private static readonly string ModelsDir = Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? Directory.GetCurrentDirectory(),
            "Models");

        private readonly INotificationService notificationService;
        private readonly RefreshSource currentModelRefresh;

        private ExamplePresenter currentModel;

        public ExamplesRoute(INotificationService notificationService)
        {
            RouteConfig.Title = "Examples";
            RouteConfig.Icon = PackIconKind.ViewList;
            currentModelRefresh = RefreshSource().WithProperties(nameof(CurrentModel));

            RouteConfig.RouteCommands.Add(Command("Validate model", PackIconKind.CheckAll,
                () =>
                {
                    var isValid = ModelState.Validate(CurrentModel.Object);
                    notificationService.Notify($"Is valid: {isValid}");
                }));

            RouteConfig.RouteCommands.Add(Command("Is valid?", PackIconKind.Help,
                () =>
                {
                    var isValid = ModelState.IsValid(CurrentModel.Object);
                    notificationService.Notify($"Is valid: {isValid}");
                }));

            RouteConfig.RouteCommands.Add(Command("Reset model", PackIconKind.Undo,
                () => ModelState.Reset(CurrentModel.Object)));
            RouteConfig.RouteCommands.Add(Command("View source", PackIconKind.CodeBraces,
                ViewSource, CanViewSource, currentModelRefresh));
            this.notificationService = notificationService;
        }

        public ExamplePresenter CurrentModel
        {
            get => currentModel;
            set
            {
                currentModel = value;
                currentModelRefresh.Refresh();
            }
        }

        public ObservableCollection<ExamplePresenter> Models { get; private set; }

        public void HandleAction(IActionContext actionContext)
        {
            notificationService.Notify($"Action '{actionContext.Action}'");
        }

        protected override void RouteInitializing()
        {
            Models = new ObservableCollection<ExamplePresenter>(GetModels());
            CurrentModel = Models.FirstOrDefault();
        }

        private void ViewSource()
        {
            if (CanViewSource(out var name, out var source))
            {
                GetRoute<SourceRoute>("title", currentModel.DisplayString ?? name, "source", source, "isPath", currentModel.Source == null).Push();
            }
        }

        private bool CanViewSource() => CanViewSource(out var _, out var _);

        private bool CanViewSource(out string name, out string source)
        {
            var model = currentModel?.Object;
            if (model == null)
            {
                name = null;
                source = null;
                return false;
            }


            name = model.GetType().Name;
            if (currentModel.Source != null)
            {
                source = currentModel.Source;
                return true;
            }

            source = Path.Combine(ModelsDir, name + ".cs");
            return File.Exists(source);
        }

        private IEnumerable<ExamplePresenter> GetModels()
        {
            const double small = 320d;
            const double large = 540d;

            yield return new ExamplePresenter(new Login(), "Login", large);

            yield return new ExamplePresenter(new Settings(), "Settings", large);

            yield return new ExamplePresenter(new User(), "User", large);

            yield return new ExamplePresenter(new TextFields(), "Text Fields", large);

            yield return new ExamplePresenter(new TextElements(), "Text Elements", large);

            yield return new ExamplePresenter(new Selection(), "Selection", large);

            yield return new ExamplePresenter(new SelectionMemberPaths(), "Selection Member Paths", large);

            yield return new ExamplePresenter(new FoodSelection(), "Food Selection", large);

            yield return new ExamplePresenter(new Dialogs(), "Dialogs", large);

            yield return new ExamplePresenter(new BooleanLogic(), "Boolean Expressions", large);

            yield return new ExamplePresenter(new InlineElements(), "Inline Elements", large);

            //yield return new ExamplePresenter(new Crud(), "CRUD", 2 * large);

            yield return new ExamplePresenter(new EnvManager(), "Environments", large);

            yield return new ExamplePresenter(new FileBindings(), "File Binding", large);

            yield return new ExamplePresenter(new CustomValidation(), "Validation", large);

            yield return new ExamplePresenter(new DirectContent(), "Direct Content", large);

            yield return new ExamplePresenter(new Alert
            {
                Message = "Item deleted."
            }, "Alert", small)
            {
                Source = @"new Alert(""Item deleted."");"
            };

            yield return new ExamplePresenter(new Confirmation
            {
                Message = "Discard draft?",
                PositiveAction = "DISCARD"
            }, "Confirm 1", small)
            {
                Source = @"new Confirmation(""Discard draft?"") { PositiveAction = ""DISCARD"" };"
            };

            yield return new ExamplePresenter(new Confirmation
            {
                Title = "Use Google's location service?",
                Message =
                    "Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.",
                PositiveAction = "AGREE",
                NegativeAction = "DISAGREE"
            }, "Confirm 2", small)
            {
                Source = @"new Confirmation(
    ""Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running."",
    ""Use Google's location service?"", ""AGREE"", ""DISAGREE"");"
            };

            yield return new ExamplePresenter(new Prompt<string>
            {
                Title = "Enter your name"
            }, "Prompt 1", small)
            {
                Source = @"new Prompt<string> { Title = ""Enter your name"" };"
            };

            yield return new ExamplePresenter(new Prompt<bool>
            {
                Message = "Discard draft?",
                PositiveAction = "DISCARD",
                Name = "Prevent future dialogs"
            }, "Prompt 2", small)
            {
                Source = @"new Prompt<bool> 
{ 
    Message = ""Discard draft?"",
    PositiveAction = ""DISCARD"",
    Name = ""Prevent future dialogs""
};"
            };

            yield return new ExamplePresenter(new DataTypes(), "Data types", large);
        }
    }
}
