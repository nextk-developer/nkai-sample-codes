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

        private string postURI;

        public string PostURI { get => postURI; set => SetProperty(ref postURI, value); }

        private string requestResult;

        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }

        private string responseResult;

        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }


        private string nodeID;
        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }

        private string channelID;
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }


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
            _nodeVM.PropertyChanged += SubViewModelPropertyChanged;
            _channelVM = new ChannelViewModel(this);
            _roiVM = new ROIViewModel(this);

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

            property.SetValue(this, propertyValue);
        }


    }


    /// <summary>
    /// 이벤트 및 커맨드 
    /// </summary>
    public partial class MainViewModel
    {

        public Action CreateButtonClicked;
        public Action GetButtonClicked = null;
        public Action RemoveButtonClicked = null;


        private ICommand createCommand;

        public ICommand CreateCommand { get { return (createCommand) ?? (createCommand = new DelegateCommand(Create)); } }

        private void Create()
        {
            CreateButtonClicked?.Invoke();
        }

        private DelegateCommand getCommand;
        public ICommand GetCommand => getCommand ??= new DelegateCommand(Get);

        private void Get()
        {
        }
    }


}
