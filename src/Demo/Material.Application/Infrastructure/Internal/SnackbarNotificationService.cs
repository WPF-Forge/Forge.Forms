using System;
using MaterialDesignThemes.Wpf;

namespace Material.Application.Infrastructure
{
    internal class SnackbarNotificationService : INotificationService
    {
        private readonly ISnackbarMessageQueue snackbarMessageQueue;
        private readonly IMainWindowLocator windowLocator;

        private string cacheBreaker = string.Empty;

        public SnackbarNotificationService(ISnackbarMessageQueue snackbarMessageQueue, IMainWindowLocator windowLocator)
        {
            this.snackbarMessageQueue = snackbarMessageQueue;
            this.windowLocator = windowLocator;
        }

        private string CacheBreaker
        {
            get
            {
                if (cacheBreaker.Length > 3)
                {
                    cacheBreaker = string.Empty;
                }

                return cacheBreaker += "\0";
            }
        }

        public void Notify(string message)
        {
            if (windowLocator.GetMainWindow() == null)
            {
                return;
            }

            snackbarMessageQueue.Enqueue(message);
        }

        public void Notify(string message, string actionLabel, Action action)
        {
            if (windowLocator.GetMainWindow() == null)
            {
                return;
            }

            snackbarMessageQueue.Enqueue(message, actionLabel, action);
        }

        public void ForceNotify(string message)
        {
            if (windowLocator.GetMainWindow() == null)
            {
                return;
            }

            snackbarMessageQueue.Enqueue(message + CacheBreaker, true);
        }

        public void ForceNotify(string message, string actionLabel, Action action)
        {
            if (windowLocator.GetMainWindow() == null)
            {
                return;
            }

            snackbarMessageQueue.Enqueue(message + CacheBreaker, actionLabel, action);
        }
    }
}
