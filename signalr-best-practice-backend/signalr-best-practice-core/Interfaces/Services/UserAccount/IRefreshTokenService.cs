using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Services.UserAccount
{
    public interface IRefreshTokenService
    {
        Task<string> CreateToken(string userId);
        Task<string> UpdateToken(string userId, string token);
    }
}
