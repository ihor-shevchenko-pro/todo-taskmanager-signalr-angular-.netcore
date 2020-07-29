using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_bl.Services.Base;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Helpers;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories.ManyToMany;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_core.Interfaces.Services.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalr_best_practice_bl.Services.UserAccount
{
    public class UserService : BaseApiService<UserAddApiModel, UserGetFullApiModel, User, string>, IUserService
    {
        private readonly IPasswordManager _passwordManager;
        private readonly IUserRoleRepository _userRoleRepository;
        new IUserRepository _repository;

        public UserService(IUserRepository repository,
            IUserRoleRepository userRoleRepository,
            IPasswordManager passwordManager,
            IDataMapper dataMapper) : base(repository, dataMapper)
        {
            _passwordManager = passwordManager;
            _userRoleRepository = userRoleRepository;
            _repository = repository;
        }

        public async Task<UserGetFullApiModel> Login(string login, string password, UserContactTypeEnum contactType)
        {
            var contact = login.ToLower();
            var hash = _passwordManager.GetHash(password);

            User user = await _repository.FirstOrDefaultAsync(x => x.Email == contact && x.Password == hash);

            if (user == null) throw new ArgumentException($"Login or password is not valid");

            var userModel = _dataMapper.Parse<User, UserGetFullApiModel>(user);
            return userModel;
        }

        public async Task<UserGetFullApiModel> Registration(string login, string password, string userName, UserContactTypeEnum contactType,
            UserProfileAddApiModel userProfile)
        {
            if (await _repository.AnyAsync(x => x.Email == login))
            {
                throw new ArgumentException($"User with this email already exist");
            }
            if (await _repository.AnyAsync(x => x.UserName == userName))
            {
                throw new ArgumentException($"User with this userName already exist");
            }

            var contact = login.ToLower();
            var hash = _passwordManager.GetHash(password);

            User user = await CreateByEmail(contact, hash);

            user.SetId();
            user.UserName = userName;
            user.UserProfile = _dataMapper.Parse<UserProfileAddApiModel, UserProfile>(userProfile);
            user.UserProfileId = user.Id;
            user.UserProfile.Id = user.Id;
            user.UserProfile.Created = DateTime.UtcNow;
            user.UserProfile.Updated = user.UserProfile.Created;

            await _repository.Add(user);
            user.UserRoles = (await _userRoleRepository.AddRoles(user.Id, RoleHelper.Current.User.Id)).ToList();

            var userModel = _dataMapper.Parse<User, UserGetFullApiModel>(user);
            return userModel;
        }

        public async override Task<string> Add(UserAddApiModel model)
        {
            if (await _repository.AnyAsync(x => x.Email == model.Email))
            {
                throw new ArgumentException($"User with this email already exist");
            }

            var passwordHash = _passwordManager.GetHash(model.Password);

            User user = new User();
            user.Email = model.Email;
            user.Password = passwordHash;
            user.Status = EntityStatusEnum.Active;

            var userProfile = model.UserProfile;

            user.SetId();
            user.UserProfile = _dataMapper.Parse<UserProfileAddApiModel, UserProfile>(userProfile);
            user.UserProfileId = user.Id;
            user.UserProfile.Id = user.Id;
            user.UserProfile.Created = DateTime.UtcNow;
            user.UserProfile.Updated = user.UserProfile.Created;

            await _repository.Add(user);

            var roles = model.Roles.Select(x => x.Id).ToArray();
            await _userRoleRepository.AddRoles(user.Id, roles);

            return user.Id;
        }

        public async Task<UserGetFullApiModel> GetEntity(string userId)
        {
            var user = await _repository.Get(userId);
            if (user == null) throw new ArgumentException($"User is not found");

            var userModel = _dataMapper.Parse<User, UserGetFullApiModel>(user);
            return userModel;
        }

        public async override Task Update(UserAddApiModel model)
        {
            var user = await _repository.Get(model.Id);
            var userProfile = _dataMapper.Parse<UserProfileAddApiModel, UserProfile>(model.UserProfile);
            userProfile.Id = model.Id;

            if (user.UserProfile == null)
            {
                user.UserProfile = userProfile;
            }
            else
            {
                user.UserProfile.FirstName = model.UserProfile.FirstName;
                user.UserProfile.SecondName = model.UserProfile.SecondName;
            }

            user.Email = model.Email;
            await _repository.Update(user);

            // update roles
            var userRoles = await _userRoleRepository.WhereAsync(x => x.Id == user.Id);
            if (userRoles != null && userRoles.ToList().Count > 0)
            {
                var oldRoles = userRoles.ToList().Select(x => x.Role).ToList();
                var oldRoleIds = oldRoles.Select(x => x.Id).ToArray();
                var newRoles = _dataMapper.Parse<List<RoleAddApiModel>, List<Role>>(model.Roles);
                var newRoleIds = newRoles.Select(x => x.Id).ToArray();

                if (!oldRoleIds.SequenceEqual(newRoleIds))
                {
                    await _userRoleRepository.RemoveAllUserRoles(user.Id);
                    await _userRoleRepository.AddRoles(model.Id, newRoleIds);
                }
            }
        }

        public async Task<UserGetFullApiModel> Anonymous()
        {
            User user = new User();

            var userProfile = new UserProfileAddApiModel();

            user.SetId();
            user.UserProfile = _dataMapper.Parse<UserProfileAddApiModel, UserProfile>(userProfile);
            user.UserProfile.Id = user.Id;

            await _repository.Add(user);
            await _userRoleRepository.AddRoles(user.Id, RoleHelper.Current.User.Id);

            var userModel = _dataMapper.Parse<User, UserGetFullApiModel>(user);
            return userModel;
        }

        public async Task<CollectionOfEntities<User>> GetUsers(int start, int count, EntitySortingEnum sort)
        {
            var result = new CollectionOfEntities<User>();
            List<User> collection = null;

            var models = (await _userRoleRepository.WhereAsync(x => x.RoleId == RoleHelper.Current.User.Id)).ToList();
            if (models != null && models.Count > 0)
            {
                collection = models.Select(x => x.User).ToList()
                                   .Where(y => y.UserProfile.Id != null).ToList();
            }

            collection = collection.Distinct().ToList();

            result.Total = collection.Count();
            result.Start = start;
            result.EntitySorting = sort;

            var x = count == 0 ? result.Total - start : count;

            result.Entities = GetSorting(collection, sort).Skip(start).Take(x);
            result.Count = result.Entities.Count();

            return result;
        }

        private async Task<User> CreateByEmail(string email, string password)
        {
            if (await _repository.AnyAsync(x => x.Email == email))
            {
                throw new ArgumentException($"User already exist");
            }
            User user = new User()
            {
                Email = email,
                Password = password,
            };
            return user;
        }


        #region NonAction

        public override Task<PaginationResponseApiModel<UserGetFullApiModel>> Get(int start, int count, EntitySortingEnum sort)
        {
            throw new Exception();
        }

        #endregion
    }
}
