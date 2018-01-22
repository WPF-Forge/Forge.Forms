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
    }

    internal class ActionContext : IActionContext
    {
        public ActionContext(object model, object context, object action, object actionParameter)
        {
            Model = model;
            Context = context;
            Action = action;
            ActionParameter = actionParameter;
        }

        public object Model { get; }

        public object Context { get; }

        public object Action { get; }

        public object ActionParameter { get; }
    }
}
