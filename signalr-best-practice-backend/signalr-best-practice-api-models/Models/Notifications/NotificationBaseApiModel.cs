using Newtonsoft.Json;
using System;

namespace signalr_best_practice_api_models.Models.Notifications
{
    public abstract class NotificationBaseApiModel
    {
        [JsonProperty("title")]                   public string Title { get; set; }
        [JsonProperty("description")]             public string Description { get; set; }
        [JsonProperty("data")]                    public string Data { get; set; }
        [JsonProperty("user_id")]                 public string UserId { get; set; }

        [JsonProperty("data_type")]               public ModelType NotificationDataType { get; set; }
        [JsonProperty("notification_type")]       public NotificationType NotificationType { get; set; }
        [JsonProperty("notification_status")]     public NotificationStatus NotificationStatus { get; set; }
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
