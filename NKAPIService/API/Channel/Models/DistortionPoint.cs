using Newtonsoft.Json;

namespace NKAPIService.API.Channel.Models
{
    public class DistortionPoint
    {
        [JsonProperty("startPoint")]
        public int StartPoint { get; set; }
        [JsonProperty("middlePoint")]
        public int MiddlePoint { get; set; }
        [JsonProperty("endPoint")]
        public int EndPoint { get; set; }
    }
}
