using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_core.Interfaces.Services.UserAccount;
using System.Threading.Tasks;

namespace signalr_best_practice_bl.Services.UserAccount
{
    public class RefreshTokenService : IRefreshTokenService
    {
        IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> CreateToken(string userId)
        {
            return await _refreshTokenRepository.CreateToken(userId);
        }

        public async Task<string> UpdateToken(string userId, string token)
        {
            return await _refreshTokenRepository.UpdateToken(userId, token);
        }
    }
}
