using Newtonsoft.Json;

namespace signalr_best_practice_api_models.Models.UserAccount
{
    public class RefreshTokenApiModel
    {
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
    }
}
