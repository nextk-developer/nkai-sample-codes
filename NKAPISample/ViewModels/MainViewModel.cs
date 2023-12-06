using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using NKAPISample.Models;
using NKAPISample.Views;
using System;
using System.Collections.Generic;
using System.Text;

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
            CurrentNode.CurrentChannel.Update(channelId, mediaServerUrl, mediaServerUrlSub);
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


    }




}
