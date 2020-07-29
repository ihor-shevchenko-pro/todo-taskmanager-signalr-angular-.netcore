using signalr_best_practice_core.Entities.ManyToMany;
using signalr_best_practice_core.Helpers;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using signalr_best_practice_core.Interfaces.Repositories.ManyToMany;
using signalr_best_practice_dl.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalr_best_practice_dl.Repositories.ManyToMany
{
    public class UserRoleRepository : BaseRepository<UserRole, string>, IUserRoleRepository
    {
        public UserRoleRepository(IGenericRepository<UserRole, string> repository) : base(repository)
        {
        }


        public async Task<IEnumerable<UserRole>> AddRoles(string userId, params string[] roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var key = await _repository.AnyAsync(x => x.Id == userId && x.RoleId == roleId);
                if (key == false)
                {
                    await _repository.Insert(new UserRole()
                    {
                        Id = userId,
                        RoleId = roleId
                    });
                }
            }

            return roleIds.Select(x => new UserRole()
            {
                Id = userId,
                RoleId = x,
                Role = RoleHelper.Current.GetRole(x),
            });
        }

        public async Task RemoveAllUserRoles(string userId)
        {
            var userRoles = await _repository.WhereAsync(x => x.Id == userId);
            if (userRoles != null && userRoles.ToList().Count > 0)
            {
                foreach (var item in userRoles.ToList())
                {
                    await _repository.Delete(item);
                }
            }
        }

        // Delete defenite roles
        public async Task RemoveRoles(string userId, params string[] roleIds)
        {
            foreach (var role in roleIds)
            {
                var key = await _repository.AnyAsync(x => x.Id == userId && x.RoleId == role);
                if (key)
                {
                    await _repository.Delete(role);
                }
            }
        }
    }
}
