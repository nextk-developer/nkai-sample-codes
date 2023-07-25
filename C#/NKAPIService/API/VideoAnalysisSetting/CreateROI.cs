using Newtonsoft.Json;
using NKAPIService.API.Converter;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public sealed class RequestCreateROI : RoiModel, IRequest
    {
        public const string Resource = "/v2/va/create-roi";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }
        [JsonProperty("roiName")]
        public string ROIName { get; set; }
        public RequestType RequsetType => RequestType.CreateROI;
        public string GetResource() => Resource;
    }

    public class ResponseCreateROI : ResponseBase
    {
        [JsonProperty("roiId")]
        public string ROIID { get; set; }
    }
}
