using MaterialDesignThemes.Wpf;

namespace Forge.Forms
{
    /// <summary>
    /// Base class for common dialogs.
    /// </summary>
    public abstract class DialogBase : FormBase
    {
        private bool confirmed;
        private string message;
        private string positiveAction = "OK";
        private string negativeAction = "CANCEL";
        private PackIconKind? positiveActionIcon;
        private PackIconKind? negativeActionIcon;

        private string title;

        /// <summary>
        /// Gets or sets the title of the dialog.
        /// Assigning null or an empty string hides this element.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if (value == title)
                {
                    return;
                }

                title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the message of the dialog.
        /// Assigning null or an empty string hides this element.
        /// </summary>
        public string Message
        {
            get => message;
            set
            {
                if (value == message)
                {
                    return;
                }

                message = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the positive action content.
        /// Assigning null or an empty string hides this element.
        /// </summary>
        public string PositiveAction
        {
            get => positiveAction;
            set
            {
                if (value == positiveAction)
                {
                    return;
                }

                positiveAction = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the positive action icon.
        /// Assigning null will hide the icon.
        /// </summary>
        public PackIconKind? PositiveActionIcon
        {
            get => positiveActionIcon;
            set
            {
                positiveActionIcon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the negative action content.
        /// Assigning null or an empty string hides this element.
        /// </summary>
        public string NegativeAction
        {
            get => negativeAction;
            set
            {
                if (value == negativeAction)
                {
                    return;
                }

                negativeAction = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the negative action icon.
        /// Assigning null will hide the icon.
        /// </summary>
        public PackIconKind? NegativeActionIcon
        {
            get => negativeActionIcon;
            set
            {
                negativeActionIcon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns true if the positive action has been clicked.
        /// </summary>
        public bool Confirmed
        {
            get => confirmed;
            private set
            {
                if (value == confirmed)
                {
                    return;
                }

                confirmed = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        protected override void OnAction(object action, object parameter)
        {
            if (action is "positive")
            {
                Confirmed = true;
            }
        }
    }
}
