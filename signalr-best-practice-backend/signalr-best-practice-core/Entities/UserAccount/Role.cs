using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.ManyToMany;
using System.Collections.Generic;

namespace signalr_best_practice_core.Entities.UserAccount
{
    public class Role : BaseEntity<string>
    {
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
