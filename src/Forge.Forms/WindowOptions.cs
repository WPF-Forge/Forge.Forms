namespace Forge.Forms
{
    public class WindowOptions : DialogOptions
    {
        public static WindowOptions Default = new WindowOptions();

        private bool showMinButton;
        private bool showMaxRestoreButton = true;
        private bool showCloseButton;
        private bool canResize;

        public bool ShowMinButton
        {
            get => showMinButton;
            set
            {
                if (value == showMinButton) return;
                showMinButton = value;
                OnPropertyChanged();
            }
        }

        public bool ShowMaxRestoreButton
        {
            get => showMaxRestoreButton;
            set
            {
                if (value == showMaxRestoreButton) return;
                showMaxRestoreButton = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCloseButton
        {
            get => showCloseButton;
            set
            {
                if (value == showCloseButton) return;
                showCloseButton = value;
                OnPropertyChanged();
            }
        }

        public bool CanResize
        {
            get => canResize;
            set
            {
                if (value == canResize) return;
                canResize = value;
                OnPropertyChanged();
            }
        }
    }
}