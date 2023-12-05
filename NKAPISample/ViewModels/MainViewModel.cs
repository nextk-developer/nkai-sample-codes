using CommunityToolkit.Mvvm.ComponentModel;
using NKAPISample.Models;
using NKAPISample.Properties;
using NKAPISample.Views;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Extenstion;
using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private string requestResult;
        private string responseResult;

        private ComputingNodeViewModel _NodeVM;

        private ChannelViewModel _ChannelVM;

        private ScheduleViewModel _ScheduleVM;
        private ROIViewModel _RoiVM;
        public ROIViewModel RoI { get => _RoiVM; } 
        private VAViewModel _VaVM;
        private VideoViewModel _VideoVM;
        private DrawingViewModel _DrawingVM;
        private StringBuilder _Builder;
        internal Action VAStarted;
        internal Action VAStoped;
        private string _PostURL;

        public NodeModel Node { get; set; }
        public ChannelModel Channel { get; set; }
        

        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string PostURL { get => _PostURL; set => SetProperty(ref _PostURL, value); }


        public ComputingNodeView NodeView { get; private set; }
        public ChannelView ChannelView { get; private set; }
        public ScheduleView ScheduleView { get; private set; }
        public ROIView RoIView { get; private set; }
        public VAView VAView { get; private set; }
        public VideoView VideoView { get; private set; }
        public DrawingView DrawingView { get; private set; }



        public MainViewModel()
        {
            NodeView = new ComputingNodeView();
            ChannelView = new ChannelView();
            RoIView = new ROIView();
            ScheduleView = new ScheduleView();
            VAView = new VAView();
            VideoView = new VideoView();
            DrawingView = new DrawingView();

            NodeModel node = new();
            ChannelModel channel = new(node);
            Node = node;
            Channel = channel;
            _NodeVM = new ComputingNodeViewModel(this, node) ;
            _ChannelVM = new ChannelViewModel(this, channel);
            _ScheduleVM = new ScheduleViewModel(this);
            _RoiVM = new ROIViewModel(this);
            _VaVM = new VAViewModel(this);
            _VideoVM = new VideoViewModel(this);
            _DrawingVM = new DrawingViewModel(this);

            _Builder = new StringBuilder();
            NodeView.DataContext = _NodeVM;
            ChannelView.DataContext = _ChannelVM;
            ScheduleView.DataContext = _ScheduleVM;
            RoIView.DataContext = _RoiVM;
            VAView.DataContext = _VaVM;
            VideoView.DataContext = _VideoVM;
            DrawingView.DataContext = _DrawingVM;
            VAStarted += StartVA;
            VAStoped += StopVA;
        }

        private void StopVA()
        {
            _VideoVM.Stop();
        }

        private void StartVA()
        {
            _VideoVM.Start(Channel.MediaUrl);
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
            Channel.Clear();
        }

        internal void UpdateChannel(string channelId, string mediaServerUrl, string mediaServerUrlSub)
        {
            Channel.Update(channelId, mediaServerUrl, mediaServerUrlSub);
        }

        internal void ClearNode()
        {
            Node.Clear();
        }

        internal void UpdateNode(string nodeId, string hostIP, string hostPort, string nodeName, string license)
        {
            Node.Update(nodeId, hostIP, hostPort, nodeName, license);
        }

        internal void Close()
        {
            //_ChannelVM.RemoveChannel();
            //_NodeVM.RemoveNode();
        }
    }




}
