using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Bogus;
using Forge.Forms.Collections.Demo.Annotations;

namespace Forge.Forms.Collections.Demo.Models
{
    internal class MainWindowModel : INotifyPropertyChanged
    {
        public List<Person> People { get; } = new List<Person>();

        public MainWindowModel()
        {
            var x = new Faker<Person>().RuleFor(i => i.FirstName, f => f.Name.FirstName())
                .RuleFor(i => i.LastName, f => f.Name.LastName())
                .RuleFor(i => i.Gender, f => f.PickRandom("Male", "Female"))
                .RuleFor(i => i.Age, f => f.Random.Int(1, 120));
            People.AddRange(x.Generate(100));
        }

        private bool _isCheckboxColumnEnabled;
        private bool _isFilterCaseSensitive;

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