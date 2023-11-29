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
    public partial class ChannelViewModel : ObservableObject
    {

        private string nodeID;

        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }


        private string channelID;

        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }

        private string channelName;

        public string ChannelName { get => channelName; set => SetProperty(ref channelName, value); }

        private string channelDescription;

        public string ChannelDescription { get => channelDescription; set => SetProperty(ref channelDescription, value); }

        private bool isAutoTimeout = false;
        public bool IsAutoTimeout { get => isAutoTimeout; set => SetProperty(ref isAutoTimeout, value); }

        private string channelURI;

        public string ChannelURI { get => channelURI; set => SetProperty(ref channelURI, value); }


        private NKAPIService.API.Channel.Models.InputType channelType;

        public NKAPIService.API.Channel.Models.InputType ChannelType { get => channelType; set => SetProperty(ref channelType, value); }

        private string channelGroupName;

        public string ChannelGroupName { get => channelGroupName; set => SetProperty(ref channelGroupName, value); }

        private string requestResult;
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }

        private string responseResult;
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }

        private string postURI;
        public string PostURI { get => postURI; set => SetProperty(ref postURI, value); }

        public ChannelViewModel(MainViewModel mainViewModel)
        {
            mainViewModel.CreateButtonClicked += registerChannelAsync;
            mainViewModel.GetButtonClicked += getChannel;
            mainViewModel.RemoveButtonClicked += removeChannel;
        }


        private void registerChannelAsync()
        {
            ErrorCode errorCode = ErrorCode.REQUEST_TIMEOUT;
            //var cc = new ChannelComponent(this, channelUid: device.Id,
            //                                           groupName: found.Group,
            //                                           channelName: found.Name,
            //                                           inputUrl: responsePlay.RtspUrl,
            //                                           inputUrlSub: responsePlay.RtspUrl);

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

            setPostURI(channel);
            setRequestResult(channel);
            setResponseResult(channel);
        }


        private void getChannel()
        {
            throw new NotImplementedException();
        }

        private void removeChannel()
        {
            throw new NotImplementedException();
        }


        private void setPostURI(IRequest node)
        {
            //string host = $"http://{hostURI}:{hostPort}";
            //PostURI = $"{host}{node.GetResource()}";
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="channel">RequestCreateComputingNode 객체</param>
        private void setRequestResult(IRequest channel)
        {
            if (channel is RequestCreateComputingNode createNode)
                RequestResult = JsonConvert.SerializeObject(createNode, Formatting.Indented);
            else if (channel is RequestGetComputingNode getNode)
                RequestResult = JsonConvert.SerializeObject(getNode, Formatting.Indented);
            else if (channel is RequestRemoveComputingNode removeNode)
                RequestResult = JsonConvert.SerializeObject(removeNode, Formatting.Indented);
        }

        /// <summary>
        /// RequestCreateComputingNode Response Json String 결과값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        /// <returns></returns>
        private async Task setResponseResult(IRequest channel)
        {
            ErrorCode code = ErrorCode.REQUEST_TIMEOUT;
            

            ResponseBase? response = await getResponse(channel);
            if (response != null)
            {
                code = response.Code;
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    ResponseResult = responseResult;
                }
            }
        }

        private async Task<ResponseBase> getResponse(IRequest channel)
        {
            //APIService service = APIService.Build().SetUrl(new Uri($"http://{hostURI}:{hostPort}"));
            
            //if (channel is RequestRegisterChannel createReq)
            //    return await service.Requset(createReq) as ResponseCreateComputingNode;
            //else if (channel is RequestGetChannel getReq)
            //    return await service.Requset(getReq) as ResponseGetComputingNode;
            //else if (channel is RequestRemoveChannel removeReq)
            //    return await service.Requset(removeReq) as ResponseRemoveChannel;

            return null;
        }
    }
}
