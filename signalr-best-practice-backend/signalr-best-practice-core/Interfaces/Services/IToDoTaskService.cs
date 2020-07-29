using signalr_best_practice_api_models.Models;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Services.Base;

namespace signalr_best_practice_core.Interfaces.Services
{
    public interface IToDoTaskService : IBaseApiService<ToDoTaskAddApiModel, ToDoTaskGetFullApiModel, ToDoTask, string>
    {
    }
}
