using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using NKAPIService.API.Channel;
using NKAPIService;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NKAPIService.API;
using Newtonsoft.Json;
using NKAPIService.API.ComputingNode;

namespace NKAPISample.ViewModels
{
    public partial class ChannelViewModel : ObservableObject, ICommnunication
    {


        private string nodeID;
        

        private string channelID;
        private string channelName;
        private string channelDescription;
        private bool isAutoTimeout = false;
        private string channelURI;
        private NKAPIService.API.Channel.Models.InputType channelType;
        private string channelGroupName;
        private string requestResult;
        private string responseResult;
        private string postURL; // host url + function domain
        private string hostURL; // host ip + host port

        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }
        public string ChannelName { get => channelName; set => SetProperty(ref channelName, value); }
        public string ChannelDescription { get => channelDescription; set => SetProperty(ref channelDescription, value); }
        public bool IsAutoTimeout { get => isAutoTimeout; set => SetProperty(ref isAutoTimeout, value); }
        public string ChannelURI { get => channelURI; set => SetProperty(ref channelURI, value); }
        public NKAPIService.API.Channel.Models.InputType ChannelType { get => channelType; set => SetProperty(ref channelType, value); }
        public string ChannelGroupName { get => channelGroupName; set => SetProperty(ref channelGroupName, value); }
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string PostURL { get => postURL; set => SetProperty(ref postURL, value); }
        public string HostURL { get => hostURL; set => SetProperty(ref hostURL, value); }

        public ChannelViewModel(MainViewModel mainViewModel)
        {
            mainViewModel.PropertyChanged += MainViewModelPropertyChanged;
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = this.GetType().GetProperty(e.PropertyName);
            if (property == null)
                return;

            var main = sender as MainViewModel;
            var propertyValue = main.GetType().GetProperty(e.PropertyName).GetValue(main).ToString();
            property.SetValue(this, propertyValue);
        }


        public void CreateObject()
        {
            var channel = new RequestRegisterChannel()
            {
                NodeId = nodeID,
                InputType = channelType,
                GroupName = channelGroupName,
                Description = channelDescription,
                ChannelName = channelName,
                InputUrl = this.channelURI,
                AutoTimeout = isAutoTimeout
            };

            SetPostURL(channel);
            SetRequestResult(channel);
            SetResponseResult(channel);
        }


        public void GetObject()
        {
            var requestChannel = new RequestGetChannel()
            {

            } as RequestGetChannel;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }

        public void RemoveObject()
        {
            var requestChannel = new RequestRemoveChannel()
            {

            } as RequestRemoveChannel;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }


        public void SetPostURL(IRequest node)
        {
            PostURL = $"{HostURL}{node.GetResource()}";
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="channel">RequestCreateComputingNode 객체</param>
        public void SetRequestResult(IRequest channel)
        {
            if (channel is RequestCreateComputingNode createReq)
                RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (channel is RequestGetComputingNode getReq)
                RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (channel is RequestRemoveComputingNode removeReq)
                RequestResult = JsonConvert.SerializeObject(removeReq, Formatting.Indented);
        }

        /// <summary>
        /// RequestCreateComputingNode Response Json String 결과값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        /// <returns></returns>
        public async Task SetResponseResult(IRequest channel)
        {
            ResponseBase? response = await GetResponse(channel);
            if (response != null)
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    ResponseResult = responseResult;
                }
            }
        }

        public async Task<ResponseBase> GetResponse(IRequest channel)
        {
            
            APIService service = APIService.Build().SetUrl(new Uri(HostURL));

            if (channel is RequestRegisterChannel createReq)
                return await service.Requset(createReq) as ResponseRegisterChannel;
            else if (channel is RequestGetChannel getReq)
                return await service.Requset(getReq) as ResponseGetChannel;
            else if (channel is RequestRemoveChannel removeReq)
                return await service.Requset(removeReq) as ResponseRemoveChannel;

            return null;
        }

    }
}
