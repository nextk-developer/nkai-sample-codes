
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
        public string intputUri { get; set; }
        public string channelName { get; set; }
    }
    public class ResponseRoiInfo
    {
        public string roiId { get; set; }
    }
}
