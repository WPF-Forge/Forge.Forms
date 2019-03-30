using System;
using System.Windows;
using Forge.Forms.Demo.Infrastructure;

namespace Forge.Forms.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Controller = new DemoAppController();
        }

        public DemoAppController Controller { get; }

        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        protected void OnStartup(object sender, StartupEventArgs e)
        {
            Controller.ShowApplicationWindow();
        }
    }
}
