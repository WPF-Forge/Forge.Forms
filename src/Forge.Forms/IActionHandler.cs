namespace Forge.Forms
{
    public interface IActionHandler
    {
        void HandleAction(object model, string action, object parameter);
    }
}
