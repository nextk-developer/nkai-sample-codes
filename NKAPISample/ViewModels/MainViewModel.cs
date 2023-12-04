using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        
        private string hostURL;
        private string hostIP = "127.0.0.1";
        private string hostPort = "8880";
        private string postURL;
        private string requestResult;
        private string responseResult;
        private string nodeID;
        private string channelID;
        private string channelURL = Resources.RTSPDefaultAddress;
        private List<string> roiIDs;

        private ComputingNodeViewModel _NodeVM;
        private ChannelViewModel _ChannelVM;
        private ScheduleViewModel _ScheduleVM;
        private ROIViewModel _RoiVM;
        private VAViewModel _VaVM;
        private VideoViewModel _VideoVM;
        private DrawingViewModel _DrawingVM;
        private StringBuilder _Builder;
        internal Action VAStarted;

        public string HostIP { get => hostIP; set => SetProperty(ref hostIP, value); }
        public string HostPort { get => hostPort; set => SetProperty(ref hostPort, value); }
        public string HostURL { get => $"http://{hostIP}:{hostPort}"; }
        public string PostURL { get => postURL; set => SetProperty(ref postURL, value); }
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }
        public string ChannelURL { get => channelURL; set => SetProperty(ref channelURL, value); }
        public List<string> RoiIDs { get => roiIDs; set => SetProperty(ref roiIDs, value); }
        


        public ComputingNodeView NodeView { get; private set; }
        public ChannelView ChannelView { get; private set; }
        public ScheduleView ScheduleView { get; private set; }
        public ROIView RoIView { get; private set; }
        public VAView VAView { get; private set; }
        public VideoView VideoView { get; private set; }
        public DrawingView DrawingView { get; private set; }



        public MainViewModel()
        {
            roiIDs = new List<string>();
            NodeView = new ComputingNodeView();
            ChannelView = new ChannelView();
            RoIView = new ROIView();
            ScheduleView = new ScheduleView();
            VAView = new VAView();
            VideoView = new VideoView();
            DrawingView = new DrawingView();

            _NodeVM = new ComputingNodeViewModel(this);
            _ChannelVM = new ChannelViewModel(this);
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
        }

        private void StartVA()
        {
            _VideoVM.Start();
        }

        internal void SetResponseResult(string responseResult)
        {
            _Builder.AppendLine(responseResult);
            _Builder.AppendLine($"---------- {DateTime.Now.ToString("G")}");
            ResponseResult = _Builder.ToString();
        }


    }




}
