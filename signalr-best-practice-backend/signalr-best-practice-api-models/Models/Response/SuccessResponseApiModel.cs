using Newtonsoft.Json;

namespace signalr_best_practice_api_models.Models.Response
{
    public class SuccessResponseApiModel<TKey>
    {
        [JsonProperty("response")]    public string Response { get; set; }
        [JsonProperty("id")]          public TKey Id { get; set; }
    }
}
