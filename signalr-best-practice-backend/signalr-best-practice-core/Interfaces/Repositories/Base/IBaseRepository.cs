using signalr_best_practice_api_models;
using signalr_best_practice_core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Repositories.Base
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort);
        Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort, params Func<TEntity, bool>[] filters);
        Task<TEntity> Get(TKey id);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TKey id);
        Task Delete(TEntity item);
        Task DeleteAll(TKey id);

        Task<bool> AnyAsync(Func<TEntity, bool> search);
        Task<TEntity> FirstOrDefaultAsync(Func<TEntity, bool> search);
        Task<IEnumerable<TEntity>> WhereAsync(Func<TEntity, bool> search);

        Task<TValue> Max<TValue>(Func<TEntity, TValue> search);

        Task ChangeEntityStatus(TKey id, EntityStatusEnum status);
    }
}
