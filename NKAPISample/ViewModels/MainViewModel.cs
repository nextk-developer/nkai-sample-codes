using CommunityToolkit.Mvvm.ComponentModel;
using NKAPISample.Views;
using SharpGen.Runtime;
using System;
using System.Collections.Generic;
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
        private string channelURL = "rtsp://211.198.128.30/vod/line1";
        private List<string> roiIDs;

        private RequestObjectType selectedObject;

        
        private ComputingNodeViewModel _nodeVM;
        private ChannelViewModel _channelVM;
        private ScheduleViewModel _scheduleVM;
        private ROIViewModel _roiVM;
        private VAViewModel _vaVM;
        private StringBuilder _builder;

        public string HostIP { get => hostIP; set => SetProperty(ref hostIP, value); }
        public string HostPort { get => hostPort; set => SetProperty(ref hostPort, value); }
        public string HostURL { get => $"http://{hostIP}:{hostPort}"; }
        public string PostURL { get => postURL; set => SetProperty(ref postURL, value); }
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }
        public string ChannelURL { get => channelURL; set => SetProperty(ref channelURL, value); }
        public RequestObjectType SelectedObject { get => selectedObject; set => SetProperty(ref selectedObject, value); }
        public List<string> RoiIDs { get => roiIDs; set => SetProperty(ref roiIDs, value); }

        

        public ComputingNodeView NodeView { get; private set; }
        public ChannelView ChannelView { get; private set; }
        public ScheduleView ScheduleView { get; private set; }
        public ROIView RoIView { get; private set; }
        public VAView VAView { get; private set; }



        public MainViewModel()
        {
            roiIDs = new List<string>();
            NodeView = new ComputingNodeView();
            ChannelView = new ChannelView();
            RoIView = new ROIView();
            ScheduleView = new ScheduleView();
            VAView = new VAView();

            _nodeVM = new ComputingNodeViewModel(this);
            _channelVM = new ChannelViewModel(this);
            _scheduleVM = new ScheduleViewModel(this);
            _roiVM = new ROIViewModel(this);
            _vaVM = new VAViewModel(this);

            _builder = new StringBuilder();
            NodeView.DataContext = _nodeVM;
            ChannelView.DataContext = _channelVM;
            ScheduleView.DataContext = _scheduleVM;
            RoIView.DataContext = _roiVM;
            VAView.DataContext = _vaVM;
        }

        internal void SetResponseResult(string responseResult)
        {
            _builder.AppendLine(responseResult);
            _builder.AppendLine($"---------- {DateTime.Now.ToString("G")}");
            ResponseResult = _builder.ToString();
        }
    }




}
