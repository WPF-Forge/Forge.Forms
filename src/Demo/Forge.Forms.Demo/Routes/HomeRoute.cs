using System;
using System.Collections.Generic;
using System.Windows;
using Bogus;
using Forge.Forms.Demo.Models;
using Forge.Forms.Demo.Models.Home;
using Material.Application.Infrastructure;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Routes
{
    public class HomeRoute : Route, IActionHandler
    {
        private readonly INotificationService notificationService;

        public HomeRoute(INotificationService notificationService)
        {
            this.notificationService = notificationService;
            RouteConfig.Title = "Introduction";
            RouteConfig.Icon = PackIconKind.Home;
            Model = new Introduction();
        }

        public object Model { get; }

        public void HandleAction(IActionContext actionContext)
        {
            switch ((actionContext.Action as string)?.ToLower())
            {
                case "copy" when actionContext.ActionParameter is string str:
                    Clipboard.SetText(str);
                    notificationService.Notify("Copied to clipboard.");
                    break;
                case "examples":
                    GoToMenuRoute<ExamplesRoute>();
                    break;
                case "xmlexamples":
                    GoToMenuRoute<XmlExamplesRoute>();
                    break;
            }
        }
    }

    public class CrudRoute : Route
    {
        public List<Login> Items { get; set; } = new List<Login>();

        public CrudRoute()
        {
            var fake1r = new Faker<Login>()
                .RuleFor(i => i.Username, f => f.Name.FirstName())
                .RuleFor(i => i.Password, f => Guid.NewGuid().ToString())
                .RuleFor(i => i.RememberMe, f => f.PickRandom(true, false))
                .FinishWith((faker, login) => Items.Add(login)).Generate(500);

            RouteConfig.Title = "Crud examples";
            RouteConfig.Icon = PackIconKind.Table;
        }
    }
}
