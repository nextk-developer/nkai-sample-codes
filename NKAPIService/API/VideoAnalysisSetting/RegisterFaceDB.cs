using Newtonsoft.Json;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestRegisterFaceDB : FaceDB, IRequest
    {
        public const string Resource = "/v2/va/register-facedb";

        [JsonIgnore]
        public RequestType RequsetType => RequestType.RegisterFaceDB;
        public string GetResource() => Resource;
    }

    public class ResponseRegisterFaceDB : ResponseBase
    {
        [JsonProperty("uuId")]
        public string UuId { get; set; }
    }
}
