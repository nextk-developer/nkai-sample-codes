using Newtonsoft.Json;
using System;

namespace NKAPIService.API.Channel
{
    public class RequestPlayback : IRequest
    {
        public const string Resource = "/v2/va/playback";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("includeMeta")]
        public bool IncludeMeta { get; set; }

        [JsonProperty("protocol")]
        public int Protocol { get; set; } = 0;

        public RequestType RequsetType => RequestType.Playback;

        public string GetResource() => Resource;
    }

    public class ResponsePlayback : ResponseBase
    {
        [JsonProperty("mediaUrl")]
        public string MediaUrl { get; set; }
        [JsonProperty("metaUrl")]
        public string MetaUrl { get; set; }
    }
}