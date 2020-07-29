using signalr_best_practice_api_models;
using signalr_best_practice_core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Repositories.Base
{
    public interface IGenericRepository<TEntity, TKey>
            where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetById(TKey id);
        Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort);
        Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort, params Func<TEntity, bool>[] filter);


        Task Insert(TEntity entity);
        Task Insert(IEnumerable<TEntity> entities);


        Task Update(TEntity entity, Expression<Func<TEntity, bool>> search = null);
        Task Update(IEnumerable<TEntity> entities);


        Task Delete(TKey id);
        Task Delete(TEntity entity);
        Task Delete(IEnumerable<TEntity> entities);


        Task<bool> AnyAsync(Func<TEntity, bool> search);
        Task<TEntity> FirstOrDefaultAsync(Func<TEntity, bool> search);
        Task<IEnumerable<TEntity>> WhereAsync(Func<TEntity, bool> search);

        Task<TValue> Max<TValue>(Func<TEntity, TValue> search);
    }
}
