using Forge.Forms.Demo.Routes;
using Material.Application.Infrastructure;
using Material.Application.Routing;

namespace Forge.Forms.Demo.Infrastructure
{
    public class DemoAppController : AppController
    {
        protected override void OnInitializing()
        {
            HotReloadManager.Init(@"C:\Users\Marcos\RiderProjects\HotReloadProofOfConcept\ConsoleApplication2\");
            var factory = Routes.RouteFactory;
            Routes.MenuRoutes.Add(InitialRoute = factory.Get<HomeRoute>());
            Routes.MenuRoutes.Add(factory.Get<ExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<XmlExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<CsharpExamplesRoute>());
            FontSize = 15d;
        }
    }
}
