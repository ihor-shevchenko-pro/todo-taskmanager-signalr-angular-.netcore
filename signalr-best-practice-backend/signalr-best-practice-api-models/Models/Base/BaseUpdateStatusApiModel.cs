using Newtonsoft.Json;

namespace signalr_best_practice_api_models.Models.Base
{
    public class BaseUpdateStatusApiModel<TKey> : BaseApiModel<TKey>
    {
        [JsonProperty("status")] public EntityStatusEnum Status { get; set; }
    }
}
