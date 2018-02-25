using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Controls
{
    internal class FormResourceContext : IFrameworkResourceContext, INotifyPropertyChanged, IFormResourceContext
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

        public IEnvironment Environment => Form.Environment;

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

        public void OnAction(IActionContext actionContext)
        {
            Form.RaiseOnAction(actionContext);
        }

        // Although never invoked, it may provide a performance benefit to include INPC.
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
