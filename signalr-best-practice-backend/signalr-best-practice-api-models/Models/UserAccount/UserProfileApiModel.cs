using Newtonsoft.Json;
using signalr_best_practice_api_models.Models.Base;
using System;

namespace signalr_best_practice_api_models.Models
{
    public abstract class UserProfileBaseApiModel : BaseApiModel<string>
    {
        [JsonProperty("first_name")]      public string FirstName { get; set; }
        [JsonProperty("second_name")]     public string SecondName { get; set; }
    }

    public class UserProfileGetFullApiModel : UserProfileBaseApiModel
    {
        [JsonProperty("created")]         public DateTime Created { get; set; }
        [JsonProperty("updated")]         public DateTime Updated { get; set; }
        [JsonProperty("status")]          public EntityStatusEnum Status { get; set; }
    }

    public class UserProfileGetMinApiModel : UserProfileBaseApiModel
    {
        [JsonProperty("created")]         public DateTime Created { get; set; }
        [JsonProperty("updated")]         public DateTime Updated { get; set; }
        [JsonProperty("status")]          public EntityStatusEnum Status { get; set; }
    }

    public class UserProfileAddApiModel : UserProfileBaseApiModel
    {
    }
}
