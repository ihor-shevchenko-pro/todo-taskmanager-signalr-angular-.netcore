using signalr_best_practice_core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalr_best_practice_core.Entities.UserAccount
{
    public class RefreshToken : BaseEntity<string>
    {
        [ForeignKey("User")]
        public override string Id { get; set; }
        public virtual User User { get; set; }

        public string Token { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsActive { get; set; }
    }
}
