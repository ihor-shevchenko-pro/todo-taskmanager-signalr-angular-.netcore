using Newtonsoft.Json;

namespace signalr_best_practice_api_models.Models.Auth
{
    public class RegistrationApiModel
    {
        [JsonProperty("login")]          public string Login { get; set; }
        [JsonProperty("password")]       public string Password { get; set; }
        [JsonProperty("user_name")]      public string UserName { get; set; }
        [JsonProperty("user_profile")]   public UserProfileAddApiModel UserProfile { get; set; }
    }
}
