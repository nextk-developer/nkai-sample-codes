using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NKAPIService.API.Statistics
{
    public sealed class RequestStatistics : IRequest
    {
        public const string Resource = "/v2/va/get-statistics";

        public List<string> ChannelIds { get; set; }

        public string Date { get; set; }

        [JsonIgnore]
        public RequestType RequsetType => RequestType.GetStatistics;

        public string GetResource() => Resource;
    }

    public sealed class ResponseStatistics : ResponseBase
    {
        public List<StatisticsMeta> StatisticsMetas { get; set; }
    }
}
