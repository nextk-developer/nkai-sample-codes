using Newtonsoft.Json;
using System;

namespace NKAPIService.API.Channel
{
    public class RequestExport : IRequest
    {
        public const string Resource = "/v2/va/export";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        public RequestType RequsetType => RequestType.Export;

        public string GetResource() => Resource;
    }

    public class ResponseExport: ResponseBase
    {
        [JsonProperty("exportUid")]
        public string ExportUid { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }
    }
}