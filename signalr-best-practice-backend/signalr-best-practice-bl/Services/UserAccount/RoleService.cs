using signalr_best_practice_api_models.Models;
using signalr_best_practice_bl.Services.Base;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_core.Interfaces.Services.UserAccount;
using System;
using System.Threading.Tasks;

namespace signalr_best_practice_bl.Services.UserAccount
{
    public class RoleService : BaseApiService<RoleAddApiModel, RoleGetFullApiModel, Role, string>, IRoleService
    {
        private readonly new IRoleRepository _repository;

        public RoleService(IRoleRepository repository, IDataMapper dataMapper) : base(repository, dataMapper)
        {
            _repository = repository;
        }

        public override async Task<string> Add(RoleAddApiModel model)
        {
            var entity = await _repository.AnyAsync(x => x.Name == model.Name);
            if (entity)
            {
                throw new Exception("Duplicate name");
            }

            return await base.Add(model);
        }

        public override async Task Update(RoleAddApiModel model)
        {
            var entity = await _repository.AnyAsync(x => x.Name == model.Name);
            if (entity)
            {
                throw new Exception("Duplicate name");
            }

            await base.Update(model);
        }
    }
}
