using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Bogus;
using Forge.Forms.Collections.Demo.Annotations;
using Forge.Forms.Collections.Extensions;

namespace Forge.Forms.Collections.Demo.Models
{
    internal class MainWindowModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; }

        public MainWindowModel()
        {
            People = new ObservableCollection<Person>(PersonFaker.Generate(1));
            AddRandomPerson = new RelayCommand(o => People.Add(PersonFaker.Generate()));
            ReloadColumnsCommand = new RelayCommand(o =>
            {
                var dynamicDataGrid = Application.Current.MainWindow.TryFindChild<DynamicDataGrid>();
                dynamicDataGrid.ReloadColumns();
            });
        }

        public ICommand AddRandomPerson { get; }

        public ICommand ReloadColumnsCommand { get; }

        private bool _isCheckboxColumnEnabled;
        private bool _isFilterCaseSensitive;

        private Faker<Person> PersonFaker { get; } = new Faker<Person>()
            .RuleFor(i => i.FirstName, f => f.Name.FirstName())
            .RuleFor(i => i.LastName, f => f.Name.LastName())
            .RuleFor(i => i.Gender, f => f.PickRandom("Male", "Female"))
            .RuleFor(i => i.Age, f => f.Random.Int(1, 120));

        public bool IsCheckboxColumnEnabled
        {
            get => _isCheckboxColumnEnabled;
            set
            {
                _isCheckboxColumnEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsFilterCaseSensitive
        {
            get => _isFilterCaseSensitive;
            set
            {
                _isFilterCaseSensitive = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}