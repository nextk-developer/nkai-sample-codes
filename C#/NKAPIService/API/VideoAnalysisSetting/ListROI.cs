using Newtonsoft.Json;
using NKAPIService.API.VideoAnalysisSetting.Models;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestListROI : IRequest
    {
        public const string Resource = "/v2/va/list-roi";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        
        public RequestType RequsetType => RequestType.ListROI;

        public string GetResource() => Resource;
    }

    public class ResponseListROI : ResponseBase
    {
        [JsonProperty("rois")]
        public List<RoiModel> RoiItems { get; set; }
    }
}
