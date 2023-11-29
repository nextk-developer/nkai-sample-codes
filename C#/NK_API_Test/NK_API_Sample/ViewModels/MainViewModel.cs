using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using DevExpress.Xpf.Core;
using Newtonsoft.Json;
using NKAPISample.Views;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel.Models;
using NKAPIService.API.ComputingNode;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        string apiVersion = "v2";
        string apiObject = "va";

        public Func<object[]> CreateButtonClicked;
        public Action<string> GetButtonClicked = null;
        public Action<string> RemoveButtonClicked = null;
        private string nodeID;
        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }


        private string postURI;

        public string PostURI { get => postURI; set => SetProperty(ref postURI, value); }

        private string requestResult;

        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }

        private string responseResult;

        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }

        public APITarget SelectedObject { get; set; }

        private ICommand createCommand;
        public ComputingNodeView NodeView { get; private set; }
        public ChannelView ChannelView { get; private set; }
        public ROIView RoIView { get; private set; }
        private ComputingNodeViewModel _nodeVM;
        private ChannelViewModel _channelVM;
        private ROIViewModel _roiVM;

        public MainViewModel()
        {
            NodeView = new ComputingNodeView();
            ChannelView = new ChannelView();
            RoIView = new ROIView();

            _nodeVM = new ComputingNodeViewModel(this);
            _channelVM = new ChannelViewModel(this);
            _roiVM = new ROIViewModel(this);

            NodeView.DataContext = _nodeVM;
            ChannelView.DataContext = _channelVM;
            RoIView.DataContext = _roiVM;
        }

        public ICommand CreateCommand { get { return (createCommand) ?? (createCommand = new DelegateCommand(OnCreate)); } }

        private void OnCreate()
        {
            object[] result = CreateButtonClicked?.Invoke();
            setResult(result);
        }

        private async void setResult(object[] result)
        {
            RequestResult = result[0].ToString();
            //var responseTask = await Task.FromResult(result[1]);
            //ResponseResult = responseTask.ToString();
        }

        private RequestType? getAPIType(string apiType, APITarget apiTarget)
        {
            if (apiType == "Create")
            {
                switch (apiTarget)
                {
                    case APITarget.Node:
                        return RequestType.CreateComputingNode;
                    case APITarget.Channel:
                        return RequestType.RegisterChannel;
                    case APITarget.ROI:
                        return RequestType.CreateROI;
                }
            }
            else if (apiType == "Get")
            {
                switch (apiTarget)
                {
                    case APITarget.Node:
                        return RequestType.GetComputingNode;
                    case APITarget.Channel:
                        return RequestType.GetChannel;
                    case APITarget.ROI:
                        return RequestType.GetROI;
                }
            }
            else if (apiType == "Remove")
            {
                switch (apiTarget)
                {
                    case APITarget.Node:
                        return RequestType.RemoveComputingNode;
                    case APITarget.Channel:
                        return RequestType.RemoveChannel;
                    case APITarget.ROI:
                        return RequestType.RemoveROI;
                }
            }
            else
                throw new ArgumentNullException();

            return null;

        }


    }


}
