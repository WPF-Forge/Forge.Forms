using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Forge.Forms.Collections
{
    public class CrudCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private List<T> InternalCollection { get; } = new List<T>();

        public CrudCollection()
        {
        }

        public CrudCollection(IEnumerable<T> enumerable)
        {
            InternalCollection = enumerable.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InternalCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            InternalCollection.Add(item);

            if (item is INotifyPropertyChanged itemNotifyPropertyChanged)
            {
                itemNotifyPropertyChanged.PropertyChanged += ItemNotifyPropertyChangedOnPropertyChanged;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        private void ItemNotifyPropertyChangedOnPropertyChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
        }

        public void Clear()
        {
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            InternalCollection.Clear();
        }

        public bool Contains(T item)
        {
            return InternalCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            InternalCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (item is INotifyPropertyChanged itemNotifyPropertyChanged)
            {
                itemNotifyPropertyChanged.PropertyChanged -= ItemNotifyPropertyChangedOnPropertyChanged;
            }

            var removeResult = InternalCollection.Remove(item);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return removeResult;
        }

        public int Count => InternalCollection.Count;
        public bool IsReadOnly { get; }

        public int IndexOf(T item)
        {
            return InternalCollection.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            InternalCollection.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public void RemoveAt(int index)
        {
            var item = InternalCollection[index];
            Remove(item);
        }

        public T this[int index]
        {
            get => InternalCollection[index];
            set => InternalCollection[index] = value;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}