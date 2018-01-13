using System;
using System.Threading.Tasks;
using Material.Application.Routing;
using MaterialForms;

namespace Material.Application.Infrastructure
{
    public interface IDialogService
    {
        DialogSession ShowTrackedDialog(MaterialDialog dialog, double width);

        void CloseDialogs();
    }


    public static class DialogServiceExtensions
    {
        public static async Task Alert(this IDialogService dialogService, string message)
        {
            try
            {
                await dialogService.ShowTrackedDialog(DialogFactory.Alert(message), Route.SmallDialog).Task;
            }
            catch
            {
                // ignored
            }
        }

        public static Task<bool?> ShowDialog(this IDialogService dialogService, MaterialDialog dialog)
            => ShowDialog(dialogService, dialog, Route.SmallDialog);

        public static Task<bool?> ShowDialog(this IDialogService dialogService, MaterialDialog dialog, double width)
            => dialogService.ShowTrackedDialog(dialog, width).Task;

        public static async Task RunTask(this IDialogService dialogService, Func<Task> task, string message)
        {
            var dialog = new MaterialDialog
            {
                Message = message,
                PositiveAction = null,
                NegativeAction = null,
                Form = new MaterialForm
                {
                    new ProgressSchema
                    {
                        IsIndeterminate = true,
                        ShowPercentage = false
                    }
                }
            };

            Session session = null;
            try
            {
                session = dialogService.ShowTrackedDialog(dialog, Route.LargeDialog);
                await Task.Run(task);
            }
            finally
            {
                session?.Close(true);
            }
        }
    }
}
