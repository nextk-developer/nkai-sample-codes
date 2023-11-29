using Newtonsoft.Json;
using System.Collections.Generic;

namespace NKAPIService.API.Channel
{
    public class RequestRecordDays : IRequest
    {
        public const string Resource = "/v2/va/record-days";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        public RequestType RequsetType => RequestType.RecordDays;

        public string GetResource() => Resource;
    }

    public class ResponseRecordDays : ResponseBase
    {
        [JsonProperty("mediaRecord")]
        public Dictionary<string, List<string>> MediaRecordDays { get; set; }
        [JsonProperty("metaRecord")]
        public Dictionary<string, List<string>> MetaRecordDays { get; set; }
    }
}