using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public class RequestMetaTimeList : IRequest
    {
        public const string Resource = "/v2/va/metadata-timelist";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelIds")]
        public List<string> ChannelID { get; set; }

        [JsonProperty("eventTypes")]
        public List<string> EventTypes { get; set; }

        [JsonProperty("progressFilter")]
        public List<int> ProgressFilter { get; set; }

        [JsonProperty("classIdFilter")]
        public List<ClassId> ClassIdFilter { get; set; }


        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }


        public RequestType RequsetType => RequestType.MetadataTimeList;

        public string GetResource() => Resource;
    }

    public class ResponseMetaTimeList : ResponseBase
    {
        [JsonProperty("timeList")]
        public List<DateTime> TimeList { get; set; }
    }
}
