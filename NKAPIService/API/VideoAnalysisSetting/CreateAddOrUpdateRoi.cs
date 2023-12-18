using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public sealed class RequestAddOrUpdateRoi : RoiModel, IRequest
    {
        public const string Resource = "/v3/va/add-or-update-roi";

        [JsonProperty("node_id")]
        public string NodeId { get; set; }
        [JsonProperty("channel_id")]
        public string ChannelID { get; set; }
        public RequestType RequsetType => RequestType.AddOrUpdate;
        public string GetResource() => Resource;
    }

    public class ResponseAddOrUpdateRoi : ResponseBase
    {
        [JsonProperty("roi")]
        public RoiModel Roi { get; set; }
    }
}
