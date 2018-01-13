using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Material.Application.Controls
{
    public class SortablePageableCollection<T> : PageableCollection<T>, ISortable
    {
        private readonly Dictionary<string, Func<T, object>> sortCache;

        public SortablePageableCollection(IEnumerable<T> allObjects, Dictionary<string, Func<T, object>> sortCache, int? entriesPerPage = null)
            : base(allObjects, entriesPerPage)
        {
            this.sortCache = sortCache ?? new Dictionary<string, Func<T, object>>();
        }

        public void Sort(string propertyName, string direction)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                CurrentPageNumber = 1;
                Calculate(CurrentPageNumber);
                return;
            }

            Func<T, object> propertyGetter;
            if (!sortCache.TryGetValue(propertyName, out propertyGetter))
            {
                var prop = typeof(T).GetProperty(propertyName);
                propertyGetter = obj => prop.GetValue(obj, null);
                if (prop == null)
                {
                    CurrentPageNumber = 1;
                    Calculate(CurrentPageNumber);
                    return;
                }
            }

            if (string.IsNullOrEmpty(direction) || direction.ToLower() == "descending")
            {
                AllObjects = new ObservableCollection<T>(AllObjects.OrderByDescending(propertyGetter));
            }
            else
            {
                AllObjects = new ObservableCollection<T>(AllObjects.OrderBy(propertyGetter));
            }

            CurrentPageNumber = 1;
            Calculate(CurrentPageNumber);
        }
    }
}