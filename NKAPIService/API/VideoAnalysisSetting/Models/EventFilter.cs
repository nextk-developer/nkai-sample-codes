using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting.Models
{
    public class EventFilter
    {
        [JsonProperty("minDetectSize")]
        public RoiSize MinDetectSize { get; set; }
        [JsonProperty("maxDetectSize")]
        public RoiSize MaxDetectSize { get; set; }

        [JsonProperty("objectsTarget")]
        public List<ClassId> ObjectsTarget { get; set; }
    }
}
