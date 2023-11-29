using Newtonsoft.Json;
using PredefineConstant.Enum.System;
using System;
using System.Collections.Generic;

namespace NKAPIService.API.System
{
    public class SystemLog
    {
        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }
        [JsonProperty("logType")]
        public LogType LogType { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("startTime")]
        public string SourceIp { get; set; }
    }

    public class RequestSystemLog : IRequest
    {
        public const string Resource = "/v2/system-log";
        [JsonIgnore]
        public RequestType RequsetType => RequestType.SystemLog;

        public string GetResource() => Resource;

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        [JsonProperty("logType")]
        public LogType LogType { get; set; }
    }

    public class ResponseSystemLog : ResponseBase
    {
        [JsonProperty("logs")]
        public List<SystemLog> SystemLogs { get; set; }
    }
}
