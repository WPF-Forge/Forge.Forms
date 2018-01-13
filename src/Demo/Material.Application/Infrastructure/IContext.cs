using System;

namespace Material.Application.Infrastructure
{
    public interface IContext
    {
        bool IsSynchronized { get; }

        void Invoke(Action action);
    }

    public static class ContextExtensions
    {
        public static void VerifyAccess(this IContext context)
        {
            if (!context.IsSynchronized)
            {
                throw new InvalidOperationException(
                    "The calling thread does not have access to this synchronization context.");
            }
        }
    }
}
