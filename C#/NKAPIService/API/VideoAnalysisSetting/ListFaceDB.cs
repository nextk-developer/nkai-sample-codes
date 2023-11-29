using Newtonsoft.Json;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestListFaceDB : IRequest
    {
        public const string Resource = "/v2/va/list-facedb";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        [JsonIgnore]
        public RequestType RequsetType => RequestType.ListFaceDB;
        public string GetResource() => Resource;
    }

    public class ResponseListFaceDB : ResponseBase
    {
        [JsonProperty("faceDBs")]
        public List<FaceDB> FaceDBs { get; set; }
    }
}