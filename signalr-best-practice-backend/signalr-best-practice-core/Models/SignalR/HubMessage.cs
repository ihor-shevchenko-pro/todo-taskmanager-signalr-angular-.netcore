using Newtonsoft.Json;
using signalr_best_practice_api_models;
using System.Collections.Generic;

namespace signalr_best_practice_core.Models.SignalR
{
    public class HubMessage
    {
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("code")] public int Code { get; set; }
        [JsonProperty("data_type")] public DataTypeEnum DataType { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
        [JsonProperty("errors")] public List<string> Errors { get; set; }

        public HubMessage()
        {
            Errors = new List<string>();
        }


        public static HubMessage GetSuccessModel(string message)
        {
            var result = new HubMessage();
            result.Data = null;
            result.Code = 200;
            result.Success = true;
            result.Message = message;

            return result;
        }

        public static HubMessage GetDataModel(object data, int code = 200, string message = "Success")
        {
            var result = new HubMessage();
            result.Data = data;
            result.Code = code;
            result.Success = true;
            result.Message = message;

            return result;
        }

        public static HubMessage GetErrorModel(int code, string errorMessage, params string[] errors)
        {
            var result = new HubMessage();
            result.Data = null;
            result.Code = code;
            result.Message = errorMessage;
            result.Success = false;
            result.Errors.AddRange(errors);

            return result;
        }
    }
}
