using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.Controls.Internal
{
    internal class FormEnvironment : IEnvironment, INotifyPropertyChanged
    {
        private readonly HashSet<string> set;

        public FormEnvironment()
            : this(null)
        {
        }

        public FormEnvironment(IEnumerable<string> initialValues)
        {
            set = new HashSet<string>(initialValues ?? new string[0], StringComparer.OrdinalIgnoreCase);
        }

        [IndexerName("Item")]
        public bool this[string key] => key != null && set.Contains(key);

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Has(string key) => key != null && set.Contains(key);

        public bool Add(string value)
        {
            if (value == null)
            {
                return false;
            }

            if (!set.Contains(value))
            {
                set.Add(value);
                OnPropertyChanged("Item[]");
                return true;
            }

            return false;
        }

        public int Add(IEnumerable<string> values)
        {
            if (values == null)
            {
                return 0;
            }

            var added = 0;
            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                if (!set.Contains(value))
                {
                    set.Add(value);
                    added++;
                }
            }

            if (added != 0)
            {
                OnPropertyChanged("Item[]");
            }

            return added;
        }

        public bool Remove(string value)
        {
            if (value == null)
            {
                return false;
            }

            if (set.Contains(value))
            {
                set.Remove(value);
                OnPropertyChanged("Item[]");
                return true;
            }

            return false;
        }

        public int Remove(IEnumerable<string> values)
        {
            if (values == null)
            {
                return 0;
            }

            var removed = 0;
            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                if (set.Contains(value))
                {
                    set.Remove(value);
                    removed++;
                }
            }

            if (removed != 0)
            {
                OnPropertyChanged("Item[]");
            }

            return removed;
        }

        public void Clear()
        {
            if (set.Count != 0)
            {
                set.Clear();
                OnPropertyChanged("Item[]");
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}