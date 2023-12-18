using Newtonsoft.Json;
using System.Collections.Generic;

namespace PredefineConstant.Enum.Analysis
{
    public class ROILine
    {
        [JsonProperty("disable")]
        public bool Disable { get; set; }
        [JsonProperty("direction")]
        public string Direction { get; set; }
        [JsonProperty("target")]
        public List<ClassId> ObjectTarget { get; set; }
    }
}
