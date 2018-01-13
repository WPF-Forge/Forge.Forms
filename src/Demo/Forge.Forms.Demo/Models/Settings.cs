using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using Forge.Forms.Annotations.Content;
using Forge.Forms.Annotations.Display;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    public class Settings : INotifyPropertyChanged
    {
        private bool bluetooth;
        private string deviceName;

        private bool facebook;
        private string hotspotName;
        private bool instagram;
        private bool mobileData;
        private bool personalHotspot;
        private bool sendAnonymousData;
        private bool twitter;
        private double volume;
        private bool wiFi;

        [Title("Settings")]
        [Heading("Connectivity", Icon = "Signal")]
        [Field(Name = "Wi-Fi", Icon = "Wifi")]
        [Toggle]
        public bool WiFi
        {
            get => wiFi;
            set
            {
                wiFi = value;
                OnPropertyChanged();
            }
        }

        [Field(Icon = "Signal")]
        [Toggle]
        public bool MobileData
        {
            get => mobileData;
            set
            {
                mobileData = value;
                OnPropertyChanged();
            }
        }

        [Field(Icon = "AccessPoint")]
        [Toggle]
        public bool PersonalHotspot
        {
            get => personalHotspot;
            set
            {
                personalHotspot = value;
                OnPropertyChanged();
            }
        }

        [Field(IsVisible = "{Binding PersonalHotspot}")]
        public string HotspotName
        {
            get => hotspotName;
            set
            {
                hotspotName = value;
                OnPropertyChanged();
            }
        }

        [Field(Icon = "Bluetooth")]
        [Toggle]
        public bool Bluetooth
        {
            get => bluetooth;
            set
            {
                bluetooth = value;
                OnPropertyChanged();
            }
        }

        [Divider]
        [Heading("Notifications", Icon = PackIconKind.MessageOutline)]
        [Field(Icon = "Twitter")]
        [Toggle]
        public bool Twitter
        {
            get => twitter;
            set
            {
                twitter = value;
                OnPropertyChanged();
            }
        }

        [Field(Icon = "Facebook")]
        [Toggle]
        public bool Facebook
        {
            get => facebook;
            set
            {
                facebook = value;
                OnPropertyChanged();
            }
        }

        [Field(Icon = "Instagram")]
        [Toggle]
        public bool Instagram
        {
            get => instagram;
            set
            {
                instagram = value;
                OnPropertyChanged();
            }
        }

        [Divider]
        [Heading("Device", Icon = PackIconKind.Cellphone)]

        public string DeviceName
        {
            get => deviceName;
            set
            {
                deviceName = value;
                OnPropertyChanged();
            }
        }

        [Slider(Minimum = 0d, Maximum = 100d)]
        [Field(Icon = "{Binding VolumeIcon}")]
        public double Volume
        {
            get => volume;
            set
            {
                volume = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VolumeIcon));
            }
        }

        public PackIconKind VolumeIcon
        {
            get
            {
                if (volume <= 0d)
                {
                    return PackIconKind.VolumeOff;
                }

                if (volume < 33d)
                {
                    return PackIconKind.VolumeLow;
                }

                if (volume > 66d)
                {
                    return PackIconKind.VolumeHigh;
                }

                return PackIconKind.VolumeMedium;
            }
        }

        [Toggle]
        public bool SendAnonymousData
        {
            get => sendAnonymousData;
            set
            {
                sendAnonymousData = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
