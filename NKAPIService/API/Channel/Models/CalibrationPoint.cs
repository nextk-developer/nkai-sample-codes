using Newtonsoft.Json;

namespace NKAPIService.API.Channel.Models
{
    public class CalibrationPoint
    {
        [JsonProperty("verticalPoint1")]
        public int VerticalPoint1 { get; set; }
        [JsonProperty("verticalPoint2")]
        public int VerticalPoint2 { get; set; }
        [JsonProperty("horizontalPoint1")]
        public int HorizontalPoint1 { get; set; }
        [JsonProperty("horizontalPoint2")]
        public int HorizontalPoint2 { get; set; }
    }
}
