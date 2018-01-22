using Forge.Forms.Controls;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms
{
    public static class Show
    {
        public static IModelHost Window()
        {
            return new WindowModelHost(null, WindowOptions.Default);
        }

        public static IModelHost Window(object context)
        {
            return new WindowModelHost(context, WindowOptions.Default);
        }

        public static IModelHost Window(WindowOptions options)
        {
            return new WindowModelHost(null, options);
        }

        public static IModelHost Window(double width)
        {
            return new WindowModelHost(null, new WindowOptions { Width = width });
        }

        public static IModelHost Window(object context, WindowOptions options)
        {
            return new WindowModelHost(context, options);
        }

        public static IModelHost Window(object context, double width)
        {
            return new WindowModelHost(context, new WindowOptions { Width = width });
        }

        public static IModelHost Dialog()
        {
            return new DialogModelHost(null, null, DialogOptions.Default);
        }

        public static IModelHost Dialog(DialogOptions options)
        {
            return new DialogModelHost(null, null, options);
        }

        public static IModelHost Dialog(double width)
        {
            return new DialogModelHost(null, null, new DialogOptions { Width = width });
        }

        public static IModelHost Dialog(object dialogIdentifier)
        {
            return new DialogModelHost(dialogIdentifier, null, DialogOptions.Default);
        }

        public static IModelHost Dialog(object dialogIdentifier, object context)
        {
            return new DialogModelHost(dialogIdentifier, context, DialogOptions.Default);
        }

        public static IModelHost Dialog(object dialogIdentifier, DialogOptions options)
        {
            return new DialogModelHost(dialogIdentifier, null, options);
        }

        public static IModelHost Dialog(object dialogIdentifier, double width)
        {
            return new DialogModelHost(dialogIdentifier, null, new DialogOptions { Width = width });
        }

        public static IModelHost Dialog(object dialogIdentifier, object context, DialogOptions options)
        {
            return new DialogModelHost(dialogIdentifier, context, options);
        }

        public static IModelHost Dialog(object dialogIdentifier, object context, double width)
        {
            return new DialogModelHost(dialogIdentifier, context, new DialogOptions { Width = width });
        }

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
            private readonly object context;
            private readonly object dialogIdentifier;
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
