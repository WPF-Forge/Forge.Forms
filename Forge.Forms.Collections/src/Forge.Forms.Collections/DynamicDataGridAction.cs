using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Collections
{
    public class DynamicDataGridAction : INotifyPropertyChanged
    {
        public PackIconKind? Icon { get; set; }

        public string Title { get; set; }

        public ICommand ClickCommand { get; }

        public Action<Button> OnClickAction { get; set; }

        /// <inheritdoc />
        public DynamicDataGridAction(string title, Action<Button> onClickAction,
            PackIconKind? icon = null)
        {
            Icon = icon;
            Title = title;
            OnClickAction = onClickAction;
            ClickCommand = new RelayCommand(o =>
            {
                if (o is Button obj)
                {
                    OnClickAction?.Invoke(obj);
                }
                else
                {
                    throw new InvalidOperationException(
                        "The command parameter is not a button, this should not happen. Please check the CommandParameter binding.");
                }
            });
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;
    }
}