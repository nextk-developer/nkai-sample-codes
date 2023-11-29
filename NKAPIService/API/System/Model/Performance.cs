using Newtonsoft.Json;

namespace NKAPIService.API.System.Model
{
    public class Performance
    {
        [JsonProperty("cpuUsage")]
        public double CpuUsage { get; set; }
        [JsonProperty("gpuUsage")]
        public double GpuUsage { get; set; }
        [JsonProperty("memoryUsage")]
        public double MemoryUsage { get; set; }
        [JsonProperty("diskUsage")]
        public double DiskUsage { get; set; }
    }
}
