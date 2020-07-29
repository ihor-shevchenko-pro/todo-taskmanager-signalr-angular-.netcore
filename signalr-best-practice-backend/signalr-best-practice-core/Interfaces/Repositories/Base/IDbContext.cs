using System.Collections.Generic;

namespace signalr_best_practice_core.Interfaces.Repositories.Base
{
    public interface IDbContext
    {
        ICollection<T> GetDataInstances<T>() where T : class, new();
        void SaveChanges<T>() where T : class, new();
    }
}
