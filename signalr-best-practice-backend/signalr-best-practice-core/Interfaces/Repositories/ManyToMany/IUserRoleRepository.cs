using signalr_best_practice_core.Entities.ManyToMany;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Repositories.ManyToMany
{
    public interface IUserRoleRepository : IBaseRepository<UserRole, string>
    {
        Task<IEnumerable<UserRole>> AddRoles(string userId, params string[] roleIds);
        Task RemoveRoles(string userId, params string[] roleIds);
        Task RemoveAllUserRoles(string userId);
    }
}
