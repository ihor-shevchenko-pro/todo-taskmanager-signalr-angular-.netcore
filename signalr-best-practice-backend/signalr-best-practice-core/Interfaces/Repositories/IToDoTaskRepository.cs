using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Repositories.Base;

namespace signalr_best_practice_core.Interfaces.Repositories
{
    public interface IToDoTaskRepository : IBaseRepository<ToDoTask, string>
    {
    }
}
