using Newtonsoft.Json;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestRemoveROI : IRequest
    {
        public const string Resource = "/v3/va/remove-roi";


        [JsonProperty("node_id")]
        public string NodeId { get; set; }
        [JsonProperty("channel_id")]
        public string ChannelID { get; set; }

        [JsonProperty("roi_ids")]
        public List<string> ROIIds { get; set; }


        [JsonIgnore]
        public RequestType RequsetType => RequestType.RemoveROI;
        public string GetResource() => Resource;
    }
}
