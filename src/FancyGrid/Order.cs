using System.ComponentModel;

namespace FancyGrid
{
    public class TestRow : INotifyPropertyChanged
    {
        private double d;
        private string name;

        private int orderCount;


        public event PropertyChangedEventHandler PropertyChanged;

        public double Double
        {
            get => d;
            set
            {
                d = value;
                NotifyPropertyChanged("Double");
            }
        }

        public int Int
        {
            get => orderCount;
            set
            {
                orderCount = value;
                NotifyPropertyChanged("Int");
            }
        }

        public string String
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged("String");
            }
        }


        /// <summary>
        /// Raise the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
