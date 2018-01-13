namespace Material.Application.Infrastructure
{
    public interface IPaletteService
    {
        bool DarkMode { get; set; }

        string LightModePrimary { get; set; }

        string LightModeAccent { get; set; }

        string DarkModePrimary { get; set; }

        string DarkModeAccent { get; set; }

        void RefreshTheme();

        void RefreshPalette();
    }
}
