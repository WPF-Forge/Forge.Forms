using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Material.Application.Commands;
using Material.Application.Properties;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Routing
{
    public class RouteConfig : INotifyPropertyChanged
    {
        private PackIconKind? icon;
        private List<KeyBinding> keyBindings;
        private bool showAppBar = true;
        private bool showTitle = true;
        private string title;

        public RouteConfig()
        {
            RouteCommands = new ObservableCollection<IMenuCommand>();
            KeyBindings = new List<KeyBinding>();
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (value == title) return;
                title = value;
                OnPropertyChanged();
            }
        }

        public bool ShowTitle
        {
            get { return showTitle; }
            set
            {
                if (value == showTitle) return;
                showTitle = value;
                OnPropertyChanged();
            }
        }

        public bool ShowAppBar
        {
            get { return showAppBar; }
            set
            {
                if (value == showAppBar) return;
                showAppBar = value;
                OnPropertyChanged();
            }
        }

        public PackIconKind? Icon
        {
            get { return icon; }
            set
            {
                if (value == icon) return;
                icon = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IMenuCommand> RouteCommands { get; }

        public List<KeyBinding> KeyBindings
        {
            get { return keyBindings; }
            set
            {
                if (Equals(value, keyBindings)) return;
                keyBindings = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddRouteCommandsSeparator() => RouteCommands.Add(null);

        public void RefreshKeyBindings() => OnPropertyChanged(nameof(KeyBindings));

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
