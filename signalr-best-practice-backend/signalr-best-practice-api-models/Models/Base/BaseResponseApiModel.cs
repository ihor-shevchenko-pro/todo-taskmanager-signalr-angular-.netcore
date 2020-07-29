using Newtonsoft.Json;
using System.Collections.Generic;

namespace signalr_best_practice_api_models.Models.Base
{
    public class BaseResponseApiModel
    {
        [JsonProperty("errors")] public List<string> Errors { get; set; }
    }
}
