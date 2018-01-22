namespace Forge.Forms.Base
{
    public abstract class DialogBase : FormBase
    {
        private bool confirmed;
        private string message;
        private string negativeAction = "CANCEL";
        private string positiveAction = "OK";
        private string title;

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

        protected override void OnAction(object action, object parameter)
        {
            if (action is "positive")
            {
                Confirmed = true;
            }
        }
    }
}
