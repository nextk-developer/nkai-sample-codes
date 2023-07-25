using Newtonsoft.Json;
using NKAPIService.API.Converter;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public sealed class RequestUpdateROI : IRequest
    {
        public const string Resource = "/v2/va/update-roi";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }
        [JsonProperty("roiId")]
        public string ROIId { get; set; }
        [JsonProperty("eventType")]
        [JsonConverter(typeof(EventToStringConverter))]
        public IntegrationEventType EventType { get; set; }
        [JsonProperty("roiName")]
        public string ROIName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("stayTime")]
        public int StayTime { get; set; }
        [JsonProperty("numberOf")]
        public int NumberOf { get; set; }
        [JsonProperty("feature")]
        public ROIFeature ROIFeature { get; set; }
        [JsonProperty("roiType")]
        public DrawingType RoiType { get; set; }
        [JsonProperty("roiDots")]
        public List<ROIDot> ROIDots { get; set; }
        [JsonProperty("EventFilter")]
        public EventFilter EventFilter { get; set; }
        [JsonProperty("roiNumber")]
        public RoiNumber ROINumber { get; set; }

        public RequestType RequsetType => RequestType.UpdateROI;
        public string GetResource() => Resource;
    }
}
