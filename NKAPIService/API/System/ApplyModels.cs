using Newtonsoft.Json;
using NKAPIService.API.System.Model;
using System.Collections.Generic;

namespace NKAPIService.API.System
{
    public class RequestApplyModels : IRequest
    {
        [JsonIgnore]
        public RequestType RequsetType => RequestType.ApplyModels;

        public string GetResource() => "/v2/va/apply-models";
        public List<ModelConfig> Models { get; set; }
    }
}