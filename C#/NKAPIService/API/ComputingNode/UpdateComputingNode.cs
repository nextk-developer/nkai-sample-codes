using Newtonsoft.Json;

namespace NKAPIService.API.ComputingNode
{
    public class RequestUpdateComputingNode : IRequest
    {
        public const string Resource = "/v2/va/udpate-computing-node";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        [JsonProperty("nodeName")]
        public string NodeName { get; set; }


        [JsonProperty("license")]
        public string License { get; set; }

        public RequestType RequsetType => RequestType.UpdateComputingNode;
        public string GetResource() => Resource;
    }
}
