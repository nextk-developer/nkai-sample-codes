using Newtonsoft.Json;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestUpdateFaceDB : IRequest
    {
        public const string Resource = "/v2/va/modify-facedb";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("faceImages")]
        public List<string> FaceImages { get; set; }

        public RequestType RequsetType => RequestType.UpdateFaceDB;
        public string GetResource() => Resource;
    }

    public class ResponseUpdateFaceDB : ResponseBase
    {

    }
}
