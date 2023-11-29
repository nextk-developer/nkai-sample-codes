using Newtonsoft.Json;
using PredefineConstant.Enum.Recording;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public class RequestRecordingSchedule : IRequest
    {
        public const string Resource = "/v2/va/recording-schedule";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelId")]
        public string ChannelID { get; set; }

        [JsonProperty("schedule")]
        public List<List<Schedule>> Schedule { get; set; }
        [JsonProperty("except")]
        public List<string> Except { get; set; }

        public RequestType RequsetType => RequestType.RecordingSchedule;

        public string GetResource() => Resource;
    }

    public class ResponseRecordingSchedule : ResponseBase
    {
        [JsonProperty("schedule")]
        public List<List<Schedule>> Schedule { get; set; }
        [JsonProperty("except")]
        public List<string> Except { get; set; }
    }


    public class Schedule
    {
        [JsonProperty("time")]
        public int Hour { get; set; }
        [JsonProperty("type")]
        public RecordType RecordType { get; set; }
    }
}