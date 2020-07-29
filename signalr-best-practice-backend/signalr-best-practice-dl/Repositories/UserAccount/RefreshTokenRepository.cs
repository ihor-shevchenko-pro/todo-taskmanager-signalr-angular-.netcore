using signalr_best_practice_api_models;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_dl.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_dl.Repositories.UserAccount
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken, string>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IGenericRepository<RefreshToken, string> repository) : base(repository)
        {
        }

        public async Task<string> CreateToken(string userId)
        {
            var token = GenerateRefreshToken();

            await _repository.Insert(new RefreshToken()
            {
                Id = userId,
                Token = token,
                SecurityStamp = token,
                IsActive = true
            });

            return token;
        }

        public async Task<string> UpdateToken(string userId, string token)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => x.Id == userId && x.Token == token && x.IsActive);

            if (entity == null)
            {
                // Get old refresh token and deactivate all issued tokens
                entity = await _repository.FirstOrDefaultAsync(x => x.Id == userId && x.Token == token);
                if (entity != null)
                {
                    var securityStamp = entity.SecurityStamp;
                    var collection = await _repository.WhereAsync(x => x.Id == userId && x.SecurityStamp == securityStamp
                    && x.IsActive);

                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                    }

                    await _repository.Update(collection);
                }

                throw new ArgumentException($"Refresh token {token} is not found for user {userId}");
            }

            // Update old token
            entity.IsActive = false;
            await _repository.Update(entity, x => x.Id.Equals(entity.Id) && x.Token.Equals(entity.Token));

            // Create new refresh token
            var newToken = new RefreshToken()
            {
                Id = entity.Id,
                Token = GenerateRefreshToken(),
                SecurityStamp = entity.SecurityStamp,
                IsActive = true,
            };
            await _repository.Insert(newToken);

            return newToken.Token;
        }


        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }


        #region Base

        public override Task Add(RefreshToken entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> AnyAsync(Func<RefreshToken, bool> search)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<RefreshToken> FirstOrDefaultAsync(Func<RefreshToken, bool> search)
        {
            throw new NotImplementedException();
        }

        public override Task<CollectionOfEntities<RefreshToken>> Get(int start, int count, EntitySortingEnum sort)
        {
            throw new NotImplementedException();
        }

        public override Task<RefreshToken> Get(string id)
        {
            throw new NotImplementedException();
        }

        public override Task Update(RefreshToken entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<RefreshToken>> WhereAsync(Func<RefreshToken, bool> search)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
