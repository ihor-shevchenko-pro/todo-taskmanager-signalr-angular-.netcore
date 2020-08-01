using Microsoft.AspNetCore.Mvc;
using signalr_best_practice_api.Controllers.Base;
using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Services;
using System.Threading.Tasks;

namespace signalr_best_practice_api.Controllers
{
    [Route("api/[controller]")]
    public class ToDoTaskController : DefaultApiController<ToDoTaskAddApiModel, ToDoTaskGetFullApiModel, ToDoTask, string>
    {
        new IToDoTaskService _service;

        public ToDoTaskController(IToDoTaskService service, IDataMapper dataMapper) : base(service, dataMapper)
        {
            _service = service;
        }

        [HttpPost("add")]
        public override async Task<ActionResult<SuccessResponseApiModel<string>>> Add([FromBody] ToDoTaskAddApiModel model)
        {
            if (model.FromUserId == null) model.FromUserId = GetUserId();

            string id = await _service.Add(model);

            var modelResult = await _service.Get(id);
            await BroadcastMessageSignalR(NotificationTypeEnum.ModelAdd, modelResult, false, model.ToUserId);

            return SuccessResult(new SuccessResponseApiModel<string>() { Response = "success", Id = id });
        }

        [HttpPost("add_for_all")]
        public async Task<ActionResult<SuccessResponseApiModel<string>>> AddAllUsers([FromBody] ToDoTaskAddApiModel model)
        {
            if (model.FromUserId == null) model.FromUserId = GetUserId();

            var models = await _service.AddForAll(model);
            if(models != null && models.Count > 0)
            {
                foreach (var item in models)
                {
                    await BroadcastMessageSignalR(NotificationTypeEnum.ModelAdd, item, false, item.ToUserId);
                }
            }

            return SuccessResult(new SuccessResponseApiModel<string>() { Response = "success" });
        }

        [HttpGet("sent_todotasks")]
        public async Task<ActionResult<PaginationResponseApiModel<ToDoTaskGetFullApiModel>>> GetSentToDoTasks(int start, int count,
            EntitySortingEnum sort)
        {
            var userId = GetUserId();
            var models = await _service.GetSentToDoTasks(start, count, sort, userId);
            return SuccessResult(models);
        }

        [HttpGet("received_todotasks")]
        public async Task<ActionResult<PaginationResponseApiModel<ToDoTaskGetFullApiModel>>> GetReceivedToDoTasks(int start, int count,
            EntitySortingEnum sort)
        {
            var userId = GetUserId();
            var models = await _service.GetReceivedToDoTasks(start, count, sort, userId);
            return SuccessResult(models);
        }
    
        [HttpPut("progress_status/{id}")]
        public async Task<ActionResult<SuccessResponseApiModel<string>>> ChangeProgressStatus(string id, [FromBody] ToDoTaskChangeProgressStatusApiModel model)
        {
            model.Id = model.Id ?? id;
            var userId = GetUserId();
            await _service.ChangeProgressStatus(userId, model);

            var taskResult = await _service.Get(model.Id);
            if(model.ProgressStatus == ToDoTaskStatusEnum.Cancelled)
            {
                await BroadcastMessageSignalR(NotificationTypeEnum.ChangeProgressStatus, taskResult, false, taskResult.ToUserId);
            }
            else
            {
                await BroadcastMessageSignalR(NotificationTypeEnum.ChangeProgressStatus, taskResult, false, taskResult.FromUserId);
            }

            return SuccessResult(new SuccessResponseApiModel<string>() { Response = "success", Id = model.Id });
        }
    }
}