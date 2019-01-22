using System.Windows;
using Forge.Forms.Demo.Infrastructure;

namespace Forge.Forms.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public DemoAppController Controller { get; } = new DemoAppController();

        protected override void OnStartup(StartupEventArgs e)
        {
            Controller.ShowApplicationWindow();
            base.OnStartup(e);
        }
    }
}
