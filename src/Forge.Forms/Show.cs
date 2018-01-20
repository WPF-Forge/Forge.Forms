using Forge.Forms.Controls;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms
{
    public static class Show
    {
        private class WindowModelHost : IModelHost
        {
            private readonly object context;
            private readonly WindowOptions options;

            public WindowModelHost(object context, WindowOptions options)
            {
                this.context = context;
                this.options = options;
            }

            public object For<T>(T model)
            {
                var window = new DialogWindow(model, context, options);
                window.ShowDialog();
                return null;
            }
        }

        private class DialogModelHost : IModelHost
        {
            private readonly object dialogIdentifier;
            private readonly object context;
            private readonly DialogOptions options;

            public DialogModelHost(object dialogIdentifier, object context, DialogOptions options)
            {
                this.context = context;
                this.options = options;
                this.dialogIdentifier = dialogIdentifier;
            }

            public object For<T>(T model)
            {
                var wrapper = new DynamicFormWrapper(model, context, options);
                return DialogHost.Show(wrapper, dialogIdentifier);
            }
        }

        public static IModelHost Window()
            => new WindowModelHost(null, WindowOptions.Default);

        public static IModelHost Window(object context)
            => new WindowModelHost(context, WindowOptions.Default);

        public static IModelHost Window(WindowOptions options)
            => new WindowModelHost(null, options);

        public static IModelHost Window(double width)
            => new WindowModelHost(null, new WindowOptions { Width = width });

        public static IModelHost Window(object context, WindowOptions options)
            => new WindowModelHost(context, options);

        public static IModelHost Window(object context, double width)
            => new WindowModelHost(context, new WindowOptions { Width = width });

        public static IModelHost Dialog()
            => new DialogModelHost(null, null, DialogOptions.Default);

        public static IModelHost Dialog(DialogOptions options)
            => new DialogModelHost(null, null, options);

        public static IModelHost Dialog(double width)
            => new DialogModelHost(null, null, new DialogOptions { Width = width });

        public static IModelHost Dialog(object dialogIdentifier)
            => new DialogModelHost(dialogIdentifier, null, DialogOptions.Default);

        public static IModelHost Dialog(object dialogIdentifier, object context)
            => new DialogModelHost(dialogIdentifier, context, DialogOptions.Default);

        public static IModelHost Dialog(object dialogIdentifier, DialogOptions options)
            => new DialogModelHost(dialogIdentifier, null, options);

        public static IModelHost Dialog(object dialogIdentifier, double width)
            => new DialogModelHost(dialogIdentifier, null, new DialogOptions { Width = width });

        public static IModelHost Dialog(object dialogIdentifier, object context, DialogOptions options)
            => new DialogModelHost(dialogIdentifier, context, options);

        public static IModelHost Dialog(object dialogIdentifier, object context, double width)
            => new DialogModelHost(dialogIdentifier, context, new DialogOptions { Width = width });
    }

    public interface IModelHost
    {
        object For<T>(T model);
    }

    public static class ModelHostExtensions
    {
        public static object For<T>(this IModelHost modelHost)
        {
            return modelHost.For(typeof(T));
        }
    }

    public class DialogResult<T>
    {
        public T Model { get; }

        public string Action { get; }

        public object ActionParameter { get; }
    }
}
