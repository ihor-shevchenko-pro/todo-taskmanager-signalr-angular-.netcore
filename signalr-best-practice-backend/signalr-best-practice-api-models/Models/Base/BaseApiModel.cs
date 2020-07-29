using Newtonsoft.Json;

namespace signalr_best_practice_api_models.Models.Base
{
    public abstract class BaseApiModel<TKey>
    {
        [JsonProperty("id")] public virtual TKey Id { get; set; }
    }
}
