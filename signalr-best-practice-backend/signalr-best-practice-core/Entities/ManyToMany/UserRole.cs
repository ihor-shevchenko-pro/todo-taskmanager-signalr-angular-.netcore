using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalr_best_practice_core.Entities.ManyToMany
{
    public class UserRole : BaseEntity<string>
    {
        [ForeignKey("User")]
        public override string Id { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
