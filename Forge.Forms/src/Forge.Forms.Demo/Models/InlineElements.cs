using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    public class InlineElements
    {
        [Action("available", "CHECK AVAILABILITY", Inline = true)]
        public string Username { get; set; }

        [Text("Verify you're human", Inline = true)]
        [Action("listen", "LISTEN", Inline = true, Icon = PackIconKind.VolumeMedium)]
        [Action("refresh", "REFRESH", Inline = true, Icon = PackIconKind.Refresh)]
        public string Captcha { get; set; }
    }
}