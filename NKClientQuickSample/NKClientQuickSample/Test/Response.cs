
using System.Collections.Generic;

namespace NKClientQuickSample.Test
{
    public class ResponseCompute
    {
        public string nodeId { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }
    public class ResponseChannel
    {
        public string channelId { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }
    public class ResponseChannelInfo
    {
        public string channelId { get; set; }
        public string inputUri { get; set; }
        public string channelName { get; set; }
    }
    public class ResponseChannelList
    {
        public List<ResponseChannelInfo> test { get; set; }
    }
    public class ResponseRoiInfo
    {
        public string roiId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int eventType { get; set; }
        public double stayTime { get; set; }
        public int numberOf { get; set; }
        public int feature { get; set; }
        public int code { get; set; }
        public List<Dot> roiDots { get; set; }

    }
    public class Dot
    {
        public double x { get; set; }
        public double y { get; set; }
    }
    public class ResponseRoiList
    {
        public List<ResponseRoiInfo> rois { get; set; }
    }

    public class ResponseVACStartrpcInfoTest
    {
        public string sourceIp { get; set; }
        public int sourcePort { get; set; }
        public int code { get; set; }
    }
}
