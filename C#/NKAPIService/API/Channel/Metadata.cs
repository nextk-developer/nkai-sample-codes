using Newtonsoft.Json;
using PredefineConstant;
using PredefineConstant.Converter;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public class ReIdObject
    {
        [JsonProperty("channelId")]
        public string ChannelUId { get; set; }
        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("eventType")]
        [JsonConverter(typeof(EventToStringConverter))]
        public IntegrationEventType EventType { get; set; }
        [JsonProperty("classId")]
        public ClassId ClassId { get; set; }
        [JsonProperty("evtId")]
        public int EventId { get; set; }
        [JsonProperty("objectId")]
        public int ObjectId { get; set; }
        [JsonProperty("frameNum")]
        public int FrameNumber { get; set; }
        [JsonProperty("matchingScore")]
        public double MatchingScore { get; set; }
    }

    public class RequestMetadata : IRequest
    {
        public const string Resource = "/v2/va/metadata";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("channelIds")]
        public List<string> ChannelIDs { get; set; }

        [JsonProperty("eventTypes")]
        public List<string> EventTypes { get; set; }

        [JsonProperty("progressFilter")]
        public List<int> ProgressFilter { get; set; }

        [JsonProperty("classIdFilter")]
        public List<int> ClassIdFilter { get; set; }


        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("includeThumbnail")]
        public bool IncludeThumbnail { get; set; }

        [JsonProperty("reIdObject")]
        public ReIdObject ReIdObject { get; set; }

        public RequestType RequsetType => RequestType.Metadata;

        public string GetResource() => Resource;
    }

    public class ResponseMetadata : ResponseBase
    {
        [JsonProperty("objectMetas")]
        public List<ObjectMeta> ObjectMetas { get; set; }
    }
}
