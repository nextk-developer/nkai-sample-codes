using Newtonsoft.Json;

namespace NKAPIService.API.Channel
{
    /// <summary>
    /// 비디오 분석이 실행 중인 채널은 삭제할 수 없으며, 삭제하려면 먼저 분석 실행을 중단해야 합니다.
    /// </summary>
    public sealed class RequestRemoveChannel : IRequest
    {
        public const string Resource = "/v2/va/remove-channel";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonIgnore]
        public RequestType RequsetType => RequestType.RemoveChannel;

        public string GetResource() => Resource;
    }

    public sealed class ResponseRemoveChannel : ResponseBase
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }
    }

}
