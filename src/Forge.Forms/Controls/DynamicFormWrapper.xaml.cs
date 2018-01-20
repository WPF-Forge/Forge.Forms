using System.Windows.Controls;

namespace Forge.Forms.Controls
{
    /// <summary>
    /// Interaction logic for FormWrapper.xaml
    /// </summary>
    internal partial class DynamicFormWrapper : UserControl
    {
        public DynamicFormWrapper(object model, object context, DialogOptions options)
        {
            DataContext = options;
            InitializeComponent();
            Form.Context = context;
            Form.Model = model;
        }
    }
}
