using Newtonsoft.Json;

namespace NKAPIService.API.ComputingNode
{
    public class RequestCreateComputingNode : IRequest
    {
        public const string Resource = "/v2/va/create-computing-node";

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("nodeName")]
        public string NodeName { get; set; }


        public RequestType RequsetType => RequestType.CreateComputingNode;
        public string GetResource() => Resource;
    }

    public class ResponseCreateComputingNode : ResponseBase
    {
        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        [JsonProperty("productVersion")]
        public string ProductVersion { get; set; }

        [JsonProperty("releaseDate")]
        public string ReleaseDate { get; set; }
    }
}
