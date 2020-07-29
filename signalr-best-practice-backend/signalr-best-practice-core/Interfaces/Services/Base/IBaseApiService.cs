using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models.Base;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_core.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Services.Base
{
    public interface IBaseApiService<TModelAdd, TModelGet, TEntity, TKey>
            where TEntity : BaseEntity<TKey>
            where TModelAdd : BaseApiModel<TKey>
            where TModelGet : BaseApiModel<TKey>
    {
        Task<TModelGet> Get(TKey id);
        Task<PaginationResponseApiModel<TModelGet>> Get(int start, int count, EntitySortingEnum sort);
        Task<PaginationResponseApiModel<TModelGet>> Get(string userId, int start, int count, EntitySortingEnum sort,
            string query = null, IEnumerable<TKey> ignoreIds = null);

        Task<TKey> Add(TModelAdd model);

        Task Update(TModelAdd model);

        Task Delete(TKey id);

        Task ChangeEntityStatus(TKey id, EntityStatusEnum status);
    }
}
