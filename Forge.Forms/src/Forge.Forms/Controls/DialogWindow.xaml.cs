using System.Windows.Input;
using MahApps.Metro.Controls;

namespace Forge.Forms.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DialogWindow : MetroWindow
    {
        private WindowOptions options;

        public DialogWindow(object model, object context, WindowOptions options)
        {
            this.options = options;
            DataContext = options;
            InitializeComponent();
            Loaded += DialogWindow_Loaded;
            Form.Environment.Add(options.EnvironmentFlags);
            Form.Context = context;
            Form.Model = model;
        }

        private void DialogWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (options.BringToFront)
            {
                this.Activate();
                this.Focus();
            }

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void CloseDialogCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = e.Parameter as bool?;
            Close();
        }

        private void CloseDialogCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
