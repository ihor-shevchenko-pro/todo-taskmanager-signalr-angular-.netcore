using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.ManyToMany;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalr_best_practice_core.Entities.UserAccount
{
    public class User : BaseEntity<string>
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        [InverseProperty("FromUser")]
        public virtual ICollection<ToDoTask> ToDoTasksFrom { get; set; } = new List<ToDoTask>();
        [InverseProperty("ToUser")]
        public virtual ICollection<ToDoTask> ToDoTasksTo { get; set; } = new List<ToDoTask>();
    }
}
