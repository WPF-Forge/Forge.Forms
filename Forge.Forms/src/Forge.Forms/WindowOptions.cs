﻿namespace Forge.Forms
{
    public class WindowOptions : DialogOptions
    {
        public new static WindowOptions Default = new WindowOptions();
        private bool canResize;
        private bool showCloseButton;
        private bool showMaxRestoreButton = true;
        private bool showMinButton;

        private string title = "Dialog";

        public WindowOptions()
            : this(Default)
        {
        }

        public WindowOptions(WindowOptions defaults)
            : base(defaults)
        {
            if (defaults == null)
            {
                return;
            }

            title = defaults.title;
            showMinButton = defaults.showMinButton;
            showMaxRestoreButton = defaults.showMaxRestoreButton;
            showCloseButton = defaults.showCloseButton;
            canResize = defaults.canResize;
        }

        public bool TopMost { get;set; }

        public bool BringToFront { get; set; }

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

        public bool ShowMinButton
        {
            get => showMinButton;
            set
            {
                if (value == showMinButton)
                {
                    return;
                }

                showMinButton = value;
                OnPropertyChanged();
            }
        }

        public bool ShowMaxRestoreButton
        {
            get => showMaxRestoreButton;
            set
            {
                if (value == showMaxRestoreButton)
                {
                    return;
                }

                showMaxRestoreButton = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCloseButton
        {
            get => showCloseButton;
            set
            {
                if (value == showCloseButton)
                {
                    return;
                }

                showCloseButton = value;
                OnPropertyChanged();
            }
        }

        public bool CanResize
        {
            get => canResize;
            set
            {
                if (value == canResize)
                {
                    return;
                }

                canResize = value;
                OnPropertyChanged();
            }
        }
    }
}
