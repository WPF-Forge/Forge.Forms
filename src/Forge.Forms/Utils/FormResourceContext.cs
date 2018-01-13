using System.Windows;
using System.Windows.Data;
using Forge.Forms.Components.Controls;
using Forge.Forms.Interfaces;

namespace Forge.Forms.Utils
{
    internal class FormResourceContext : IFrameworkResourceContext
    {
        public FormResourceContext(DynamicForm form)
            : this(form, null)
        {
        }

        public FormResourceContext(DynamicForm form, string basePath)
        {
            Form = form;
            BasePath = basePath;
        }

        public DynamicForm Form { get; }

        public string BasePath { get; }

        public object GetModelInstance()
        {
            return Form.Value;
        }

        public BindingExpressionBase[] GetBindings()
        {
            return ModelState.GetBindings(Form.Value);
        }

        public object GetContextInstance()
        {
            return Form.Context;
        }

        public Binding CreateDirectModelBinding()
        {
            return new Binding(nameof(Form.Model) + BasePath)
            {
                Source = Form
            };
        }

        public Binding CreateModelBinding(string path)
        {
            return new Binding(nameof(Form.Value) + BasePath + Resource.FormatPath(path))
            {
                Source = Form
            };
        }

        public Binding CreateContextBinding(string path)
        {
            return new Binding(nameof(Form.Context) + BasePath + Resource.FormatPath(path))
            {
                Source = Form
            };
        }

        public object TryFindResource(object key)
        {
            return Form.TryFindResource(key);
        }

        public object FindResource(object key)
        {
            return Form.FindResource(key);
        }

        public void AddResource(object key, object value)
        {
            Form.Resources.Add(key, value);
        }

        public FrameworkElement GetOwningElement()
        {
            return Form;
        }

        public void OnAction(object model, string action, object parameter)
        {
            Form.RaiseOnAction(model, action, parameter);
        }
    }
}
