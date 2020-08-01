using signalr_best_practice_api_models;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Entities.UserAccount;
using System;

namespace signalr_best_practice_core.Entities
{
    public class ToDoTask : BaseManualEntity<string>
    {
        public ToDoTaskStatusEnum ProgressStatus { get; set; }
        public string Description { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string FromUserId { get; set; }
        public virtual User FromUser { get; set; }

        public string ToUserId { get; set; }
        public virtual User ToUser { get; set; }
    }
}
