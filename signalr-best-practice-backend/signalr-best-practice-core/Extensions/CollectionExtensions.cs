using signalr_best_practice_core.Interfaces.Entities;
using System.Collections.Generic;
using System.Linq;

namespace signalr_best_practice_core.Extensions
{
    public static class CollectionExtensions
    {
        public static void SetIds<TEntity, TKey>(this ICollection<TEntity> collection, TKey id)
               where TEntity : IBaseEntity<TKey>
        {
            foreach (var item in collection)
            {
                item.Id = id;
            }
        }

        public static void SynchronizeCollection<TEntity>(this ICollection<TEntity> oldCollection, ICollection<TEntity> newCollection, string propertyName)
        {
            var toremove = oldCollection.Where(x => !newCollection.Any(n => n.CompareProperty(x, propertyName))).ToList();
            foreach (var item in toremove)
            {
                oldCollection.Remove(item);
            }
            foreach (var item in newCollection)
            {
                if (!oldCollection.Any(x => x.CompareProperty(item, propertyName)))
                {
                    oldCollection.Add(item);
                }
            }
        }

        public static void SynchronizeCollection<TEntity>(this ICollection<TEntity> oldCollection, ICollection<TEntity> newCollection)
            where TEntity : IBaseEntity<string>
        {
            var toremove = oldCollection.Where(x => !newCollection.Any(n => n.Id == x.Id)).ToList();

            foreach (var item in toremove)
            {
                oldCollection.Remove(item);
            }

            foreach (var item in newCollection)
            {
                var first = oldCollection.FirstOrDefault(x => x.Id == item.Id);

                if (first == null)
                {
                    oldCollection.Add(item);
                }
                else
                {
                    first.Copy(item);
                }
            }
        }
    }
}
