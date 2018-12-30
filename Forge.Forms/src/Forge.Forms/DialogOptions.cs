using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Forge.Forms.FormBuilding;

namespace Forge.Forms
{
    public class DialogOptions : INotifyPropertyChanged
    {
        public static DialogOptions Default = new DialogOptions();

        private double headingFontSize = 15d;
        private double height = double.NaN;
        private Thickness padding = new Thickness(16d, 16d, 16d, 8d);
        private double textFontSize = 15d;
        private double titleFontSize = 20d;
        private double width = 350d;
        private IFormBuilder formBuilder = FormBuilding.FormBuilder.Default;
        private HashSet<string> environmentFlags = new HashSet<string>
        {
            "DialogHostContext"
        };

        public DialogOptions()
            : this(Default)
        {
        }

        public DialogOptions(DialogOptions defaults)
        {
            if (defaults == null)
            {
                return;
            }

            width = defaults.width;
            height = defaults.height;
            padding = defaults.padding;
            titleFontSize = defaults.titleFontSize;
            headingFontSize = defaults.headingFontSize;
            textFontSize = defaults.textFontSize;
            formBuilder = defaults.formBuilder;
            environmentFlags = new HashSet<string>(defaults.environmentFlags);
        }

        public IFormBuilder FormBuilder
        {
            get => formBuilder;
            set
            {
                if (value == formBuilder)
                {
                    return;
                }

                formBuilder = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => width;
            set
            {
                if (value.Equals(width))
                {
                    return;
                }

                width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => height;
            set
            {
                if (value.Equals(height))
                {
                    return;
                }

                height = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the environment flags for the displayed form.
        /// </summary>
        public HashSet<string> EnvironmentFlags
        {
            get => environmentFlags;
            set
            {
                if (Equals(value, environmentFlags))
                {
                    return;
                }

                environmentFlags = value;
                OnPropertyChanged();
            }
        }

        public Thickness Padding
        {
            get => padding;
            set
            {
                if (value == padding)
                {
                    return;
                }

                padding = value;
                OnPropertyChanged();
            }
        }

        public double TitleFontSize
        {
            get => titleFontSize;
            set
            {
                if (value == titleFontSize)
                {
                    return;
                }

                titleFontSize = value;
                OnPropertyChanged();
            }
        }

        public double HeadingFontSize
        {
            get => headingFontSize;
            set
            {
                if (value == headingFontSize)
                {
                    return;
                }

                headingFontSize = value;
                OnPropertyChanged();
            }
        }

        public double TextFontSize
        {
            get => textFontSize;
            set
            {
                if (value == textFontSize)
                {
                    return;
                }

                textFontSize = value;
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
