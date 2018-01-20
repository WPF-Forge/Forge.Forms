using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Scripting;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    [ScriptAction("DECREMENT", "this.Counter--", Icon = PackIconKind.Minus)]
    [ScriptAction("INCREMENT", "this.Counter++", Icon = PackIconKind.Plus)]
    public class ScriptedCounter : INotifyPropertyChanged
    {
        private int counter;

        public int Counter
        {
            get => counter;
            set
            {
                counter = value;
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
