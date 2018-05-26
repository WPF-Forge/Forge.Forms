using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Forge.Forms.Collections.Demo.Annotations;
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
            DataContext = new MainWindowModel();
        }
    }
}