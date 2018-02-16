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
    }

    public class ActionContext : IActionContext
    {
        public ActionContext(object model, object context, object action, object actionParameter, IResourceContext resourceContext)
        {
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
    }
}
