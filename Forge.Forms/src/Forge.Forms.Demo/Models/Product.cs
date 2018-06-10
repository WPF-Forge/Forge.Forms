using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.Demo.Models
{
    public class Product : INotifyPropertyChanged
    {
        private double priceBought;
        private double price;

        public string Name { get; set; }

        public double PriceBought
        {
            get { return this.priceBought; }
            set
            {
                if (Nullable.Equals<double>(this.priceBought, value))
                    return;
                this.priceBought = value;
                this.OnPropertyChanged(nameof(Gain));
                this.OnPropertyChanged();
            }
        }

        public double Gain
        {
            get
            {
                if (Price > 0 && PriceBought > 0)
                    return (Price - PriceBought) / PriceBought;
                return 0;
            }
            set
            {
                Price = PriceBought + PriceBought * (value / 100);
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceBought));
                OnPropertyChanged(nameof(Gain));
            }
        }

        public double Price
        {
            get => price;
            set
            {
                price = value;
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