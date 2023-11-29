using Newtonsoft.Json;
using NKAPIService.API.Converter;

namespace NKAPIService.API
{
    public class ResponseBase
    {
        [JsonProperty("code")]
        [JsonConverter(typeof(IntToErrorCodeConverter))]
        public ErrorCode Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
