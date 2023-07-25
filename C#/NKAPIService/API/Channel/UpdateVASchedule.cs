using Newtonsoft.Json;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public class RequestVaSchedule : IRequest
    {
        public const string Resource = "/v2/va/va-schedule";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        [JsonProperty("schedule")]
        public List<List<int>> Schedule { get; set; }
        [JsonProperty("except")]
        public List<string> Except { get; set; }

        public RequestType RequsetType => RequestType.VaSchedule;

        public string GetResource() => Resource;
    }

    public class ResponseVaSchedule : ResponseBase
    {
        [JsonProperty("schedule")]
        public List<List<int>> Schedule { get; set; }
        [JsonProperty("except")]
        public List<string> Except { get; set; }
    }
}