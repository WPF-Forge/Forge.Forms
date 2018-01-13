using System.Windows.Controls;

namespace Material.Application.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    internal partial class LoadingView : UserControl
    {
        public LoadingView(string message)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(message))
            {
                MessageTextBlock.Text = message;
            }
            else
            {
                StackPanel.Children.RemoveAt(1);
            }
        }
    }
}
