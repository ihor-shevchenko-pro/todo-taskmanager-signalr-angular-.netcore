using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using signalr_best_practice_api.Controllers.Base;
using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_api_models.Models.Auth;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_api_models.Models.UserAccount;
using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Helpers;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Services.UserAccount;

namespace signalr_best_practice_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : DefaultApiController<UserAddApiModel, UserGetFullApiModel, User, string>
    {
        new IUserService _service;
        IRefreshTokenService _refreshTokenService;

        public AccountController(IUserService service, IRefreshTokenService refreshTokenService, IDataMapper dataMapper)
            : base(service, dataMapper)
        {
            _service = service;
            _refreshTokenService = refreshTokenService;
        }

        [AllowAnonymous]
        [HttpGet("get_anonymous")]
        public async Task<ActionResult<SignInApiModel>> GetAnonymous()
        {
            var user = await _service.Anonymous();
            var signIn = await GetTokenApiModel(user);

            return SuccessResult(signIn);
        }

        [AllowAnonymous]
        [HttpPost("authorization_email")]
        public async Task<ActionResult<SignInApiModel>> SignInByEmail(LoginApiModel model)
        {
            var user = await _service.Login(model.Login, model.Password, UserContactTypeEnum.email);
            var signIn = await GetTokenApiModel(user);

            return SuccessResult(signIn);
        }

        [AllowAnonymous]
        [HttpPost("refresh_token")]
        public async Task<ActionResult<SignInApiModel>> RefreshToken(RefreshTokenApiModel model)
        {
            var header = HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;
            var userId = GetUserIdFromToken(header);
            var user = await _service.GetEntity(userId);
            var signIn = await GetTokenApiModel(user, model.RefreshToken);

            return SuccessResult(signIn);
        }

        [AllowAnonymous]
        [HttpPost("registration_email")]
        public async Task<ActionResult<SignInApiModel>> RegistrationEmail(RegistrationApiModel model)
        {
            var user = await _service.Registration(model.Login, model.Password, model.UserName, UserContactTypeEnum.email,
                       model.UserProfile ?? new UserProfileAddApiModel());
            var signIn = await GetTokenApiModel(user);

            await BroadcastMessageSignalR(NotificationTypeEnum.ModelAdd, user, true, user.Id);

            return SuccessResult(signIn);
        }


        private async Task<SignInApiModel> GetTokenApiModel(UserGetFullApiModel user, string refreshToken = null)
        {
            // Create claims for token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var roleClaims = user.Roles.Select(x => new Claim(ClaimTypes.Role, RoleHelper.Current.GetName(x.Id)));
            claims.AddRange(roleClaims);

            // Generate JWT
            var createTime = DateTime.UtcNow;
            var expiresTime = createTime.Add(AuthJwtConfig.Current.Lifetime);

            var token = new JwtSecurityToken(
                issuer: AuthJwtConfig.Current.Issuer,
                audience: AuthJwtConfig.Current.Audience,
                claims: claims,
                expires: expiresTime,
                signingCredentials: new SigningCredentials(
                        AuthJwtConfig.Current.SymmetricSecurityKey,
                        AuthJwtConfig.Current.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refresh = string.IsNullOrEmpty(refreshToken)
                ? await _refreshTokenService.CreateToken(user.Id)
                : await _refreshTokenService.UpdateToken(user.Id, refreshToken);

            SignInApiModel response = new SignInApiModel()
            {
                UserId = user.Id,
                Token = jwtToken,
                RefreshToken = refresh,
                LifeTime = expiresTime.ToString()
            };
            return response;
        }

        private string GetUserIdFromToken(string bearer)
        {
            var token = bearer.Replace("Bearer ", "").Replace("bearer ", "");

            var jwt = new JwtSecurityTokenHandler();

            if (jwt.CanReadToken(token))
            {
                IdentityModelEventSource.ShowPII = true;

                SecurityToken validatedToken;
                TokenValidationParameters validationParameters = new TokenValidationParameters();

                validationParameters.ValidateLifetime = false;

                validationParameters.ValidAudience = AuthJwtConfig.Current.Audience;
                validationParameters.ValidIssuer = AuthJwtConfig.Current.Issuer;
                validationParameters.IssuerSigningKey = AuthJwtConfig.Current.SymmetricSecurityKey;

                ClaimsPrincipal principal = jwt.ValidateToken(token, validationParameters, out validatedToken);

                var id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                return id;
            }

            throw new ArgumentException("Access token is not valid");
        }


        #region NonAction

        [NonAction]
        public override Task<ActionResult<PaginationResponseApiModel<UserGetFullApiModel>>> Get(int start, int count, 
            EntitySortingEnum sort, string query)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public override Task<ActionResult<PaginationResponseApiModel<UserGetFullApiModel>>> Get(int start, int count,
            EntitySortingEnum sort)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public override Task<ActionResult<UserGetFullApiModel>> Get(string id)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public override Task<ActionResult<SuccessResponseApiModel<string>>> Add([FromBody] UserAddApiModel model)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public override Task<ActionResult<SuccessResponseApiModel<string>>> Update(string id, [FromBody] UserAddApiModel model)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public override Task<ActionResult<string>> Delete(string id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}