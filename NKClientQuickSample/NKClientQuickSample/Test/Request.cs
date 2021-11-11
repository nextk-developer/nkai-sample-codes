using System;
using System.Collections.Generic;
using System.Text;

namespace NKClientQuickSample.Test
{
    public class RequestAddCompute
    {
        public string host { get; set; }
        public int httpPort { get; set; }
        public string nodeName { get; set; }
        public string license { get; set; }
    }
    public class RequestCompute
    {
        public string nodeId { get; set; }
    }

    public class RequestAddChannel
    {
        public string nodeId { get; set; }
        public string channelName { get; set; }
        public string inputUri { get; set; }
        public string inputType { get; set; }
    }
    public class RequestChannel
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
    }
    public class RequestAddROI
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
        public int eventType { get; set; }
        public string roiName { get; set; }
        public string description { get; set; }
        public List<RoiDot> roiDots { get; set; }
    }
    public class RoiDot
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class RequestROI
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
        public string roiId { get; set; }
    }
    public class RequestLink
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
        public List<string> roiLink { get; set; }
        
    }
}
