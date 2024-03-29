﻿using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;

namespace NKAPIService.API.Channel
{
    public sealed class RequestUpdateChannelLink : IRequest
    {
        public const string Resource = "/v2/va/update-channel-link";

        [JsonProperty("nodeId")]
        public string NodeId { get; set; }

        [JsonProperty("mainChannelId")]
        public string MainChannelId { get; set; }

        [JsonProperty("subChannelId")]
        public string SubChannelId { get; set; }

        [JsonProperty("linkedType")]
        public LinkType LinkedType { get; set; }

        public RequestType RequsetType => RequestType.UpdateChannelLink;
        public string GetResource() => Resource;
    }
}
