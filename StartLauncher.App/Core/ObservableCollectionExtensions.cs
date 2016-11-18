using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace StartLauncher.App.Core
{
    public static class ObservableCollectionExtensions
    {
        public static int Remove<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
                coll.Remove(itemToRemove);

            return itemsToRemove.Count;
        }
    }
}