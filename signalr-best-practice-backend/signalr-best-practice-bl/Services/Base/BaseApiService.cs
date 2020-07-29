using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models.Base;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Interfaces.Entities;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using signalr_best_practice_core.Interfaces.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalr_best_practice_bl.Services.Base
{
    public abstract class BaseApiService<TModelAdd, TModelGet, TEntity, TKey> : IBaseApiService<TModelAdd, TModelGet, TEntity, TKey>
                where TEntity : BaseEntity<TKey>
                where TModelAdd : BaseApiModel<TKey>
                where TModelGet : BaseApiModel<TKey>
    {

        protected IBaseRepository<TEntity, TKey> _repository;
        protected IDataMapper _dataMapper;

        public BaseApiService(IBaseRepository<TEntity, TKey> repository, IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
            _repository = repository;
        }

        public virtual async Task<TKey> Add(TModelAdd model)
        {
            var entity = _dataMapper.Parse<TModelAdd, TEntity>(model);
            await _repository.Add(entity);
            return entity.Id;
        }

        public virtual async Task Delete(TKey id)
        {
            await _repository.Delete(id);
        }

        public virtual async Task<PaginationResponseApiModel<TModelGet>> Get(int start, int count, EntitySortingEnum sort)
        {
            var pagination = await _repository.Get(start, count, sort);

            var models = _dataMapper.ParseCollection<TEntity, TModelGet>(pagination.Entities);
            var result = new PaginationResponseApiModel<TModelGet>()
            {
                Total = pagination.Total,
                Start = pagination.Start,
                Count = pagination.Count,
                EntitySorting = pagination.EntitySorting,
                Models = models,
            };

            return result;
        }

        public virtual async Task<PaginationResponseApiModel<TModelGet>> Get(string userId, int start, int count, 
            EntitySortingEnum sort, string query = null, IEnumerable<TKey> ignoreIds = null)
        {
            List<Func<TEntity, bool>> filters = new List<Func<TEntity, bool>>();

            var type = typeof(TEntity);

            if (!string.IsNullOrEmpty(userId) && type.GetInterface(typeof(IUserEntity).Name) != null)
            {
                filters.Add(x => ((IUserEntity)x).UserId == userId);
            }
            if (!string.IsNullOrEmpty(query) && type.GetInterface(typeof(IBaseManualEntity<TKey>).Name) != null)
            {
                var q = query.ToLower();
                filters.Add(x => ((IBaseManualEntity<TKey>)x).Title != null
                    && ((IBaseManualEntity<TKey>)x).Title.ToLower().Contains(q));
            }
            if (ignoreIds != null && ignoreIds.Count() > 0)
            {
                filters.Add(x => !ignoreIds.Any(id => id.Equals(x.Id)));
            }

            var pagination = await _repository.Get(start, count, sort, filters.ToArray());

            var models = _dataMapper.ParseCollection<TEntity, TModelGet>(pagination.Entities);
            var result = new PaginationResponseApiModel<TModelGet>()
            {
                Total = pagination.Total,
                Start = pagination.Start,
                Count = pagination.Count,
                EntitySorting = pagination.EntitySorting,
                Models = models,
            };

            return result;
        }

        public virtual async Task<TModelGet> Get(TKey id)
        {
            var entity = await _repository.Get(id);
            var models = _dataMapper.Parse<TEntity, TModelGet>(entity);

            return models;
        }

        public virtual async Task Update(TModelAdd model)
        {
            var entity = _dataMapper.Parse<TModelAdd, TEntity>(model);
            await _repository.Update(entity);
        }

        public async Task ChangeEntityStatus(TKey id, EntityStatusEnum status)
        {
            await _repository.ChangeEntityStatus(id, status);
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
