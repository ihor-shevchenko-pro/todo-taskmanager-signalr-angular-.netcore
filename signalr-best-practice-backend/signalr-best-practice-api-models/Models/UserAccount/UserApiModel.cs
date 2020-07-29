using Newtonsoft.Json;
using signalr_best_practice_api_models.Models.Base;
using System;
using System.Collections.Generic;

namespace signalr_best_practice_api_models.Models
{
    public abstract class UserBaseApiModel : BaseApiModel<string>
    {
        [JsonProperty("email")]              public string Email { get; set; }
        [JsonProperty("user_name")]          public string UserName { get; set; }
        [JsonProperty("user_profile_id")]    public string UserProfileId { get; set; }
    }

    public class UserGetFullApiModel : UserBaseApiModel
    {
        [JsonProperty("created")]           public DateTime Created { get; set; }
        [JsonProperty("updated")]           public DateTime Updated { get; set; }
        [JsonProperty("status")]            public EntityStatusEnum Status { get; set; }
        [JsonProperty("roles")]             public List<RoleGetMinApiModel> Roles { get; set; }
        //[JsonProperty("todo_tasks_from")]   public List<ToDoTaskGetMinApiModel> ToDoTasksFrom { get; set; }
        //[JsonProperty("todo_tasks_to")]     public List<ToDoTaskGetMinApiModel> ToDoTasksTo { get; set; }
        [JsonProperty("user_profile")]      public UserProfileGetMinApiModel UserProfile { get; set; }
    }

    public class UserGetMinApiModel : UserBaseApiModel
    {
        [JsonProperty("created")]           public DateTime Created { get; set; }
        [JsonProperty("updated")]           public DateTime Updated { get; set; }
        [JsonProperty("status")]            public EntityStatusEnum Status { get; set; }
        [JsonProperty("user_profile")]      public UserProfileGetMinApiModel UserProfile { get; set; }
    }

    public class UserAddApiModel : UserBaseApiModel
    {
        [JsonProperty("password")]          public string Password { get; set; }
        [JsonProperty("roles")]             public List<RoleAddApiModel> Roles { get; set; }
        [JsonProperty("user_profile")]      public UserProfileAddApiModel UserProfile { get; set; }
    }
}
