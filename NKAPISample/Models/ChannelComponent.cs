using NKAPISample.Properties;
using NKAPIService.API.Channel;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Camera;
using PredefineConstant.Model.Camera;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NKAPIService.API.VideoAnalysisSetting;
using Vortice.MediaFoundation;
using NKMeta;
using NKAPIService.API.ComputingNode.Models;
using NKAPIService.API;

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
        }


        internal void AddROI(string nodeId, string channelID, string rOIID, IntegrationEventType eventType, PredefineConstant.Enum.Analysis.ObjectType objectType, List<ROIDot> roiDots, List<ROIDot> roiDotsSub,
            string roiName, ROIFeature roiFeature, RoiNumber roiNumber, DrawingType roiType, EventFilter eventFilter)
        {
            PredefineConstant.Model.ROI roi = new PredefineConstant.Model.ROI()
            {
                DrawingType = roiType,
                Points = roiDots?.Select(d => new PointF((float)d.X, (float)d.Y)).ToList(),
                PointsSub = roiDotsSub?.Select(d => new PointF((float)d.X, (float)d.Y)).ToList(),
            };

            RoiComponent rc = new(nodeId, channelID, roiName, roiNumber, rOIID, eventType, objectType, roi, eventFilter);
            CurrentROI = rc;
            //if (RoIs.ContainsKey(rOIID))
            //    UpdateROI(rc);
            //else if (RoIs.Count == 1 && RoIs.First().Key == "")
            //{
            //    RoIs[""] = rc;
            //}
            //else
            //{
            //    RoIs.Add(rc.UID, rc);
            //}
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

        internal Task<ErrorCode> VAControlStart(NKAPIService.APIService service)
        {
            return Task.Run(async () =>
            {
                ErrorCode errorCode = ErrorCode.SUCCESS;

                if (CurrentROI != null)
                {
                    // 여기도 해야됨 ..
                    if (string.IsNullOrEmpty(ParentNode.NodeId))
                        return ErrorCode.NOT_FOUND_COMPUTING_NODE;

                    errorCode = ErrorCode.REQUEST_TIMEOUT;
                    // 시작 보내줘야함
                    var responseControl = await service.Requset(new RequestControl()
                    {
                        NodeId = ParentNode.NodeId,
                        ChannelIDs = new List<string>() { ChannelUid },
                        Operation = Operations.VA_START,
                    }) as ResponseControl;

                    if (responseControl != null)
                    {
                        errorCode = responseControl.Code;

                        if (responseControl.Code == ErrorCode.SUCCESS)
                            StartMetaService($"{responseControl.SourceIp}:{responseControl.SourcePort}");
                    }
                }


                return errorCode;
            });
        }

        #endregion
    }
}
