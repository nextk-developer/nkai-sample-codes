using NKAPISample.Properties;
using NKAPIService.API;
using NKAPIService.API.VideoAnalysisSetting;
using NKMeta;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ObjectType = PredefineConstant.Enum.Analysis.ObjectType;

namespace NKAPISample.Models
{
    public class ChannelComponent
    {
        

        public ChannelComponent()
        {
            InputUrl = Resources.RTSPDefaultAddress;
            InitRoi();
        }

        public NodeComponent ParentNode { get; private set; }
        private List<RoiComponent> _RoiList = new();
        public List<RoiComponent> RoiList { get => _RoiList; set => _RoiList = value; }

        public RoiComponent CurrentROI { get; private set; }

        public IMetaData ObjectMetaClient { get; private set; }
        #region Properties
        public string NodeID { get; private set; }
        public string ChannelUid { get; private set; }
        public string GroupName { get; private set; }
        public string ChannelName { get; private set; }
        public string InputUrl { get; private set; }
        public string InputUrlSub { get; private set; }

        public string MediaUrl { get; private set; }
        public string MediaUrlSub { get; private set; }
        public string SourceIP { get; private set; }
        
        


        internal void Clear()
        {
            ParentNode = null;
            NodeID = "";
            ChannelUid = "";
            MediaUrl = "";
            MediaUrlSub = "";
            CurrentROI = new();
        }

        internal void Update(NodeComponent node, string channelId, string mediaServerUrl, string mediaServerUrlSub)
        {
            ParentNode = node;
            NodeID = node.NodeId;
            ChannelUid = channelId;
            MediaUrl = mediaServerUrl;
            MediaUrlSub = mediaServerUrlSub;
        }


        #region roi

        internal void InitRoi()
        {
            CurrentROI = new();
            _RoiList.Clear();
        }


        internal void SetRoi(List<RoiModel> items)
        {
            _RoiList.Clear();
            foreach (var roi in items)
            {
                RoiComponent rc = new(NodeID, ChannelUid, roi.RoiName, roi.RoiId, roi.EventType, roi.ObjectType, roi);
                _RoiList.Add(rc);
            }


            CurrentROI = _RoiList.LastOrDefault();
        }


        internal void AddROI(RoiModel roi)
        {
            RoiComponent rc = new(NodeID, ChannelUid, roi.RoiName, roi.RoiId, roi.EventType, roi.ObjectType, roi);
            _RoiList.Add(rc);
            CurrentROI = rc;
        }

        private void UpdateROI(RoiComponent rc)
        {
            CurrentROI = rc;
            //RoIs[rc.UID] = rc;
        }

        #endregion

        private void StartMetaService(string host)
        {
            if (string.IsNullOrEmpty(host)) return;

            IMetaData metaClient = new NKMeta.Zmq.ObjectMetaClient(NodeID, ChannelUid, ChannelName, host);
            ObjectMetaClient = metaClient;
            metaClient?.StartTask();
        }

        internal Task<ErrorCode> VAControlStart(ResponseControl res)
        {
            return Task.Run(async () =>
            {
                StartMetaService($"{res.SourceIp}:{res.SourcePort}");
                return ErrorCode.SUCCESS;
            });
        }



        #endregion
    }
}
