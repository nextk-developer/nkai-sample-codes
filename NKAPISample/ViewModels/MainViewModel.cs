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
        private string postURL;
        private string hostURL;
        private string requestResult;
        private string responseResult;
        private string nodeID;
        private string channelID;

        private RequestObjectType selectedObject;

        
        private ComputingNodeViewModel _nodeVM;
        private ChannelViewModel _channelVM;
        private ROIViewModel _roiVM;

        public string PostURL { get => postURL; set => SetProperty(ref postURL, value); }
        public string HostURL { get => hostURL; set => SetProperty(ref hostURL, value); }
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
            _nodeVM.PropertyChanged += SubViewModelPropertyChanged;
            _channelVM = new ChannelViewModel(this);
            _channelVM.PropertyChanged += SubViewModelPropertyChanged;
            _roiVM = new ROIViewModel(this);
            _roiVM.PropertyChanged += SubViewModelPropertyChanged;

            // 뷰모델마다 속성 변경 이벤트 각각 처리 필요.
            // 현재 상호참조 되어있어 재귀로 계속 들어옴.

            NodeView.DataContext = _nodeVM;
            ChannelView.DataContext = _channelVM;
            RoIView.DataContext = _roiVM;
        }



        /// <summary>
        /// 서브 뷰모델(node/channel/roi 뷰모델) 값 변경시 이벤트 처리
        /// </summary>
        /// <param name="sender">뷰모델 객체</param>
        /// <param name="e">이벤트 인자</param>
        private void SubViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = this.GetType().GetProperty(e.PropertyName);
            if (property == null)
                return;

            var propertyValue = "";

            if (sender is ComputingNodeViewModel node)
                propertyValue = node.GetType().GetProperty(e.PropertyName).GetValue(node).ToString();

            else if (sender is ChannelViewModel channel)
                propertyValue = channel.GetType().GetProperty(e.PropertyName).GetValue(channel).ToString();

            else if (sender is ROIViewModel roi)
                propertyValue = roi.GetType().GetProperty(e.PropertyName).GetValue(roi).ToString();

            if (property.GetValue(this) != null && property.GetValue(this).ToString().Equals(propertyValue))
                return;

            property.SetValue(this, propertyValue);
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
