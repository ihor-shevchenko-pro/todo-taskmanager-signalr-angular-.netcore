using Newtonsoft.Json;
using signalr_best_practice_api_models.Models.Base;
using System;

namespace signalr_best_practice_api_models.Models
{
    public abstract class ToDoTaskBaseApiModel : BaseApiModel<string>
    {
        [JsonProperty("title")]              public string Title { get; set; }
        [JsonProperty("description")]        public string Description { get; set; }
        [JsonProperty("task_finish")]        public DateTime? TaskFinish { get; set; }
        [JsonProperty("progress_status")]    public ToDoTaskStatusEnum ProgressStatus { get; set; }
        [JsonProperty("from_user_id")]       public string FromUserId { get; set; }
        [JsonProperty("to_user_id")]         public string ToUserId { get; set; }
    }

    public class ToDoTaskGetFullApiModel : ToDoTaskBaseApiModel
    {
        [JsonProperty("created")]            public DateTime Created { get; set; }
        [JsonProperty("updated")]            public DateTime Updated { get; set; }
        [JsonProperty("status")]             public EntityStatusEnum Status { get; set; }
        [JsonProperty("from_user")]          public virtual UserGetMinApiModel FromUser { get; set; }
        [JsonProperty("to_user")]            public virtual UserGetMinApiModel ToUser { get; set; }

    }

    public class ToDoTaskGetMinApiModel : ToDoTaskBaseApiModel
    {
        [JsonProperty("created")]            public DateTime Created { get; set; }
        [JsonProperty("updated")]            public DateTime Updated { get; set; }
        [JsonProperty("status")]             public EntityStatusEnum Status { get; set; }
    }

    public class ToDoTaskAddApiModel : ToDoTaskBaseApiModel
    {
    }
}
