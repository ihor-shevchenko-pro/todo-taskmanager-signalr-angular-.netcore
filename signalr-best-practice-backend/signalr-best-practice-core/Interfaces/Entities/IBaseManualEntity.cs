namespace signalr_best_practice_core.Interfaces.Entities
{
    public interface IBaseManualEntity<TKey> : IBaseEntity<TKey>
    {
        string Title { get; set; }
    }
}
