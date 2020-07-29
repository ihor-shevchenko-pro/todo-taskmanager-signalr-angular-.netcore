using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signalr_best_practice_api.Controllers.Base;
using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Services.UserAccount;

namespace signalr_best_practice_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : DefaultApiController<UserAddApiModel, UserGetFullApiModel, User, string>
    {
        new IUserService _service;

        public UserController(IUserService service, IDataMapper dataMapper) : base(service, dataMapper)
        {
            _service = service;
        }

        [HttpGet("get_current_user")]
        public async Task<ActionResult<UserGetFullApiModel>> GetUserInfo()
        {
            var userId = GetUserId();
            var user = await _service.Get(userId);

            return SuccessResult(user);
        }

        [HttpGet("get")]
        public override async Task<ActionResult<PaginationResponseApiModel<UserGetFullApiModel>>> Get(int start, int count,
            EntitySortingEnum sort)
        {
            var pagination = await _service.GetUsers(start, count, sort);

            var models = _dataMapper.ParseCollection<User, UserGetFullApiModel>(pagination.Entities);
            var result = new PaginationResponseApiModel<UserGetFullApiModel>()
            {
                Total = pagination.Total,
                Start = pagination.Start,
                Count = pagination.Count,
                EntitySorting = pagination.EntitySorting,
                Models = models,
            };

            return SuccessResult(result);
        }


        #region NonAction

        [NonAction]
        public override Task<ActionResult<PaginationResponseApiModel<UserGetFullApiModel>>> Get(int start, int count, EntitySortingEnum sort, string query)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}