using Newtonsoft.Json;
using NKAPIService.API.Channel.Models;
using System.Threading.Tasks;

namespace NKAPIService.API.Channel
{
    public sealed class RequestGetChannel : IRequest
    {
        public const string Resource = "/v2/va/get-channel";

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonIgnore]
        public RequestType RequsetType => RequestType.GetChannel;

        public string GetResource() => Resource;
    }


    // Response는 ChannelModel 반환

    public sealed class ResponseGetChannel : ResponseBase
    {
        public ChannelModel ChannelModel { get; set; }
    }
}
