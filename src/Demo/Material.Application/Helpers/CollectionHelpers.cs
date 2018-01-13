using System.Collections.Generic;

namespace Material.Application.Helpers
{
    public static class CollectionHelpers
    {
        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}
