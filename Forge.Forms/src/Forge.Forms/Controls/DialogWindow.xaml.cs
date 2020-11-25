using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace Forge.Forms.Controls
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class DialogWindow : MetroWindow
	{
		public DialogWindow(object model, object context, WindowOptions options)
		{
			DataContext = options;
			InitializeComponent();
			Loaded += (sender, e) =>
			{
				if (options.CenterOnParentWindow)
				{
					this.Left = Application.Current.MainWindow.Left + (Application.Current.MainWindow.Width / 2) - (this.Width / 2);
					this.Top = Application.Current.MainWindow.Top + (Application.Current.MainWindow.Height / 2) - (this.Height / 2);
				}
				MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			};
			Form.Environment.Add(options.EnvironmentFlags);
			Form.Context = context;
			Form.Model = model;
			
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
