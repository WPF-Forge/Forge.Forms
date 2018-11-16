using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Forge.Forms.FormBuilding;
using Material.Application.Infrastructure;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace Forge.Forms.Demo.Routes
{
    public class XmlExamplesRoute : Route, IActionHandler
    {
        private readonly INotificationService notificationService;

        private IFormDefinition compiledDefinition;
        private string xmlString;

        public XmlExamplesRoute(INotificationService notificationService)
        {
            this.notificationService = notificationService;
            RouteConfig.Title = "XML Examples";
            RouteConfig.Icon = PackIconKind.Xml;
            BuildDefinitionCommand = Command(BuildDefinition);
            RouteConfig.RouteCommands.Add(Command("View JSON", PackIconKind.CodeBraces, ViewSource));
            RouteConfig.RouteCommands.Add(Command("Print Form", PackIconKind.Printer, Print));
        }

        public IFormDefinition CompiledDefinition
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

        public string XmlString
        {
            get => xmlString;
            set
            {
                if (Equals(xmlString, value))
                {
                    return;
                }

                xmlString = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand BuildDefinitionCommand { get; }

        public object CurrentModel { get; set; }

        public Visual Form { get; set; }

        private void ViewSource()
        {
            string json;
            try
            {
                json = JsonConvert.SerializeObject(CurrentModel, Formatting.Indented);
            }
            catch
            {
                return;
            }

            GetRoute<SourceRoute>("title", "XML Form", "source", json, "isPath", false).Push();
        }

        private void Print()
        {
            if (Form == null)
            {
                return;
            }

            var printDlg = new PrintDialog();
            printDlg.PrintVisual(Form, "Form Printing.");
        }

        public void HandleAction(IActionContext actionContext)
        {
            notificationService.Notify($"Action '{actionContext.Action}'");
        }

        protected override void RouteInitializing()
        {
            XmlString =
                @"<form>
    <title>Create account</title>
    <heading>Personal details</heading>
    <input type=""string"" name=""FirstName""
           label=""First name""
           tooltip=""Enter your first name here.""
           icon=""pencil"">
        <validate must=""NotBeEmpty"" />
    </input>
    <input type=""string"" name=""LastName""
           label=""Last name"" icon=""empty""
           tooltip=""Enter your last name here."" />
    <input type=""datetime?""
           name=""DateOfBirth""
           label=""Date of birth""
           icon=""calendar""
           conversionError=""Invalid date string."">
        <validate must=""NotBeEmpty"" />
        <validate must=""BeLessThan"" value=""2020-01-01"">
            You said you are born in the year {Value:yyyy}. Are you really from the future?
        </validate>
    </input>
    <input type=""time?"" name=""TimeOfBirth"" 
           label=""Time of birth"" icon=""empty""
           tooltip=""We really must know!""/>
    <heading>Account details</heading>
    <input type=""string"" name=""Username""
           label=""Username"" icon=""account"" >
        <validate must=""MatchPattern"" value=""^[a-zA-Z][a-zA-Z0-9]*$""
                  message=""'{Value}' is not a valid username."" />
    </input>
    <password name=""Password""
           label=""Password"" icon=""key"">
        <validate converter=""Length"" must=""BeGreaterThanOrEqualTo"" value=""6"">
            Your password has {Value|Length} characters, which is less than the required {Argument}.
        </validate>
    </password>
    <password name=""PasswordConfirm""
           label=""Confirm password"" icon=""empty"">
        <validate must=""BeEqualTo"" value=""{Binding Password}""
                  onValueChanged=""ClearErrors""
                  message=""The entered passwords do not match."" />
    </password>
    <br />
    <heading icon=""checkall"">Review entered information</heading>
    <text>Name: {Binding FirstName} {Binding LastName}</text>
    <text>Date of birth: {Binding DateOfBirth:yyyy-MM-dd}</text>
    <text>Username: {Binding Username}</text>
    <br />
    <heading>License agreement</heading>
    <text>By signing up, you agree to our terms of use, privacy policy, and cookie policy.</text>
    <input type=""bool"" name=""Agree"" label=""Agree to license"" defaultValue=""false"">
        <validate must=""BeTrue"">You must accept the license agreement.</validate>
    </input>
    <row>
        <action name=""reset"" content=""RESET"" icon=""close"" resets=""true"" />
        <action name=""submit"" content=""SUBMIT"" icon=""check"" validates=""true"" />
    </row>
</form>";

            BuildDefinition();
        }

        private void BuildDefinition()
        {
            try
            {
                CompiledDefinition = FormBuilder.Default.GetDefinition(xmlString);
            }
            catch (Exception ex)
            {
                notificationService.ForceNotify(ex.Message, "COPY", () => Clipboard.SetText(ex.ToString()));
            }
        }
    }
}
