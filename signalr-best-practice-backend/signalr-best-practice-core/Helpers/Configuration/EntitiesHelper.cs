using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using System.Collections.Generic;

namespace signalr_best_practice_core.Helpers.Configuration
{
    public class EntitiesHelper
    {
        private static readonly EntitiesHelper _instance = new EntitiesHelper();
        public static EntitiesHelper Current => _instance;

        private EntitiesHelper()
        {
        }

        public RoleHelper Roles => RoleHelper.Current;

        public IEnumerable<TEntity> GetItems<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (typeof(TEntity).Name == nameof(Role)) return (IEnumerable<TEntity>)Roles.Roles;

            return new List<TEntity>();
        }
    }
}
