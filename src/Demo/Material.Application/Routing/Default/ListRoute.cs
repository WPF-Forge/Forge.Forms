using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Material.Application.Views;

namespace Material.Application.Routing.Default
{
    public class ListRoute : Route
    {
        private object selectedItem;
        private string displayMemberPath;

        public ListRoute(string title, IEnumerable<object> items)
        {
            RouteConfig.Title = title;
            RouteConfig.KeyBindings.Add(new KeyBinding(PopRouteCommand, Key.Escape, ModifierKeys.None));
            Items = new ObservableCollection<object>(items);
        }

        public ObservableCollection<object> Items { get; }

        public object SelectedItem
        {
            get => selectedItem;
            set
            {
                if (Equals(value, selectedItem)) return;
                selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public string DisplayMemberPath
        {
            get => displayMemberPath;
            set
            {
                if (value == displayMemberPath) return;
                displayMemberPath = value;
                NotifyPropertyChanged();
            }
        }

        protected internal override object CreateView(bool isReload)
        {
            return new CollectionView();
        }
    }
}
