using Newtonsoft.Json;
using NKAPIService.API.VideoAnalysisSetting.Models;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public sealed class RequestUpdateChannelLinkPoints : IRequest
    {
        public const string Resource = "/v2/va/update-channel-link-points";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }
                [JsonProperty("points")]
        public List<ROIDot> Points { get; set; }

        public RequestType RequsetType => RequestType.UpdateChannelLinkPoints;
        public string GetResource() => Resource;
    }
}
