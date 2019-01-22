using Forge.Application.Infrastructure;
using Forge.Application.Routing;
using Forge.Forms.Controls;
using Forge.Forms.Demo.Behaviors;
using Forge.Forms.Demo.Routes;

namespace Forge.Forms.Demo.Infrastructure
{
    public class DemoAppController : AppController
    {
        protected override void OnInitializing()
        {
            DynamicForm.AddBehavior(new CheckAllBehavior());
            var factory = Routes.RouteFactory;
            Routes.MenuRoutes.Add(InitialRoute = factory.Get<HomeRoute>());
            Routes.MenuRoutes.Add(factory.Get<ExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<XmlExamplesRoute>());
            FontSize = 15d;
        }
    }
}