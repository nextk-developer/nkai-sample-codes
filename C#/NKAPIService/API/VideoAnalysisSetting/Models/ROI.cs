using Newtonsoft.Json;
using NKAPIService.API.Converter;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting.Models
{
    public class RoiModel
    {
        [JsonProperty("roiId")]
        public string RoiId { get; set; }
        [JsonProperty("name")]
        public string RoiName { get; set; }

        [JsonProperty("roiType")]
        public DrawingType RoiType { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("stayTime")]
        public double StayTime { get; set; }
        [JsonProperty("numberOf")]
        public int NumberOf { get; set; }
        [JsonProperty("eventType")]
        [JsonConverter(typeof(EventToStringConverter))]
        public IntegrationEventType EventType { get; set; }
        [JsonProperty("feature")]
        public ROIFeature ROIFeature { get; set; }
        [JsonProperty("roiDots")]
        public List<ROIDot> ROIDots { get; set; }
        [JsonProperty("roiDotsSub")]
        public List<ROIDot> ROIDotsSub { get; set; }
        [JsonProperty("eventFilter")]
        public EventFilter EventFilter { get; set; }
        [JsonProperty("objectTypes")]
        public List<ClassId> ClassTypes { get; set; }
        [JsonProperty("number")]
        public RoiNumber ROINumber { get; set; }
        [JsonProperty("params")]
        public Dictionary<string, string> Params { get; set; }
    }
}
