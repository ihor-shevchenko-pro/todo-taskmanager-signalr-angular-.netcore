using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using signalr_best_practice_core.Extensions;
using signalr_best_practice_core.Helpers.Configuration;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace signalr_best_practice_dl_postgresql
{
    public class PostgreSqlContextInitializer : IDatabaseInitializer
    {
        PostgreSqlContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _dbInitializePermition;

        public PostgreSqlContextInitializer(IDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbInitializePermition = _configuration["initialize_database"];
            _context = context as PostgreSqlContext;
            _context.Database.Migrate();
        }

        public void Initialize()
        {
            if (_dbInitializePermition != "1")
            {
                return;
            }

            Log.Current.Message("Start database initialize");
            AddBaseEntities<Role, string>();
            Log.Current.Message("Finish database initialize");
        }

        private void AddBaseEntities<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>, new()
        {
            var time = DateTime.UtcNow;
            var table = _context.Set<TEntity>();

            foreach (var item in EntitiesHelper.Current.GetItems<TEntity, TKey>())
            {
                if (table.Any(x => x.Id.Equals(item.Id)) == false)
                {
                    var entity = new TEntity();
                    entity.Copy(item);
                    entity.Id = item.Id;
                    entity.Created = time;
                    entity.Updated = time;

                    table.Add(entity);
                }
            }

            _context.SaveChanges();
        }
    }
}
