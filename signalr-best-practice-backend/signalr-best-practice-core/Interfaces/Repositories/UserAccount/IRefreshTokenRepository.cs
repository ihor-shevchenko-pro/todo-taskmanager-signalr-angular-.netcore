using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Repositories.UserAccount
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken, string>
    {
        Task<string> CreateToken(string userId);

        Task<string> UpdateToken(string userId, string token);
    }
}
