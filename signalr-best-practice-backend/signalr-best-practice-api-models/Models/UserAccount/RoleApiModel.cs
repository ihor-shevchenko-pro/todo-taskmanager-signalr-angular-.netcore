using Newtonsoft.Json;
using signalr_best_practice_api_models.Models.Base;
using System.Collections.Generic;

namespace signalr_best_practice_api_models.Models
{
    public abstract class RoleBaseApiModel : BaseApiModel<string>
    {
        [JsonProperty("name")]        public string Name { get; set; }
    }

    public class RoleGetFullApiModel : RoleBaseApiModel
    {
        [JsonProperty("created")]     public string Created { get; set; }
        [JsonProperty("updated")]     public string Updated { get; set; }
        [JsonProperty("status")]      public EntityStatusEnum Status { get; set; }
        [JsonProperty("users")]       public List<UserGetMinApiModel> Users { get; set; }
    }

    public class RoleGetMinApiModel : RoleBaseApiModel
    {
        [JsonProperty("created")]    public string Created { get; set; }
        [JsonProperty("updated")]    public string Updated { get; set; }
        [JsonProperty("status")]     public EntityStatusEnum Status { get; set; }
    }

    public class RoleAddApiModel : RoleBaseApiModel
    {
    }
}
