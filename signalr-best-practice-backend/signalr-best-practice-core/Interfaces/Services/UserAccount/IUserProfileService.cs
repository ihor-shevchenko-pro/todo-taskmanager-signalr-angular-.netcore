using signalr_best_practice_api_models.Models;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Services.Base;

namespace signalr_best_practice_core.Interfaces.Services.UserAccount
{
    public interface IUserProfileService : IBaseApiService<UserProfileAddApiModel, UserProfileGetFullApiModel, UserProfile, string>
    {
    }
}
