using System;
using Forge.Forms.FormBuilding;

namespace Forge.Forms
{
    public interface IActionHandler
    {
        void HandleAction(IActionContext actionContext);
    }

    public interface IActionInterceptor
    {
        IActionContext InterceptAction(IActionContext actionContext);
    }

    public interface IActionContext
    {
        object Model { get; }

        object Context { get; }

        object Action { get; }

        object ActionParameter { get; }

        IResourceContext ResourceContext { get; }

        bool CloseFormHost();
    }

    public class ActionContext : IActionContext
    {
        private readonly Func<bool> close;

        public ActionContext(object model, object context, object action, object actionParameter, IResourceContext resourceContext, Func<bool> close)
        {
            this.close = close;
            Model = model;
            Context = context;
            Action = action;
            ActionParameter = actionParameter;
            ResourceContext = resourceContext;
        }

        public object Model { get; }

        public object Context { get; }

        public object Action { get; }

        public object ActionParameter { get; }

        public IResourceContext ResourceContext { get; }

        public bool CloseFormHost() => close();
    }
}
