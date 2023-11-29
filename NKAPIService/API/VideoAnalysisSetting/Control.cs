using Newtonsoft.Json;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestControl : IRequest
    {
        public const string Resource = "/v2/va/control";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelIds")]
        public List<string> ChannelIDs { get; set; }
        [JsonProperty("operation")]
        public Operations Operation { get; set; }

        [JsonProperty("parameter")]
        public List<string> Parameters { get; set; }

        public RequestType RequsetType => RequestType.Control;
        public string GetResource() => Resource;
    }

    public class ResponseControl : ResponseBase
    {
        [JsonProperty("sourceIp")]
        public string SourceIp { get; set; }
        [JsonProperty("sourcePort")]
        public string SourcePort { get; set; }
    }
}
