using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Forge.Forms.Collections
{
    public class CrudCollection : ObservableCollection<object>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public CrudCollection()
        {
        }

        public CrudCollection(IEnumerable enumerable) : base(enumerable.OfType<object>().ToList())
        {
        }

        public new void Add(object item)
        {
            Add(item);

            if (item is INotifyPropertyChanged itemNotifyPropertyChanged)
            {
                itemNotifyPropertyChanged.PropertyChanged += ItemNotifyPropertyChangedOnPropertyChanged;
            }
        }

        private void ItemNotifyPropertyChangedOnPropertyChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

    }
}