using Newtonsoft.Json;
using NKAPIService.API.Channel.Models;

namespace NKAPIService.API.Channel
{
    public sealed class RequestCalibrate : IRequest
    {
        public const string Resource = "/v2/va/callibrate";

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("distortionPoints")]
        public DistortionPoint[] DistortionPoints { get; set; }

        [JsonProperty("calibrationPoints")]
        public CalibrationPoint[] CalibrationPoints { get; set; }

        [JsonIgnore]
        public RequestType RequsetType => RequestType.Calibrate;

        public string GetResource() => Resource;
    }

    /// <summary>
    /// -1 | 등록되지 않은 채널 |
    /// -2 | 비디오 입력(inputUri)이 유효하지 않음 |
    /// -3 | 캘리브레이션 실패 |
    /// </summary>
    public sealed class ResponseCalibrate : ResponseBase
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

    }
}
