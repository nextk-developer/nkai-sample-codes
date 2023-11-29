using Newtonsoft.Json;
using NKAPIService.API.VideoAnalysisSetting.Models;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequsetGetROI : IRequest
    {
        public const string Resource = "/v2/va/get-roi";
        [JsonProperty("roiId")]
        public string ROIID { get; set; }

        public RequestType RequsetType => RequestType.GetROI;

        public string GetResource() => Resource;
    }

    public class ResponseGetROI : ResponseBase
    {
        [JsonProperty("roi")]
        public RoiModel Roi { get; set; }
    }
}
