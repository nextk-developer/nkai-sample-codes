using Newtonsoft.Json;
using NKAPIService.API.System.Model;
using System.Collections.Generic;

namespace NKAPIService.API.System
{
    public class RequestGetModels : IRequest
    {
        [JsonIgnore]
        public RequestType RequsetType => RequestType.GetModels;

        public string GetResource() => "/v2/va/get-models";
        public string NodeId { get; set; }
    }

    public class ResponseGetModels : ResponseBase
    {
        public List<ModelConfig> Models { get; set; }
    }
}
