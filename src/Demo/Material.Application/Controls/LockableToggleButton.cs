using System.Windows;
using System.Windows.Controls.Primitives;

namespace Material.Application.Controls
{
    internal class LockableToggleButton : ToggleButton
    {
        public static readonly DependencyProperty LockToggleProperty =
            DependencyProperty.Register("LockToggle", typeof(bool), typeof(LockableToggleButton),
                new UIPropertyMetadata(false));

        public LockableToggleButton()
        {
            SetResourceReference(StyleProperty, typeof(ToggleButton));
        }

        public bool LockToggle
        {
            get { return (bool)GetValue(LockToggleProperty); }
            set { SetValue(LockToggleProperty, value); }
        }

        protected override void OnToggle()
        {
            if (!LockToggle)
            {
                base.OnToggle();
            }
        }
    }
}
