using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_dl.Repositories.Base;

namespace signalr_best_practice_dl.Repositories.UserAccount
{
    public class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(IGenericRepository<User, string> repository) : base(repository)
        {
        }
    }
}
