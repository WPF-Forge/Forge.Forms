namespace Forge.Forms
{
    public interface IActionHandler
    {
        void HandleAction(object model, string action, object parameter);
    }

    public interface IActionInterceptor
    {
        void InterceptAction(object model, object context, object parameter);
    }
}
