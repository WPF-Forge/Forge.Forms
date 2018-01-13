using System.Collections;
using System.Reflection;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Material.Application.MaterialDesign
{
    public static partial class MaterialDesignHelper
    {
        public static void ReplaceDefaultHintProxies()
        {
            var list = (IList)typeof(HintProxyFabric).GetField("Builders", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            list.RemoveAt(2);
            list.RemoveAt(1);
            HintProxyFabric.RegisterBuilder(c => c is TextBox, c => new CustomTextBoxHintProxy((TextBox)c));
            HintProxyFabric.RegisterBuilder(c => c is PasswordBox, c => new CustomPasswordBoxHintProxy((PasswordBox)c));
        }
    }
}
