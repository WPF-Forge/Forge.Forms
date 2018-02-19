using Forge.Forms.Demo.Routes;
using Material.Application.Infrastructure;
using Material.Application.Routing;

namespace Forge.Forms.Demo.Infrastructure
{
    public class DemoAppController : AppController
    {
        protected override void OnInitializing()
        {     
            MaterialDesignHelper.ReplaceDefaultHintProxies();
            var factory = Routes.RouteFactory;
            Routes.MenuRoutes.Add(InitialRoute = factory.Get<HomeRoute>());
            Routes.MenuRoutes.Add(factory.Get<ExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<XmlExamplesRoute>());
            FontSize = 15d;
        }
    }
}