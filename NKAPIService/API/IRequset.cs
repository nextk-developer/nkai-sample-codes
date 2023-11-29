using Newtonsoft.Json;

namespace NKAPIService.API
{
    public interface IRequest
    {
        [JsonIgnore]
        RequestType RequsetType { get; }
        string GetResource();
    }
}
