using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode.Models;
using NKMeta.Zmq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Channels;

namespace NKAPISample.Models
{
    public partial class NodeComponent
    {
        public NodeComponent()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var localIpResult = host.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();
            HostIP = localIpResult.ToString();
            HostPort = "8880";
            NodeName = "NK_AI";
            CurrentChannel = new();
        }


        #region Properties
        public string NodeId { get; set; }
        public string HostIP { get; set; }
        public string HostPort { get; set; }
        public string HostURL { get => $"http://{HostIP}:{HostPort}"; }
        public string NodeName { get; set; }
        public string LicenseKey { get; set; }


        #endregion

        public ChannelComponent CurrentChannel { get; set; }
        public ZmqMaster MetaClient { get; private set; }

        internal void Clear()
        {
            NodeId = "";
            NodeName = "";
            LicenseKey = "";
        }

        internal void Update(string nodeId, string hostIP, string hostPort, string nodeName, string license)
        {
            NodeId = nodeId;
            HostIP = hostIP;
            HostPort = hostPort;
            NodeName = nodeName;
            LicenseKey = license;
        }


    }
}
