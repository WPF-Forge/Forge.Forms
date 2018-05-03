using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    public class InlineElements
    {
        [Action("available", "CHECK AVAILABILITY", Placement = Placement.Inline)]
        public string Username { get; set; }

        [Text("Verify you're human", Placement = Placement.Inline)]
        [Action("listen", "LISTEN", Placement = Placement.Inline, Icon = PackIconKind.VolumeMedium)]
        [Action("refresh", "REFRESH", Placement = Placement.Inline, Icon = PackIconKind.Refresh)]
        public string Captcha { get; set; }
    }
}