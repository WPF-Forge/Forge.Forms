
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Text("Click the button to invalidate the text field")]
    [Action("invalidate", "INVALIDATE")]
    public class CustomValidation : IActionHandler, INotifyPropertyChanged
    {
        private string text;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public void HandleAction(IActionContext actionContext)
        {
            if (actionContext.Action is "invalidate")
            {
                ModelState.Invalidate(this, nameof(Text), "Invalid value.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    } 
}
