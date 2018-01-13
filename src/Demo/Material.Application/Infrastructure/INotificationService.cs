using System;

namespace Material.Application.Infrastructure
{
    public interface INotificationService
    {
        void Notify(string message);

        void Notify(string message, string actionLabel, Action action);

        void ForceNotify(string message);

        void ForceNotify(string message, string actionLabel, Action action);
    }
}
