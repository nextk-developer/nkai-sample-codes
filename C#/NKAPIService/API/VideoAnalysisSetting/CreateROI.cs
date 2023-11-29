using Newtonsoft.Json;
using NKAPIService.API.VideoAnalysisSetting.Models;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public sealed class RequestCreateROI : RoiModel, IRequest
    {
        public const string Resource = "/v2/va/create-roi";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }
        public RequestType RequsetType => RequestType.CreateROI;
        public string GetResource() => Resource;
    }

    public class ResponseCreateROI : ResponseBase
    {
        [JsonProperty("roiId")]
        public string ROIID { get; set; }
    }
}
