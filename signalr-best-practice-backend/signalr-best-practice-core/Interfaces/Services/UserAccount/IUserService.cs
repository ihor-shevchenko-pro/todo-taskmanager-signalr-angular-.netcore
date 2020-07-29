using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Services.Base;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Services.UserAccount
{
    public interface IUserService : IBaseApiService<UserAddApiModel, UserGetFullApiModel, User, string>
    {
        Task<UserGetFullApiModel> Login(string login, string password, UserContactTypeEnum contactType);
        Task<UserGetFullApiModel> Registration(string login, string password, string userName, UserContactTypeEnum contactType,
            UserProfileAddApiModel userProfile);
        Task<UserGetFullApiModel> GetEntity(string userId);
        Task<UserGetFullApiModel> Anonymous();
        Task<CollectionOfEntities<User>> GetUsers(int start, int count, EntitySortingEnum sort);
    }
}
