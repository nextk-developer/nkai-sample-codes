using Newtonsoft.Json;
using NKAPIService.API.Channel.Models;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public sealed class RequestListChannels : IRequest
    {
        public const string Resource = "/v2/va/list-channel";

        [JsonIgnore]
        public RequestType RequsetType => RequestType.ListChannel;

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        public string GetResource() => Resource;
    }

    public sealed class ResponseListChannels : ResponseBase
    {
        [JsonProperty("channels")]
        public List<ChannelModel> ChannelItems { get; set; }
    }
}
