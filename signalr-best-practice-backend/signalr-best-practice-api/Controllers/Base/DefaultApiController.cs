using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models.Base;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Services.Base;

namespace signalr_best_practice_api.Controllers.Base
{
    [ApiController]
    public abstract class DefaultApiController<TModelAdd, TModelGet, TEntity, TKey> : BaseApiController
            where TEntity : BaseEntity<TKey>
            where TModelAdd : BaseApiModel<TKey>
            where TModelGet : BaseApiModel<TKey>, new ()
    {

        protected IBaseApiService<TModelAdd, TModelGet, TEntity, TKey> _service;
        protected IDataMapper _dataMapper;


        public DefaultApiController(IBaseApiService<TModelAdd, TModelGet, TEntity, TKey> service, IDataMapper dataMapper)
        {
            _service = service;
            _dataMapper = dataMapper;
        }


        [HttpPost("add")]
        public virtual async Task<ActionResult<SuccessResponseApiModel<TKey>>> Add([FromBody] TModelAdd model)
        {
            TKey id = await _service.Add(model);

            var modelResult = await _service.Get(id);
            await BroadcastMessageSignalR(NotificationTypeEnum.ModelAdd, modelResult, false, GetUserId());

            return SuccessResult(new SuccessResponseApiModel<TKey>() { Response = "success", Id = id });
        }

        [HttpPut("update/{id}")]
        public virtual async Task<ActionResult<SuccessResponseApiModel<TKey>>> Update(TKey id, [FromBody] TModelAdd model)
        {
            model.Id = model.Id ?? id;
            await _service.Update(model);

            var modelResult = await _service.Get(model.Id);
            await BroadcastMessageSignalR(NotificationTypeEnum.ModelUpdate, modelResult, false, GetUserId());

            return SuccessResult(new SuccessResponseApiModel<TKey>() { Response = "success", Id = model.Id });
        }

        [HttpGet("get_by_user")]
        public virtual async Task<ActionResult<PaginationResponseApiModel<TModelGet>>> Get(int start, int count,
            EntitySortingEnum sort, string query)
        {
            var userId = GetUserId();
            var models = await _service.Get(userId, start, count, sort, query);
            return SuccessResult(models);
        }

        [HttpGet("get")]
        public virtual async Task<ActionResult<PaginationResponseApiModel<TModelGet>>> Get(int start, int count,
            EntitySortingEnum sort)
        {
            var models = await _service.Get(start, count, sort);
            return SuccessResult(models);
        }

        [HttpGet("get/{id}")]
        public virtual async Task<ActionResult<TModelGet>> Get(TKey id)
        {
            var model = await _service.Get(id);
            return SuccessResult(model);
        }

        [HttpPut("status")]
        public async Task<ActionResult<string>> ChangeEntityStatus(BaseUpdateStatusApiModel<TKey> model)
        {
            await _service.ChangeEntityStatus(model.Id, model.Status);

            var modelResult = await _service.Get(model.Id);
            await BroadcastMessageSignalR(NotificationTypeEnum.ModelChangeStatus, modelResult, false, GetUserId());

            return SuccessResult();
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<ActionResult<string>> Delete(TKey id)
        {
            var modelResult = await _service.Get(id);

            await _service.Delete(id);

            await BroadcastMessageSignalR(NotificationTypeEnum.ModelDelete, modelResult, false, GetUserId());

            return SuccessResult();
        }
    }
}
