using Newtonsoft.Json;

namespace NKAPIService.API.System
{
    public enum Command { Custom = 0, Reboot, Restart }

    public class RequestSystem : IRequest
    {
        [JsonIgnore]
        public RequestType RequsetType => RequestType.System;

        public string GetResource() => "/v2/va/system";
        public string NodeId { get; set; }
        public Command CommandType { get; set; }
        public string Command { get; set; }
    }
}
