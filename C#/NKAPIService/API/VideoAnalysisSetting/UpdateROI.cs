using Newtonsoft.Json;
using NKAPIService.API.VideoAnalysisSetting.Models;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public sealed class RequestUpdateROI : RoiModel, IRequest
    {
        public const string Resource = "/v2/va/update-roi";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        public RequestType RequsetType => RequestType.UpdateROI;
        public string GetResource() => Resource;
    }
}
