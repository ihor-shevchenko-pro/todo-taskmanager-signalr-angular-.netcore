using signalr_best_practice_api_models.Models;
using signalr_best_practice_bl.Services.Base;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_core.Interfaces.Services.UserAccount;

namespace signalr_best_practice_bl.Services.UserAccount
{
    public class UserProfileService : BaseApiService<UserProfileAddApiModel, UserProfileGetFullApiModel, UserProfile, string>, IUserProfileService
    {
        public UserProfileService(IUserProfileRepository repository, IDataMapper dataMapper) : base(repository, dataMapper)
        {
        }
    }
}
