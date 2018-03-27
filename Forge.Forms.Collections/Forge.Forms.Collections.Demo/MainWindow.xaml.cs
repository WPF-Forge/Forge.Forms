using Forge.Forms.Collections.Demo.Models;

namespace Forge.Forms.Collections.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TestModel();
        }
    }
}