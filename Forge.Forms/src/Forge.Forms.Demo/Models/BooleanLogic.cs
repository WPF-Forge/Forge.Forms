using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Title("Boolean Operators")]
    [Text("A message will appear when you set the right configuration.")]
    [Text("Toggle one must be switched ON, the text box must not be empty, and toggle two must be switched OFF.")]
    [Heading("Congratulations, you got the right combination!",
        IsVisible = "{Binding GivenUp} || ({Binding ToggleOne} && {Binding TypeSomething|IsNotEmpty} && !{Binding ToggleTwo})")]
    [Divider]
    public class BooleanLogic : INotifyPropertyChanged
    {
        private bool toggleOne;
        private bool toggleTwo;
        private string typeSomething;
        private bool givenUp;

        [Toggle]
        public bool ToggleOne
        {
            get => toggleOne;
            set
            {
                toggleOne = value;
                OnPropertyChanged();
            }
        }

        [Binding(UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged)]
        public string TypeSomething
        {
            get => typeSomething;
            set
            {
                typeSomething = value;
                OnPropertyChanged();
            }
        }

        [Toggle]
        public bool ToggleTwo
        {
            get => toggleTwo;
            set
            {
                toggleTwo = value;
                OnPropertyChanged();
            }
        }

        [Field(Name = "I give up, just show it already!")]
        public bool GivenUp
        {
            get => givenUp;
            set
            {
                givenUp = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
