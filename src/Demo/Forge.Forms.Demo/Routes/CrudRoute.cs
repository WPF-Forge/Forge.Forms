using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Routes
{
    public class CrudRoute : Route
    {
        public List<Person> Items { get; } = new List<Person>();

        public CrudRoute()
        {
            RouteConfig.Title = "Crud examples";
            RouteConfig.Icon = PackIconKind.Table;
        }
    }

    public class Person : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}