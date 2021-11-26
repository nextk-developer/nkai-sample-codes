
using System;
using System.Collections.Generic;

namespace NKClientQuickSample.Test
{
    public static class TestCase
    {
        static public string GetPath(Path path)
        {
            switch (path)
            {
                //node
                case Path.NODE_CREATE: return "v2/va/create-computing-node";
                case Path.NODE_REMOVE: return "v2/va/remove-computing-node";
                case Path.NODE_UPDATE: return "v2/va/update-computing-node";
                case Path.NODE_GET: return "v2/va/get-computing-node";
                case Path.NODE_LIST: return "v2/va/list-computing-node";
                //channel
                case Path.CH_REGIST: return "v2/va/register-channel";
                case Path.CH_REMOVE: return "v2/va/remove-channel";
                case Path.CH_UPDATE: return "v2/va/update-channel";
                case Path.CH_GET: return "v2/va/get-channel";
                case Path.CH_LIST: return "v2/va/list-channel";
                //roi
                case Path.ROI_CREATE: return "v2/va/create-roi";
                case Path.ROI_REMOVE: return "v2/va/remove-roi";
                case Path.ROI_UPDATE: return "v2/va/update-roi";
                case Path.ROI_GET: return "v2/va/get-roi";
                case Path.ROI_LIST: return "v2/va/list-roi";
                //link
                case Path.LINK_CREATE: return "v2/va/create-link";
                case Path.LINK_GET: return "v2/va/get-link";
                case Path.LINK_LIST: return "v2/va/list-link";
                case Path.LINK_REMOVE: return "v2/va/remove-link";
                case Path.LINK_UPDATE: return "v2/va/update-link";
                //va Command
                case Path.VA_COMMAND_START: 
                case Path.VA_COMMAND_STOP:
                    return "v2/va/control";
                //system
                case Path.SYSTEM_STATUS: return "v2/va/get-system-status";

                default: return "";
            }
        }
        static public string GetFormat(Path path, string param1, string param2 = null, string param3 = null)
        {
            switch (path)
            {
                //node
                case Path.NODE_CREATE:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddCompute
                    {
                        host = param1,
                        httpPort = Convert.ToInt32(param2),
                        nodeName = "TEST_COMPUTE_NODE",
                        license = "license-license-license-license"

                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.NODE_REMOVE:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestCompute
                    {
                        nodeId = param1
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.NODE_GET:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestCompute
                    {
                        nodeId = param1
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.NODE_LIST:
                    return "{\r\n}";
                //channel
                case Path.CH_REGIST:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddChannel
                    {
                        nodeId = param1,
                        channelName = param2,
                        inputUri = param3
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.CH_REMOVE:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestChannel
                    {
                        nodeId = param1,
                        channelId = param2
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.CH_UPDATE:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestChannel
                    {
                        nodeId = param1,
                        channelId = param2
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.CH_GET:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddROI
                    {
                        nodeId = param1,
                        channelId = param2
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.CH_LIST:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddROI
                    {
                        nodeId = param1
                    }, Newtonsoft.Json.Formatting.Indented);
                //roi
                case Path.ROI_CREATE:
                    {
                        var listRoi = new List<RoiDot>
                        {
                            new RoiDot { X = 0.1, Y = 0.1},
                            new RoiDot { X = 0.9, Y = 0.1},
                            new RoiDot { X = 0.9, Y = 0.9},
                            new RoiDot { X = 0.1, Y = 0.9},
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddROI
                        {
                            nodeId = param1,
                            eventType = Test.EventType.EVT_LOITERING,
                            channelId = param2,
                            roiName = "ROI_NAME",
                            description = "Loitering Event",
                            roiDots = listRoi
                        }, Newtonsoft.Json.Formatting.Indented);
                    }
                case Path.ROI_REMOVE:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
                    {
                        nodeId = param1,
                        channelId = param2,
                        roiId = "??????",
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.ROI_UPDATE: return "v2/va/update-roi";
                case Path.ROI_GET:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
                    {
                        nodeId = param1,
                        channelId = param2,
                        roiId = "??????",
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.ROI_LIST:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
                    {
                        nodeId = param1,
                        channelId = param2
                    }, Newtonsoft.Json.Formatting.Indented);
                //link
                case Path.LINK_CREATE:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestLink
                    {
                        nodeId = param1,
                        channelId = param2,
                        roiLink = new List<string> { "roi_1" , "roi_2"}
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.LINK_GET: return "v2/va/get-link";
                case Path.LINK_LIST: return "v2/va/list-link";
                case Path.LINK_REMOVE: return "v2/va/remove-link";
                case Path.LINK_UPDATE: return "v2/va/update-link";
                //va Command
                case Path.VA_COMMAND_START:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestVaStart
                    {
                        nodeId = param1,
                        channelIds = new List<string> { param2 },
                        operation = Test.VAOperations.VA_START
                    }, Newtonsoft.Json.Formatting.Indented);
                case Path.VA_COMMAND_STOP:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestVaStart
                    {
                        nodeId = param1,
                        channelIds = new List<string> { param2 },
                        operation = Test.VAOperations.VA_STOP
                    }, Newtonsoft.Json.Formatting.Indented);
                //system
                case Path.SYSTEM_STATUS:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestCompute
                    {
                        nodeId = param1
                    }, Newtonsoft.Json.Formatting.Indented);

                default: return "";
            }
        }
    }
}
