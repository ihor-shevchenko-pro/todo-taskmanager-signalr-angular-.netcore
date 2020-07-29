using signalr_best_practice_core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalr_best_practice_core.Entities.UserAccount
{
    public class UserProfile : BaseEntity<string>
    {
        [Key, ForeignKey("User")]
        public override string Id { get; set; }
        public virtual User User { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
