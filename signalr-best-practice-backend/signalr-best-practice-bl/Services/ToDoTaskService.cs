using signalr_best_practice_api_models.Models;
using signalr_best_practice_bl.Services.Base;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories;
using signalr_best_practice_core.Interfaces.Services;

namespace signalr_best_practice_bl.Services
{
    public class ToDoTaskService : BaseApiService<ToDoTaskAddApiModel, ToDoTaskGetFullApiModel, ToDoTask, string>, IToDoTaskService
    {
        public ToDoTaskService(IToDoTaskRepository repository, IDataMapper dataMapper) : base(repository, dataMapper)
        {
        }
    }
}
