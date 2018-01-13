using MaterialDesignThemes.Wpf;

namespace Material.Application.Infrastructure
{
    internal class PaletteService : IPaletteService
    {
        public bool DarkMode { get; set; }

        public string LightModePrimary { get; set; }

        public string LightModeAccent { get; set; }

        public string DarkModePrimary { get; set; }

        public string DarkModeAccent { get; set; }

        public void RefreshTheme()
        {
            new PaletteHelper().SetLightDark(DarkMode);
        }

        public void RefreshPalette()
        {
            var paletteHelper = new PaletteHelper();
            if (DarkMode)
            {
                if (DarkModePrimary != null)
                {
                    paletteHelper.ReplacePrimaryColor(DarkModePrimary);
                }

                if (DarkModeAccent != null)
                {
                    paletteHelper.ReplaceAccentColor(DarkModeAccent);
                }
            }
            else
            {
                if (LightModePrimary != null)
                {
                    paletteHelper.ReplacePrimaryColor(LightModePrimary);
                }

                if (LightModeAccent != null)
                {
                    paletteHelper.ReplaceAccentColor(LightModeAccent);
                }
            }
        }
    }
}
