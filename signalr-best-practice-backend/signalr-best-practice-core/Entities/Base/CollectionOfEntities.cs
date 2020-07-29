using signalr_best_practice_api_models;
using System.Collections.Generic;

namespace signalr_best_practice_core.Entities.Base
{
    public class CollectionOfEntities<T>
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public EntitySortingEnum EntitySorting { get; set; }
        public IEnumerable<T> Entities { get; set; }
    }
}
