using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FancyGrid
{

    public class TestRow : INotifyPropertyChanged
    {
        private string name;
        public string String
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("String");
            }
        }

        private int orderCount;

        public int Int
        {
            get
            {
                return orderCount;
            }
            set
            {
                orderCount = value;
                NotifyPropertyChanged("Int");
            }
        }

        private double d;

        public double Double
        {
            get { return d; }
            set { d = value; NotifyPropertyChanged("Double"); }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public TestRow()
        {

        }
    }
}
