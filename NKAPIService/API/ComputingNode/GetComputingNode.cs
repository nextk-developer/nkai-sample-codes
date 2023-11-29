using Newtonsoft.Json;
using NKAPIService.API.ComputingNode.Models;

namespace NKAPIService.API.ComputingNode
{
    public class RequestGetComputingNode : IRequest
    {
        public const string Resource = "/v2/va/get-computing-node";

        public RequestType RequsetType => RequestType.GetComputingNode;
        public string GetResource() => Resource;
    }

    public class ResponseGetComputingNode : ResponseBase
    {
        [JsonProperty("node")]
        public Node Node { get; set; }
    }
}
