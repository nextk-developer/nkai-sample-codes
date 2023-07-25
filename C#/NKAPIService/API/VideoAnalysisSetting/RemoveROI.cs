using Newtonsoft.Json;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestRemoveROI : IRequest
    {
        public const string Resource = "/v2/va/remove-roi";


        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        [JsonProperty("roiIds")]
        public List<string> ROIIds { get; set; }


        [JsonIgnore]
        public RequestType RequsetType => RequestType.RemoveROI;
        public string GetResource() => Resource;
    }
}
