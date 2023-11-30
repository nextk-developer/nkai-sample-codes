using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;

namespace NKAPIService.API.Channel.Models
{
    public class ChannelModel
    {
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }
        [JsonProperty("link")]
        public ChannelLink ChannelLink { get; set; }
        [JsonProperty("inputUrl")]
        public string InputUrl { get; set; }
        [JsonProperty("inputUrlSub")]
        public string InputUrlSub { get; set; }
        [JsonProperty("mediaServerUrl")]
        public string MediaServerUrl { get; set; }
        [JsonProperty("mediaServerUrlSub")]
        public string MediaServerUrlSub { get; set; }
        [JsonProperty("group")]
        public string GroupName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("frameWidth")]
        public int FrameWidth { get; set; }
        [JsonProperty("frameHeight")]
        public int FrameHeight { get; set; }
        [JsonProperty("channelName")]
        public string ChannelName { get; set; }
        [JsonProperty("inputDeviceType")]
        public InputType InputDeviceType { get; set; }
        [JsonProperty("siblings")]
        public string[] Siblings { get; set; }
        [JsonProperty("status")]
        public ChannelStatus Status { get; set; }



        [JsonProperty("localIp")]
        public string LocalIP { get; set; }
        [JsonProperty("rtspPort")]
        public int RtspPort { get; set; }
        [JsonProperty("rpcPort")]
        public int RPCPort { get; set; }

        [JsonProperty("sourceType")]
        public string SourceType { get; set; } = "";
    }

    public class Location
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}