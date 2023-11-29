using Newtonsoft.Json;
using NKAPIService.API.Channel.Models;
using NKAPIService.API.Converter;

namespace NKAPIService.API.Channel
{
    public sealed class RequestRegisterChannel : IRequest
    {
        public const string Resource = "/v2/va/register-channel";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("channelName")]
        public string ChannelName { get; set; }

        [JsonProperty("inputUrl")]
        public string InputUrl { get; set; }
        [JsonProperty("inputUrlSub")]
        public string InputUrlSub { get; set; }

        [JsonProperty("inputType")]
        [JsonConverter(typeof(StringToInputTypeConverter))]
        public InputType InputType { get; set; }
        [JsonProperty("groupName")]
        public string GroupName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("autoTimeout")]
        public bool AutoTimeout { get; set; }

        [JsonIgnore]
        public RequestType RequsetType => RequestType.RegisterChannel;

        public string GetResource() => Resource;
    }

    public sealed class ResponseRegisterChannel : ResponseBase
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("mediaServerUrl")]
        public string MediaServerUrl { get; set; }
        [JsonProperty("mediaServerUrlSub")]
        public string MediaServerUrlSub { get; set; }

        [JsonProperty("sourceType")]
        public string SourceType { get; set; } = "";
    }
}
