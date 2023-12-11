using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using NKAPISample.Models;
using NKAPIService;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;
using System.Text;

namespace NKAPISample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private string _RequestResult;
        private string _ResponseResult;
        private string _MetadataLog;
        private StringBuilder _ResponseResultBuilder;
        private StringBuilder _MetadataLogBuilder;
        private string _PostURL;
        internal Action<APIService> VAStarted;
        internal Action VAStopped;
        internal Action<DrawingType> ROIEventTypeSelected;

        public NodeComponent CurrentNode { get; set; }
        
        public string RequestResult { get => _RequestResult; set => SetProperty(ref _RequestResult, value); }
        public string ResponseResult { get => _ResponseResult; set => SetProperty(ref _ResponseResult, value); }
        public string MetadataLog { get => _MetadataLog; set => SetProperty(ref _MetadataLog, value); }
        public string PostURL { get => _PostURL; set => SetProperty(ref _PostURL, value); }

        #region viewmodels

        public VAViewModel VAVM { get => Ioc.Default.GetRequiredService<VAViewModel>(); }

        public ROIViewModel ROIVM { get => Ioc.Default.GetRequiredService<ROIViewModel>(); }

        public ScheduleViewModel ScheduleVM { get => Ioc.Default.GetRequiredService<ScheduleViewModel>(); }

        public ChannelViewModel ChannelVM { get => Ioc.Default.GetRequiredService<ChannelViewModel>(); }

        public ComputingNodeViewModel NodeVM { get => Ioc.Default.GetRequiredService<ComputingNodeViewModel>(); }

        public VideoViewModel VideoVM { get => Ioc.Default.GetRequiredService<VideoViewModel>(); }
        #endregion



        public MainViewModel()
        {
            NodeComponent node = new();
            CurrentNode = node;            
            _ResponseResultBuilder = new StringBuilder();
            _MetadataLogBuilder = new();
            VAStarted += StartVAAsync;
        }


        private void StartVAAsync(APIService service)
        {
            if(CurrentNode.CurrentChannel.VAControlStart(service).Result == NKAPIService.API.ErrorCode.SUCCESS)
                VideoVM.VAStart(CurrentNode.CurrentChannel);
        }

        internal void SetResponseResult(string responseResult)
        {
            _ResponseResultBuilder.AppendLine(responseResult);
            _ResponseResultBuilder.AppendLine($"---------- {DateTime.Now.ToString("G")}");
            ResponseResult = _ResponseResultBuilder.ToString();
        }

        internal void SetPostURL(string v)
        {
            PostURL = v;
        }

        internal void ClearChannel()
        {
            CurrentNode.CurrentChannel.Clear();
        }

        internal void UpdateChannel(string channelId, string mediaServerUrl, string mediaServerUrlSub)
        {
            CurrentNode.CurrentChannel.Update(CurrentNode, channelId, mediaServerUrl, mediaServerUrlSub);
            VideoVM.VideoStart(mediaServerUrl);
        }

        internal void ClearNode()
        {
            CurrentNode.Clear();
        }

        internal void UpdateNode(string nodeId, string hostIP, string hostPort, string nodeName, string license)
        {
            CurrentNode.Update(nodeId, hostIP, hostPort, nodeName, license);
        }

        internal void Close()
        {
            //_ChannelVM.RemoveChannel();
            //_NodeVM.RemoveNode();
        }

        internal void AddRoi(string nodeId, string channelID, string rOIID, IntegrationEventType eventType, PredefineConstant.Enum.Analysis.ObjectType objectType, List<ROIDot> roiDots, List<ROIDot> roiDotsSub, 
            string roiName, ROIFeature roiFeature, RoiNumber roiNumber, DrawingType roiType, EventFilter eventFilter)
        {

            CurrentNode.CurrentChannel.AddROI(nodeId, channelID, rOIID, eventType, objectType, roiDots, roiDotsSub, roiName, roiFeature, roiNumber, roiType, eventFilter);
        }

        internal string GetCurrentRoiID()
        {
            return CurrentNode.CurrentChannel.CurrentROI.UID;
        }

        internal void SetCurrentRoi(RoiModel roi)
        {
            CurrentNode.CurrentChannel.AddROI(CurrentNode.NodeId, CurrentNode.CurrentChannel.ChannelUid, roi.RoiId, roi.EventType, PredefineConstant.Enum.Analysis.ObjectType.Person, roi.RoiDots, roi.RoiDotsSub, roi.RoiName, roi.RoiFeature, roi.RoiNumber, roi.RoiType, roi.EventFilter);
        }

        internal void RemoveRoi()
        {
            CurrentNode.CurrentChannel.InitRoi();
        }

        internal void SetMetadataLog(Progress eventStatus, ClassId classID, int eventID, IntegrationEventType eventType, string roiName, System.Drawing.Rectangle position)
        {
            _MetadataLogBuilder.AppendLine($"[{eventStatus}] {classID} {eventID}, {eventType}, X: {position.X}, Y: {position.Y}, Width: {position.Width}, Height: {position.Height}");
            MetadataLog = _MetadataLogBuilder.ToString();
        }
    }




}
