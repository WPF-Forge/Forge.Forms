using System;
using System.IO;
using Forge.Application.Routing;

namespace Forge.Forms.Demo.Routes
{
    public class SourceRoute : Route
    {
        private readonly Lazy<string> source;

        public SourceRoute(string title, string source, bool isPath)
        {
            RouteConfig.Title = title;
            this.source = new Lazy<string>(() =>
            {
                try
                {
                    return isPath ? File.ReadAllText(source) : source;
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
