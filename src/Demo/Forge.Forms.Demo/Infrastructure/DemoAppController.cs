using Forge.Forms.Annotations;
using Forge.Forms.Demo.Models;
using Forge.Forms.Demo.Routes;
using Forge.Forms.Livereload;
using Forge.Forms.Mapper;
using Material.Application.Infrastructure;
using Material.Application.Routing;

namespace Forge.Forms.Demo.Infrastructure
{
    public class DemoAppController : AppController
    {
        protected override void OnInitializing()
        {
            HotReloadManager.WatchAllFiles = false;
            Proxier.Mappers.Maps.ProxierMapper.InitializeMapperClasses();
            var factory = Routes.RouteFactory;
            Routes.MenuRoutes.Add(InitialRoute = factory.Get<HomeRoute>());
            Routes.MenuRoutes.Add(factory.Get<ExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<XmlExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<CsharpExamplesRoute>());
            Routes.MenuRoutes.Add(factory.Get<CrudRoute>());
            FontSize = 15d;
        }
    }
}