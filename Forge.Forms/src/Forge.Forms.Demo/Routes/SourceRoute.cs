using System;
using System.IO;
using Material.Application.Routing;

namespace Forge.Forms.Demo.Routes
{
    public class SourceRoute : Route
    {
        private readonly Lazy<string> source;

        public SourceRoute(string title, string path)
        {
            RouteConfig.Title = title;
            source = new Lazy<string>(() =>
            {
                try
                {
                    return File.ReadAllText(path);
                }
                catch
                {
                    return "";
                }
            });
        }

        public string Source => source.Value;
    }
}
