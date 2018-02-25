using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Controls
{
    public interface IFormResourceContext
    {
        DynamicForm Form { get; }
        string BasePath { get; }
        IEnvironment Environment { get; }
        object GetModelInstance();
        BindingExpressionBase[] GetBindings();
        object GetContextInstance();
        Binding CreateDirectModelBinding();
        Binding CreateModelBinding(string path);
        Binding CreateContextBinding(string path);
        object TryFindResource(object key);
        object FindResource(object key);
        FrameworkElement GetOwningElement();
    }
}