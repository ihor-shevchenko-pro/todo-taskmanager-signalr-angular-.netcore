using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Repositories;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using signalr_best_practice_dl.Repositories.Base;

namespace signalr_best_practice_dl.Repositories
{
    public class ToDoTaskRepository : BaseRepository<ToDoTask, string>, IToDoTaskRepository
    {
        public ToDoTaskRepository(IGenericRepository<ToDoTask, string> repository) : base(repository)
        {
        }
    }
}
