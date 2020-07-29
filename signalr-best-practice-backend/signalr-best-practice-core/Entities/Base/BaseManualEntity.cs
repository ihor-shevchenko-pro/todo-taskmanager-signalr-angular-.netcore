using signalr_best_practice_core.Interfaces.Entities;

namespace signalr_best_practice_core.Entities.Base
{
    public abstract class BaseManualEntity<TKey> : BaseEntity<TKey>, IBaseManualEntity<TKey>
    {
        public string Title { get; set; }
    }
}
