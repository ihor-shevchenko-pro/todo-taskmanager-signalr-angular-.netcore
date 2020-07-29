using Newtonsoft.Json;

namespace signalr_best_practice_api_models.Models.Auth
{
    public class LoginApiModel
    {
        [JsonProperty("login")]        public string Login { get; set; }
        [JsonProperty("password")]     public string Password { get; set; }
    }
}
