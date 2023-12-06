using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using NKAPISample.Models;
using NKAPISample.Views;
using NKAPIService.API.ComputingNode.Models;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
using System.Windows.Forms;
using Vortice.MediaFoundation;

namespace NKAPISample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private string requestResult;
        private string responseResult;

        private StringBuilder _Builder;
        internal Action<string> VAStarted;
        internal Action VAStopped;
        private string _PostURL;

        public NodeComponent CurrentNode { get; set; }
        
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string PostURL { get => _PostURL; set => SetProperty(ref _PostURL, value); }

        #region viewmodels


        public DrawingViewModel DrawingVM { get => Ioc.Default.GetRequiredService<DrawingViewModel>(); }

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
            _Builder = new StringBuilder();
            //VAStarted += StartVA;
            //VAStopped += StopVA;

        }

        private void StopVA()
        {
            throw new NotImplementedException();
        }

        private void StartVA(string obj)
        {
            
        }

        internal void SetResponseResult(string responseResult)
        {
            _Builder.AppendLine(responseResult);
            _Builder.AppendLine($"---------- {DateTime.Now.ToString("G")}");
            ResponseResult = _Builder.ToString();
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
    }




}
