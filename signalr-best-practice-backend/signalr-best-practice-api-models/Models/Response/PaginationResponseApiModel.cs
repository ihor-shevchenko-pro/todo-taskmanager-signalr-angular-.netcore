using Newtonsoft.Json;
using System.Collections.Generic;

namespace signalr_best_practice_api_models.Models.Response
{
    public class PaginationResponseApiModel<T>
    {
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("count")] public int Count { get; set; }
        [JsonProperty("total")] public int Total { get; set; }
        [JsonProperty("sorting")] public EntitySortingEnum EntitySorting { get; set; }
        [JsonProperty("models")] public IEnumerable<T> Models { get; set; }
    }
}
