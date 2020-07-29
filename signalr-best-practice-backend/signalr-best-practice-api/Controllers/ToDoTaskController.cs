using Microsoft.AspNetCore.Mvc;
using signalr_best_practice_api.Controllers.Base;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_bl.Services;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Managers;

namespace signalr_best_practice_api.Controllers
{
    [Route("api/[controller]")]
    public class ToDoTaskController : DefaultApiController<ToDoTaskAddApiModel, ToDoTaskGetFullApiModel, ToDoTask, string>
    {
        public ToDoTaskController(ToDoTaskService service, IDataMapper dataMapper) : base(service, dataMapper)
        {
        }
    }
}