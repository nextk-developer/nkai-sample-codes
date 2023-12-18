using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestMatchingFace : FaceDB, IRequest
    {
        public const string Resource = "/v2/va/mathing-face";

        [JsonIgnore]
        public RequestType RequsetType => RequestType.MatchingFace;
        public string GetResource() => Resource;
    }

    public class ResponseMatchingFace : ResponseBase
    {
        [JsonProperty("uuId")]
        public string UuId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userAge")]
        public int UserAge { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }

        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }

        [JsonProperty("faceImages")]
        public List<string> FaceImages { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("similarity")]
        public double Similarity { get; set; }
    }
}
