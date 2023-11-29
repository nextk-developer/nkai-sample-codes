using Newtonsoft.Json;
using NKAPIService.API.ComputingNode.Models;
using System.Collections.Generic;

namespace NKAPIService.API.ComputingNode
{
    public class RequestListComputingNode : IRequest
    {
        public const string Resource = "/v2/va/list-computing-node";

        public RequestType RequsetType => RequestType.ListComputingNode;

        public string GetResource() => Resource;
        
    }

    public class ResponseListComputingNode : ResponseBase
    {
        [JsonProperty("nodeId")]
        public string NodeId { get; set; }
        [JsonProperty("httpHost")]
        public string HttpHost { get; set; }
        [JsonProperty("httpPort")]
        public int HttpPort { get; set; }
        [JsonProperty("rpcHost")]
        public string RpcHost { get; set; }
        [JsonProperty("rpcPort")]
        public int RpcPort { get; set; }
        [JsonProperty("nodeName")]
        public string NodeName { get; set; }
        [JsonProperty("releaseDate")]
        public string ReleaseDate { get; set; }
        [JsonProperty("productCode")]
        public string ProductCode { get; set; }
        [JsonProperty("productVersion")]
        public string ProductVersion { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
