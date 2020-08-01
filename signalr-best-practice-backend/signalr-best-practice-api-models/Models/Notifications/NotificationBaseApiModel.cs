using Newtonsoft.Json;
using System;

namespace signalr_best_practice_api_models.Models.Notifications
{
    public abstract class NotificationBaseApiModel
    {
        [JsonProperty("title")]                   public string Title { get; set; }
        [JsonProperty("description")]             public string Description { get; set; }
        [JsonProperty("data")]                    public string Data { get; set; }
        [JsonProperty("from_user_id")]            public string FromUserId { get; set; }
        [JsonProperty("to_user_id")]              public string ToUserId { get; set; }

        [JsonProperty("data_type")]               public ModelTypeEnum NotificationDataType { get; set; }
        [JsonProperty("notification_type")]       public NotificationTypeEnum NotificationType { get; set; }
        [JsonProperty("notification_status")]     public NotificationStatusEnum NotificationStatus { get; set; }
    }

    public class NotificationGetFullApiModel : NotificationBaseApiModel
    {
        [JsonProperty("created")]                 public virtual DateTime Created { get; set; }
    }

    public class NotificationGetMinApiModel : NotificationBaseApiModel
    {
    }

    public class NotificationAddApiModel : NotificationBaseApiModel
    {
    }
}
