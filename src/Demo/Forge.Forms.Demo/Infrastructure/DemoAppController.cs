using System.Windows;
using Forge.Forms.Annotations;
using Forge.Forms.Controls;
using Forge.Forms.Demo.Models;
using Forge.Forms.Demo.Routes;
using Forge.Forms.Mappers;
using MahApps.Metro.Controls;
using Material.Application.Infrastructure;
using Material.Application.Routing;

namespace Forge.Forms.Demo.Infrastructure
{
    public class LoginExtensions : MaterialMapper<Login>
    {
        public LoginExtensions()
        {
            AddPropertyAttribute(i => i.RememberMe, () => new FieldAttribute { Name = "sdfgsrysert" });
        }

        public override void Action(Login model, string action, object parameter)
        {
            base.Action(model, action, parameter);
        }
    }

    public class DemoAppController : AppController
    {
        protected override void OnInitializing()
        {
            var factory = Routes.RouteFactory;
            Routes.MenuRoutes.Add(InitialRoute = factory.Get<HomeRoute>());
            Routes.MenuRoutes.Add(factory.Get<ExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<XmlExamplesRoute>());
            FontSize = 15d;

            var window = new MetroWindow
            {
                Content = new DynamicForm { Model = new Login() },
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Height = 200,
                Width = 200
            };
            window.Show();
        }
    }
}
