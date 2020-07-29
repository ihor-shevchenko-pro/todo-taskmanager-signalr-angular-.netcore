using System;

namespace signalr_best_practice_core.Interfaces.Entities
{
    public interface IBaseEntity<TKey> : IBaseEntity
    {
        TKey Id { get; set; }
    }

    public interface IBaseEntity
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
