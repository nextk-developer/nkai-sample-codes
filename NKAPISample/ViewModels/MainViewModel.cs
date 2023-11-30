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
        
        private string hostURL;
        private string hostIP = "127.0.0.1";
        private string hostPort = "8880";
        private string postURL;
        private string requestResult;
        private string responseResult;
        private string nodeID;
        private string channelID;

        private RequestObjectType selectedObject;

        
        private ComputingNodeViewModel _nodeVM;
        private ChannelViewModel _channelVM;
        private ROIViewModel _roiVM;

        
        
        public string HostIP { get => hostIP; set => SetProperty(ref hostIP, value); }
        public string HostPort { get => hostPort; set => SetProperty(ref hostPort, value); }
        public string HostURL { get => $"http://{hostIP}:{hostPort}/"; }
        public string PostURL { get => postURL; set => SetProperty(ref postURL, value); }
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }
        public RequestObjectType SelectedObject { get => selectedObject; set => SetProperty(ref selectedObject, value); }

        

        public ComputingNodeView NodeView { get; private set; }
        public ChannelView ChannelView { get; private set; }
        public ROIView RoIView { get; private set; }



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

    }


    /// <summary>
    /// 이벤트 및 커맨드 
    /// </summary>
    public partial class MainViewModel
    {
        private DelegateCommand createCommand;
        private DelegateCommand getCommand;
        private DelegateCommand removeCommand;
        public ICommand CreateCommand => createCommand ??= new DelegateCommand(Create);
        public ICommand GetCommand => getCommand ??= new DelegateCommand(Get);
        public ICommand RemoveCommand => removeCommand ??= new DelegateCommand(Remove);



        private void Create()
        {
            switch(SelectedObject)
            {
                case RequestObjectType.Node:
                    _nodeVM.CreateObject();
                    break;
                case RequestObjectType.Channel:
                    _channelVM.CreateObject();
                    break;
                case RequestObjectType.RoI:
                    _roiVM.CreateObject();
                    break; ;
            }
        }


        private void Get()
        {
            switch (SelectedObject)
            {
                case RequestObjectType.Node:
                    _nodeVM.GetObject();
                    break;
                case RequestObjectType.Channel:
                    _channelVM.GetObject();
                    break;
                case RequestObjectType.RoI:
                    _roiVM.GetObject();
                    break; ;
            }
        }


        private void Remove()
        {
            switch (SelectedObject)
            {
                case RequestObjectType.Node:
                    _nodeVM.RemoveObject();
                    break;
                case RequestObjectType.Channel:
                    _channelVM.RemoveObject();
                    break;
                case RequestObjectType.RoI:
                    _roiVM.RemoveObject();
                    break; ;
            }
        }
    }


}
