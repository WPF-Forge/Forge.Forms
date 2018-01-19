using Forge.Forms.Controls;

namespace Forge.Forms
{
    public static class Show
    {
        public static object Window<T>(T model)
        {
            var window = new DialogWindow(model, WindowOptions.Default);

            window.ShowDialog();
            return null;
        }
    }
}
