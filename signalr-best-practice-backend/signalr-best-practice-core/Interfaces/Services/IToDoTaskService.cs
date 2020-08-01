using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Services
{
    public interface IToDoTaskService : IBaseApiService<ToDoTaskAddApiModel, ToDoTaskGetFullApiModel, ToDoTask, string>
    {
        Task<PaginationResponseApiModel<ToDoTaskGetFullApiModel>> GetSentToDoTasks(int start, int count, EntitySortingEnum sort,
            string userId);
        Task<PaginationResponseApiModel<ToDoTaskGetFullApiModel>> GetReceivedToDoTasks(int start, int count, EntitySortingEnum sort,
            string userId);
        Task ChangeProgressStatus(string userId, ToDoTaskChangeProgressStatusApiModel model);
        Task<List<ToDoTaskGetFullApiModel>> AddForAll(ToDoTaskAddApiModel model);
    }
}
