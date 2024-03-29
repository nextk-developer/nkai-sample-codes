﻿using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis;
using System.Collections.Generic;

namespace NKAPIService.API.VideoAnalysisSetting
{
    public class RequestListROI : IRequest
    {
        public const string Resource = "/v3/va/list-roi";

        [JsonProperty("node_id")]
        public string NodeId { get; set; }
        [JsonProperty("channel_id")]
        public string ChannelID { get; set; }


        public RequestType RequsetType => RequestType.ListROI;

        public string GetResource() => Resource;
    }

    public class ResponseListROI : ResponseBase
    {
        [JsonProperty("roi_items")]
        public List<RoiModel> RoiItems { get; set; }
    }
}
