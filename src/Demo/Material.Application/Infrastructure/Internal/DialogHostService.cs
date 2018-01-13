using System;
using Material.Application.Helpers;
using MaterialForms;

namespace Material.Application.Infrastructure
{
    internal class DialogHostService : IDialogService
    {
        private readonly IMainWindowLocator windowLocator;

        public DialogHostService(IMainWindowLocator windowLocator)
        {
            this.windowLocator = windowLocator;
        }

        public DialogSession ShowTrackedDialog(MaterialDialog dialog, double width)
        {
            var window = windowLocator.GetMainWindow();
            if (window == null)
            {
                throw new InvalidOperationException(ErrorMessages.MainWindowNotFound);
            }

            return dialog.ShowTracked(window.RootDialog.Identifier.ToString(), width);
        }

        public void CloseDialogs()
        {
            var window = windowLocator.GetMainWindow();
            if (window == null)
            {
                return;
            }

            window.RootDialog.IsOpen = false;
        }
    }
}
