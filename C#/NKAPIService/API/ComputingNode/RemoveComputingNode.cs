using Newtonsoft.Json;

namespace NKAPIService.API.ComputingNode
{
    public class RequestRemoveComputingNode : IRequest
    {
        public const string Resource = "/v2/va/remove-computing-node";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        public RequestType RequsetType => RequestType.RemoveComputingNode;
        public string GetResource() => Resource;
    }

    public class ResponseRemoveComputingNode : ResponseBase
    {
        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
    }
}
