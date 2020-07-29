using signalr_best_practice_api_models;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalr_best_practice_dl.Repositories.Base
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
                where TEntity : BaseEntity<TKey>
    {
        protected IGenericRepository<TEntity, TKey> _repository;

        public BaseRepository(IGenericRepository<TEntity, TKey> repository)
        {
            _repository = repository;
        }

        public virtual async Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort)
        {
            return await _repository.Get(start, count, sort);
        }

        public async Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort, params Func<TEntity, bool>[] filters)
        {
            return await _repository.Get(start, count, sort, filters);
        }

        public virtual async Task<TEntity> Get(TKey id)
        {
            return await _repository.GetById(id);
        }

        public virtual async Task Add(TEntity entity)
        {
            await _repository.Insert(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await _repository.Update(entity);
        }

        public virtual async Task Delete(TKey id)
        {
            await _repository.Delete(id);
        }

        public async Task Delete(TEntity item)
        {
            await _repository.Delete(item);
        }

        public virtual async Task<bool> AnyAsync(Func<TEntity, bool> search)
        {
            return await _repository.AnyAsync(search);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Func<TEntity, bool> search)
        {
            return await _repository.FirstOrDefaultAsync(search);
        }

        public virtual async Task<IEnumerable<TEntity>> WhereAsync(Func<TEntity, bool> search)
        {
            return await _repository.WhereAsync(search);
        }

        public async Task<TValue> Max<TValue>(Func<TEntity, TValue> search)
        {
            return await _repository.Max(search);
        }

        public async Task DeleteAll(TKey id)
        {
            var entities = await _repository.WhereAsync(e => e.Id.Equals(id));
            foreach (var entity in entities.ToList())
            {
                await _repository.Delete(id);
            }
        }

        public async Task ChangeEntityStatus(TKey id, EntityStatusEnum status)
        {
            var entity = await _repository.GetById(id);
            entity.Status = status;
            await _repository.Update(entity);
        }

        protected IOrderedEnumerable<TEntity> GetSorting(IEnumerable<TEntity> entities, EntitySortingEnum sort)
        {
            switch (sort)
            {
                case EntitySortingEnum.ByCreate:
                    return entities.OrderByDescending(x => x.Created);

                case EntitySortingEnum.ByUpdate:
                    return entities.OrderByDescending(x => x.Updated);

                default:
                    throw new ArgumentException($"Sorting type {sort} is not valid.");
            }
        }
    }
}
