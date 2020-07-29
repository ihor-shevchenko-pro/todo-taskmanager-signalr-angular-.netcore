using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signalr_best_practice_api.Controllers.Base;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Services.UserAccount;

namespace signalr_best_practice_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RoleController : DefaultApiController<RoleAddApiModel, RoleGetFullApiModel, Role, string>
    {
        public RoleController(IRoleService service, IDataMapper dataMapper) : base(service, dataMapper)
        {
            _service = service;
        }
    }
}