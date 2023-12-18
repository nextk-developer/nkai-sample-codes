using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public class ChannelLink
    {
        [JsonProperty("channelUid")]
        public string LinkedChannelId { get; set; }
        [JsonProperty("linkedType")]
        public LinkType LinkedType { get; set; }
        [JsonProperty("points")]
        public List<ROIDot> Points { get; set; }
    }
}
