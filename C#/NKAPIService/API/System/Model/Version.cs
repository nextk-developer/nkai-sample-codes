using Newtonsoft.Json;

namespace NKAPIService.API.System.Model
{
    public class Version
    {
        [JsonProperty("software")]
        public string Software { get; set; }
        [JsonProperty("detectorModel")]
        public object DetectorModel { get; set; }
        [JsonProperty("firmware")]
        public string Firmware { get; set; }
        [JsonProperty("gpuModel")]
        public string GpuModel { get; set; }
        [JsonProperty("gpuVersion")]
        public string GpuVersion { get; set; }
    }
}
