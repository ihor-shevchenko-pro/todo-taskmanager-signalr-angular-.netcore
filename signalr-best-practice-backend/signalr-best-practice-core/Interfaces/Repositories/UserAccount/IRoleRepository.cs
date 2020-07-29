using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Repositories.Base;

namespace signalr_best_practice_core.Interfaces.Repositories.UserAccount
{
    public interface IRoleRepository : IBaseRepository<Role, string>
    {
    }
}
