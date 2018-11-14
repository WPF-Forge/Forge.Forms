
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using Forge.Forms.Validation;

namespace Forge.Forms.Demo.Models
{
    public class CustomValidation : IActionHandler, INotifyPropertyChanged
    {
        public static bool Validate(ValidationContext validationContext)
        {
            return false;
        }

        private string throughModelState;
        private string throughStaticMethod;
        private string externalValidation;
        private bool invalidateTextbox;

        [Text("Click the button to invalidate the text field")]
        [Action("invalidate", "INVALIDATE")]
        public string ThroughModelState
        {
            get => throughModelState;
            set
            {
                throughModelState = value;
                OnPropertyChanged();
            }
        }

        [Text("Trigger validation from the popup menu or from the button below.")]
        [Action("validate", "VALIDATE")]

        [Value(Must.SatisfyMethod, nameof(Validate))]
        public string ThroughStaticMethod
        {
            get => throughStaticMethod;
            set
            {
                throughStaticMethod = value;
                OnPropertyChanged();
            }
        }

        [Text("Invalidate this property by marking the checkbox below.")]

        [Value(Must.BeInvalid, When = "{Binding InvalidateTextbox}")]
        public string ExternalValidation
        {
            get => externalValidation;
            set
            {
                externalValidation = value;
                OnPropertyChanged();
            }
        }

        public bool InvalidateTextbox
        {
            get => invalidateTextbox;
            set
            {
                invalidateTextbox = value;
                OnPropertyChanged();
            }
        }

        public void HandleAction(IActionContext actionContext)
        {
            switch (actionContext.Action)
            {
                case "invalidate":
                    ModelState.Invalidate(this, nameof(ThroughModelState), "Invalid value.");
                    break;
                case "validate":
                    ModelState.Validate(this, nameof(ThroughStaticMethod));
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
