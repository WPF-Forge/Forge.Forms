using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Material.Application.Infrastructure;
using Material.Application.Routing;

namespace Material.Application.Controls
{
    /// <summary>
    /// Interaction logic for MaterialRoutesWindow.xaml
    /// </summary>
    public partial class MaterialRoutesWindow : MetroWindow
    {
        private readonly AppController controller;

        public MaterialRoutesWindow(AppController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            DataContext = controller;
            this.controller = controller;
            InitializeComponent();
        }

        public object CurrentView => VisualTreeHelper.GetChild(RouteContentPresenter, 0);

        private void MenuRoute_Click(object sender, RoutedEventArgs e)
        {
            if (!controller.IsMenuOpen)
            {
                return;
            }

            controller.IsMenuOpen = false;
            var route = ((FrameworkElement)sender).DataContext as Route;
            controller.Routes.Change(route);
        }
    }
}
